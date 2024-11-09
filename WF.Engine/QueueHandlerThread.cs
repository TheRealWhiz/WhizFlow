using System;
using System.Threading;
using System.Data.SqlClient;
using Whiz.WhizFlow.Common;
using System.Xml.Serialization;
using System.IO;
using System.Timers;
using Whiz.WhizFlow.Common.Objects;
using System.Collections.Concurrent;
using Whiz.Framework.Configuration;

namespace Whiz.WhizFlow.Engine.Modules
{
	/// <summary>
	/// Queue Handler Thread
	/// </summary>
	public class QueueHandlerThread : IDisposable
	{
		/// <summary>
		/// Internal timer for queue polling
		/// </summary>
		private System.Timers.Timer _timer = null;
		/// <summary>
		/// Flag that indicates if the thread is already running
		/// </summary>
		private Boolean _statusRunning;
		/// <summary>
		/// The Thread on which all the process occurs
		/// </summary>
		private Thread _thread;
		/// <summary>
		/// Flag that indicates that the Queue Handler Thread was stopped by WMI or internal instrumentation
		/// </summary>
		private Boolean _instrumentationStopped = true;
		/// <summary>
		/// The Queue Processor that will process all the files
		/// </summary>
		private QueueProcessor _taskProcessor;
		/// <summary>
		/// Monitor object
		/// </summary>
		private Monitoring.QueueHandlerThread _monitor;
		/// <summary>
		/// Performance counter of the tasks received
		/// </summary>
		private System.Diagnostics.PerformanceCounter _tasks;
		/// <summary>
		/// Performance counter of the tasks processed per second
		/// </summary>
		private System.Diagnostics.PerformanceCounter _tasksPerSecond;
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		private String _serviceName;
		/// <summary>
		/// WhizFlow domain name
		/// </summary>
		private String _domainName;
		/// <summary>
		/// The queue assigned to the thread
		/// </summary>
		public String Queue { get; set; }
		/// <summary>
		/// The WhizFlow main configuration for this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// The frequency for polling the assigned queue
		/// </summary>
		public Double Frequency
		{
			get
			{
				return _timer.Interval;
			}
			set
			{
				_timer.Interval = value;
			}
		}
		/// <summary>
		/// Continue to retrieve data until data is found when the task is running repeating the task cycle
		/// </summary>
		public Boolean RepeatCycle { get; set; }
		/// <summary>
		/// A value indicating how many tasks he will retrieve (as a maximum) from the database each time
		/// </summary>
		public Int32 NumberOfTasks { get; set; }
		/// <summary>
		/// Contains the last task id which processing results in an error
		/// </summary>
		private Int32 _lastTaskContentIdWithError;
		/// <summary>
		/// Calculated sp for read in case of dedicated queue
		/// </summary>
		private String _dedicatedSPRead;
		/// <summary>
		/// Calculated sp for delete in case of dedicated queue
		/// </summary>
		private String _dedicatedSPDelete;
		/// <summary>
		/// Contructor. If you use this constructor you MUST set all the class property manually before invoke the Start method
		/// </summary>
		public QueueHandlerThread()
		{
			_timer = new System.Timers.Timer();
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="serviceName">WhizFlow service instance name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		/// <param name="queueName">The assigned queue to manage</param>
		/// <param name="configuration">WhizFlow Configuration file</param>
		/// <param name="frequency">Frequency of check for tasks</param>
		/// <param name="repeatCycle">Retrieve data until data is found when the task is running</param>
		/// <param name="numberOfTasks">A value indicating how many tasks he will retrieve (as a maximum) from the database each time</param>
		/// <param name="queueConfiguration">The configuration for the QueueProcessor</param>
		/// <param name="dedicated">Indicator if the queue should be a dedicated queue</param>
		public QueueHandlerThread(String serviceName, String domainName, String queueName, GenericConfiguration configuration, Double frequency, Boolean repeatCycle, Int32 numberOfTasks, Boolean dedicated, GenericConfiguration queueConfiguration)
		{
			_serviceName = serviceName;
			_domainName = domainName;
			_timer = new System.Timers.Timer();
			Queue = queueName;
			_configuration = configuration;
			_connectionString = configuration.Get("db").Value;
			_timer.Interval = frequency;
			RepeatCycle = repeatCycle;
			NumberOfTasks = numberOfTasks;
			_taskProcessor = new QueueProcessor(configuration, queueConfiguration, queueName);
			InitializeMonitoring(serviceName, domainName, queueName, frequency, dedicated ? "Dedicated" : "Standard");
			if (dedicated)
			{
				_dedicatedSPRead = "dbo.WF_Tasks_" + Queue + "_Read";
				_dedicatedSPDelete = "dbo.WF_Tasks_" + Queue + "_Remove";
				// I'll try to setup just in case the structure doesn't exists yet
				DAL.SetupDedicatedQueue(queueName, _connectionString);
				_timer.Elapsed += new ElapsedEventHandler(TimerEventDedicated);
			}
			else
			{
				_timer.Elapsed += new ElapsedEventHandler(TimerEvent);
			}
		}
		/// <summary>
		/// Initialize monitoring
		/// </summary>
		/// <param name="serviceName">WhizFlow service instance name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		/// <param name="queueName">Queue</param>
		/// <param name="frequency">Frequency</param>
		/// <param name="mode">Working mode</param>
		private void InitializeMonitoring(String serviceName, String domainName, String queueName, Double frequency, String mode)
		{
			_monitor = new Monitoring.QueueHandlerThread(queueName, serviceName, domainName, frequency, mode);
			_instrumentationStopped = false;
			_tasks = new System.Diagnostics.PerformanceCounter(Monitoring.PerformanceCounters.Utilities.FormatPerformanceCounterCategoryName(_serviceName), Monitoring.PerformanceCounters.Utilities.RECEIVED_TASKS_PERFORMANCE_COUNTER_NAME, String.Format("Tasks_{1}_{0}", Queue, domainName), false);
			_tasks.RawValue = 0;
			_tasksPerSecond = new System.Diagnostics.PerformanceCounter(Monitoring.PerformanceCounters.Utilities.FormatPerformanceCounterCategoryName(_serviceName), Monitoring.PerformanceCounters.Utilities.RECEIVED_TASKS_PER_SECOND_PERFORMANCE_COUNTER_NAME, String.Format("Tasks_Per_Second_{1}_{0}", Queue, domainName), false);
			_tasksPerSecond.NextSample();
		}
		/// <summary>
		/// Starts the QueueHandlerThread
		/// </summary>
		public void Start()
		{
			_timer.Start();
			_monitor.Status = "Running";
		}
		/// <summary>
		/// Stops future tasks handling. Current processing will be completed as normal.
		/// </summary>
		public void Stop()
		{
			_timer.Stop();
			_monitor.Status = "Stopped";
		}
		/// <summary>
		/// Timer event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerEvent(object sender, EventArgs e)
		{
			_monitor.LastRunning = DateTime.Now.ToString();
			_timer.Enabled = false;
			try
			{
				if (!_statusRunning)
				{
					_statusRunning = true;
					_thread = new Thread(Task);
					//_thread.SetApartmentState(ApartmentState.STA);
					_thread.Start();
				}
			}
			catch (Exception ex)
			{
				Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", "Queue Handler Thread Timer Event " + Queue + " Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
				Monitoring.Events.WhizFlowError pEvent = new Monitoring.Events.WhizFlowError();
				pEvent.WhizFlow = _serviceName;
				pEvent.Error = "Queue Handler Thread error";
				pEvent.InternalException = ex.Message + Environment.NewLine + ex.StackTrace;
				pEvent.Module = "QueueHandlerThread";
				pEvent.Fire();
			}
			_timer.Enabled = !_instrumentationStopped;
		}
		/// <summary>
		/// Dedicated queue timer event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerEventDedicated(object sender, EventArgs e)
		{
			_monitor.LastRunning = DateTime.Now.ToString();
			_timer.Enabled = false;
			try
			{
				if (!_statusRunning)
				{
					_statusRunning = true;
					_thread = new Thread(TaskDedicated);
					_thread.Start();
				}
			}
			catch (Exception ex)
			{
				Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", "Queue Handler Thread Timer Event " + Queue + " Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
				Monitoring.Events.WhizFlowError pEvent = new Monitoring.Events.WhizFlowError();
				pEvent.WhizFlow = _serviceName;
				pEvent.Error = "Queue Handler Thread error";
				pEvent.InternalException = ex.Message + Environment.NewLine + ex.StackTrace;
				pEvent.Module = "QueueHandlerThread";
				pEvent.Fire();
			}
			_timer.Enabled = !_instrumentationStopped;
		}
		/// <summary>
		/// Converts from the database saved task to a deserialized task (for content different from string type)
		/// </summary>
		/// <param name="serializedContent">The serialized content</param>
		/// <returns>The deserialized object</returns>
		private Object DeserializeContent(String serializedContent)
		{
			Object result;
			XmlSerializer xmlSer = new XmlSerializer(typeof(Object));
			result = (Object)xmlSer.Deserialize(new StringReader(serializedContent));
			return result;
		}
		/// <summary>
		/// Gets the task content from db or disk as indicated
		/// </summary>
		/// <param name="dbContent">The Content field</param>
		/// <returns>The task content</returns>
		private String GetContent(String dbContent)
		{
			StreamReader file = File.OpenText(dbContent);
			return file.ReadToEnd();
		}
		/// <summary>
		/// The routine used inside the thread that will handle the queue
		/// </summary>
		private void Task()
		{
			System.Diagnostics.Stopwatch taskStopWatch = new System.Diagnostics.Stopwatch();
			Int32 n = 0;
			Int32 tasksCompleted = 0;
			Boolean repeat = true;
			Boolean logError = true;
			_taskProcessor.LastTaskContentIdWithError = _lastTaskContentIdWithError;
			try
			{
				while (repeat)
				{
					repeat = false;
					using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(_connectionString))
					{
						sqlConn.Open();
						System.Data.SqlClient.SqlCommand sqlCom = new System.Data.SqlClient.SqlCommand("dbo.WF_Tasks_Read", sqlConn);
						sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
						sqlCom.Parameters.Add("Queue", System.Data.SqlDbType.VarChar, 50).Value = Queue;
						sqlCom.Parameters.Add("RowCount", System.Data.SqlDbType.Int).Value = NumberOfTasks;
						using (System.Data.SqlClient.SqlDataReader drRead = sqlCom.ExecuteReader())
						{
							if (drRead.Read())
							{
								_monitor.ItemsInQueue = Int32.Parse(drRead["QueueItems"].ToString());
							}
							drRead.NextResult();
							WhizFlowTask task = null;
							while (drRead.Read())
							{
								taskStopWatch.Reset();
								taskStopWatch.Start();
								n++;
								task = new WhizFlowTask();
								if (drRead["Serialized"].ToString() == "True")
								{
									if (drRead["Disk"].ToString() == "True")
									{
										task.Content = DeserializeContent(GetContent(drRead["Content"].ToString()));
									}
									else
									{
										task.Content = DeserializeContent(drRead["Content"].ToString());
									}
								}
								else
								{
									if (drRead["Disk"].ToString() == "True")
									{
										task.Content = GetContent(drRead["Content"].ToString());
									}
									else
									{
										task.Content = drRead["Content"].ToString();
									}
								}
								task.TimeStamp = (DateTime)drRead["TimeStamp"];
								task.Signature = drRead["Signature"].ToString();
								task.Id = (Int32)drRead["Id"];
								try
								{
									// if i reach this point it means that the task completed succesfully (so it can be removed later in case of error)
									_taskProcessor.Process(task);
									tasksCompleted++;
									_tasks.RawValue++;
									_tasksPerSecond.IncrementBy(1);
								}
								catch (AggregateException ex)
								{
									if (_lastTaskContentIdWithError != task.Id)
									{
										_lastTaskContentIdWithError = task.Id;
										foreach (Exception e in ex.InnerExceptions)
										{
											Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", task.Id, "Queue Handler " + Queue + " Thread Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
										}
									}
									else
									{
										logError = false;
									}
									throw (ex);
								}
								catch (Exception ex)
								{
									if (_lastTaskContentIdWithError != task.Id)
									{
										Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", task.Id, "Queue Handler " + Queue + " Thread Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
										_lastTaskContentIdWithError = task.Id;
									}
									else
									{
										logError = false;
									}
									throw (ex);
								}
								taskStopWatch.Stop();
								Log.WriteQHTPerformance(_serviceName, _domainName, task.Id, (Int32)taskStopWatch.ElapsedMilliseconds, task.Signature, Queue, _connectionString);
							}
							drRead.Close();
						}
						if (tasksCompleted > 0)
						{
							repeat = RepeatCycle;
							sqlCom = new System.Data.SqlClient.SqlCommand("dbo.WF_Tasks_Remove", sqlConn);
							sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
							sqlCom.Parameters.Add("Queue", System.Data.SqlDbType.VarChar, 50).Value = Queue;
							sqlCom.Parameters.Add("RowCount", System.Data.SqlDbType.Int).Value = tasksCompleted;
							sqlCom.ExecuteNonQuery();
						}
					}
					// reset the counter of the completed tasks
					tasksCompleted = 0;
				}
			}
			catch (AggregateException ex)
			{
				if (logError)
				{
					foreach (Exception e in ex.InnerExceptions)
					{
						Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", "Queue Handler " + Queue + " Thread Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
					}
				}
			}
			catch (Exception ex)
			{
				if (logError)
				{
					Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", "Queue Handler " + Queue + " Thread Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
				}
			}
			finally
			{
				// stops the stop watch anyway
				taskStopWatch.Stop();
				if (tasksCompleted > 0)
				{
					try
					{
						using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(_connectionString))
						{
							sqlConn.Open();
							SqlCommand sqlCom;
							sqlCom = new System.Data.SqlClient.SqlCommand("dbo.WF_Tasks_Remove", sqlConn);
							sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
							sqlCom.Parameters.Add("Queue", System.Data.SqlDbType.VarChar, 50).Value = Queue;
							sqlCom.Parameters.Add("RowCount", System.Data.SqlDbType.Int).Value = tasksCompleted;
							sqlCom.ExecuteNonQuery();
						}
					}
					catch (Exception ex)
					{
						Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", "Queue Handler " + Queue + " Thread Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
					}
				}
				_statusRunning = false;
			}
		}
		/// <summary>
		/// The routine used inside the thread that will handle the queue
		/// </summary>
		private void TaskDedicated()
		{
			System.Diagnostics.Stopwatch taskStopWatch = new System.Diagnostics.Stopwatch();
			Int32 n = 0;
			Int32 tasksCompleted = 0;
			Boolean repeat = true;
			Boolean logError = true;
			_taskProcessor.LastTaskContentIdWithError = _lastTaskContentIdWithError;
			try
			{
				while (repeat)
				{
					repeat = false;
					using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(_connectionString))
					{
						sqlConn.Open();
						System.Data.SqlClient.SqlCommand sqlCom = new System.Data.SqlClient.SqlCommand(_dedicatedSPRead, sqlConn);
						sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
						sqlCom.Parameters.Add("RowCount", System.Data.SqlDbType.Int).Value = NumberOfTasks;
						using (System.Data.SqlClient.SqlDataReader drRead = sqlCom.ExecuteReader())
						{
							if (drRead.Read())
							{
								_monitor.ItemsInQueue = Int32.Parse(drRead["QueueItems"].ToString());
							}
							drRead.NextResult();
							WhizFlowTask task = null;
							while (drRead.Read())
							{
								taskStopWatch.Reset();
								taskStopWatch.Start();
								n++;
								task = new WhizFlowTask();
								if (drRead["Serialized"].ToString() == "True")
								{
									if (drRead["Disk"].ToString() == "True")
									{
										task.Content = DeserializeContent(GetContent(drRead["Content"].ToString()));
									}
									else
									{
										task.Content = DeserializeContent(drRead["Content"].ToString());
									}
								}
								else
								{
									if (drRead["Disk"].ToString() == "True")
									{
										task.Content = GetContent(drRead["Content"].ToString());
									}
									else
									{
										task.Content = drRead["Content"].ToString();
									}
								}
								task.TimeStamp = (DateTime)drRead["TimeStamp"];
								task.Signature = drRead["Signature"].ToString();
								task.Id = (Int32)drRead["Id"];
								try
								{
									// if i reach this point it means that the task completed succesfully (so it can be removed later in case of error)
									_taskProcessor.Process(task);
									tasksCompleted++;
									_tasks.RawValue++;
									_tasksPerSecond.IncrementBy(1);
								}
								catch (AggregateException ex)
								{
									if (_lastTaskContentIdWithError != task.Id)
									{
										_lastTaskContentIdWithError = task.Id;
										foreach (Exception e in ex.InnerExceptions)
										{
											Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", task.Id, "Queue Handler " + Queue + " Thread Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
										}
									}
									else
									{
										logError = false;
									}
									throw (ex);
								}
								catch (Exception ex)
								{
									if (_lastTaskContentIdWithError != task.Id)
									{
										Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", task.Id, "Queue Handler " + Queue + " Thread Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
										_lastTaskContentIdWithError = task.Id;
									}
									else
									{
										logError = false;
									}
									throw (ex);
								}
								taskStopWatch.Stop();
								Log.WriteQHTPerformance(_serviceName, _domainName, task.Id, (Int32)taskStopWatch.ElapsedMilliseconds, task.Signature, Queue, _connectionString);
							}
							drRead.Close();
						}
						if (tasksCompleted > 0)
						{
							repeat = RepeatCycle;
							sqlCom = new System.Data.SqlClient.SqlCommand(_dedicatedSPDelete, sqlConn);
							sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
							sqlCom.Parameters.Add("RowCount", System.Data.SqlDbType.Int).Value = tasksCompleted;
							sqlCom.ExecuteNonQuery();
						}
					}
					// reset the counter of the completed tasks
					tasksCompleted = 0;
				}
			}
			catch (AggregateException ex)
			{
				if (logError)
				{
					foreach (Exception e in ex.InnerExceptions)
					{
						Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", "Queue Handler " + Queue + " Thread Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
					}
				}
			}
			catch (Exception ex)
			{
				if (logError)
				{
					Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", "Queue Handler " + Queue + " Thread Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
				}
			}
			finally
			{
				// stops the stop watch anyway
				taskStopWatch.Stop();
				if (tasksCompleted > 0)
				{
					try
					{
						using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(_connectionString))
						{
							sqlConn.Open();
							SqlCommand sqlCom;
							sqlCom = new System.Data.SqlClient.SqlCommand(_dedicatedSPDelete, sqlConn);
							sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
							sqlCom.Parameters.Add("RowCount", System.Data.SqlDbType.Int).Value = tasksCompleted;
							sqlCom.ExecuteNonQuery();
						}
					}
					catch (Exception ex)
					{
						Log.WriteLogAsync(Log.Module.QueueHandlerThread, Log.LogTypes.Error, "Queue Handler Thread", "Queue Handler " + Queue + " Thread Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
					}
				}
				_statusRunning = false;
			}
		}
		#region IDisposable Members
		/// <summary>
		/// Dispose method to correctly destroy this object.
		/// </summary>
		public void Dispose()
		{
			_monitor.Dispose();
			_timer.Elapsed -= new ElapsedEventHandler(TimerEvent);
			_tasks.RemoveInstance();
			_tasksPerSecond.RemoveInstance();
			_taskProcessor.Dispose();
			GC.SuppressFinalize(this);
		}
		#endregion
		#region Monitoring Members
		/// <summary>
		/// Instrumentation start event handler
		/// </summary>
		private void InstrumentationStartHandling()
		{
			_instrumentationStopped = false;
			Start();
		}
		/// <summary>
		/// Instrumentation stop event handler
		/// </summary>
		private void InstrumentationStopHandling()
		{
			_instrumentationStopped = true;
			Stop();
		}
		#endregion
	}
}