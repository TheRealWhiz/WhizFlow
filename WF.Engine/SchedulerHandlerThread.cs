using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Threading;
using System.Data.SqlClient;
using Whiz.WhizFlow.Common;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Whiz.WhizFlow.Common.Objects;
using System.Collections.Concurrent;
using Whiz.Framework.Configuration;

namespace Whiz.WhizFlow.Engine.Modules
{
	/// <summary>
	/// Scheduler Handler Thread
	/// </summary>
	public class SchedulerHandlerThread : IDisposable
	{
		/// <summary>
		/// The timer for the scheduler
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
		/// Flag that indicates that the Scheduler Handler Thread was stopped by WMI
		/// </summary>
		private Boolean _instrumentationStopped = true;
		/// <summary>
		/// The Scheduler Processor that will process all the files
		/// </summary>
		private SchedulerProcessor _taskProcessor;
		/// <summary>
		/// Monitor object
		/// </summary>
		private Monitoring.SchedulerHandlerThread _monitor;
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		private String _serviceName;
		/// <summary>
		/// The scheduler name assigned to the thread
		/// </summary>
		public String SchedulerName { get; set; }
		/// <summary>
		/// The WhizFlow main configuration for this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// The start date when the scheduled event should occur
		/// </summary>
		private DateTime _startDate;
		/// <summary>
		/// The timespan from the start date on which the event should re-occur or the timer frequency
		/// </summary>
		private TimeSpan _timeSpan;
		/// <summary>
		/// The timespan originally set from the domain config
		/// </summary>
		private TimeSpan _originalTimeSpan;
		/// <summary>
		/// Used when the scheduled event occurred in the past and there is no need to start the scheduler
		/// </summary>
		private Boolean _disabled = false;
		/// <summary>
		/// Calculated in order to know when the next event will occur
		/// </summary>
		private DateTime _nextEvent;
		/// <summary>
		/// Contructor. If you use this constructor you MUST set all the class property manually before invoke the Start method.
		/// </summary>
		public SchedulerHandlerThread()
		{
			_timer = new System.Timers.Timer();
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="serviceName">WhizFlow service instance name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		/// <param name="schedulerName">The name of the scheduler</param>
		/// <param name="configuration">WhizFlow Configuration file path</param>
		/// <param name="startDate">Start date</param>
		/// <param name="span">Time span</param>
		/// <param name="schedulerConfiguration">The configuration for the SchedulerProcessor</param>
		public SchedulerHandlerThread(String serviceName, String domainName, String schedulerName, GenericConfiguration configuration, DateTime startDate, TimeSpan span, GenericConfiguration schedulerConfiguration)
		{
			_serviceName = serviceName;
			_timer = new System.Timers.Timer();
			SchedulerName = schedulerName;
			_configuration = configuration;
			_connectionString = configuration.Get("db").Value;
			_startDate = startDate;
			_timeSpan = span;
			// save the original timespan
			_originalTimeSpan = TimeSpan.FromMilliseconds(span.TotalMilliseconds);
			_taskProcessor = new SchedulerProcessor(configuration, schedulerConfiguration, schedulerName);
			InitializeMonitoring(serviceName, domainName, schedulerName, _startDate.ToString(), _timeSpan.ToString(), "Schedule");
			_nextEvent = DateTime.MinValue;
			_timer.Elapsed += new ElapsedEventHandler(TimerEvent);
		}
		/// <summary>
		/// Initialize monitoring
		/// </summary>
		/// <param name="serviceName">WhizFlow service instance name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		/// <param name="interval">The scheduler interval</param>
		/// <param name="schedulerName">The scheduler name</param>
		/// <param name="startDate">The scheduler start date</param>
		/// <param name="mode">Working mode</param>
		private void InitializeMonitoring(String serviceName, String domainName, String schedulerName, String startDate, String interval, String mode)
		{
			_monitor = new Monitoring.SchedulerHandlerThread(schedulerName, serviceName, domainName, startDate, interval, mode);
			_instrumentationStopped = false;
		}
		/// <summary>
		/// Starts the SchedulerHandlerThread
		/// </summary>
		public void Start()
		{
			if (_nextEvent == DateTime.MinValue)
			{
				if (_startDate.Ticks == 0 && _timeSpan.Ticks > 0)
				{
					_nextEvent = DateTime.Now.Add(_timeSpan);
					_timer.Interval = _nextEvent.Subtract(DateTime.Now).TotalMilliseconds;
				}
				else if (_startDate.Ticks > 0 && _timeSpan.Ticks == 0)
				{
					if (_startDate > DateTime.Now)
					{
						_nextEvent = _startDate;
						_timer.Interval = _nextEvent.Subtract(DateTime.Now).TotalMilliseconds;
					}
					// the schedule event already occurred
					else
					{
						_disabled = true;
					}
				}
				else if (_startDate.Ticks > 0 && _timeSpan.Ticks > 0)
				{
					if (_startDate > DateTime.Now)
					{
						TimeSpan pTempDate = new TimeSpan(_startDate.Ticks);
						_timer.Interval = _startDate.Subtract(DateTime.Now).TotalMilliseconds;
						_nextEvent = _startDate;
					}
					else
					{
						DateTime pTempDate = _startDate;
						// faster calculation for next occurrence improvement
						Double diff = DateTime.Now.Subtract(_startDate).TotalMilliseconds;
						pTempDate = pTempDate.AddMilliseconds(((Int64)(diff / _timeSpan.TotalMilliseconds)) * _timeSpan.TotalMilliseconds);
						
						while (pTempDate < DateTime.Now)
						{
							pTempDate = pTempDate.Add(_timeSpan);
						}
						_nextEvent = pTempDate;
						_timer.Interval = _nextEvent.Subtract(DateTime.Now).TotalMilliseconds;
					}
				}
			}
			if (!_disabled)
			{
				_timer.Start();
				_monitor.Status = "Running (next " + _nextEvent.ToString() + ")";
			}
			else
			{
				_monitor.Status = "Disabled";
			}
		}
		/// <summary>
		/// Stops future tasks handling. Current processing will be completed as normal.
		/// </summary>
		public void Stop()
		{
			_timer.Stop();
			_monitor.Status = "Stopped";
			_nextEvent = DateTime.MinValue;
		}
		/// <summary>
		/// Change the timespan of the current scheduler
		/// </summary>
		/// <param name="milliseconds"></param>
		public void ChangeRecurrence(Double milliseconds)
		{
			_timeSpan = TimeSpan.FromMilliseconds(milliseconds);
			_monitor.ChangeMode(_startDate.ToString(), _timeSpan.ToString(), "Schedule");
		}
		/// <summary>
		/// Restore the original recurrence configuration
		/// </summary>
		public void RestoreRecurrence()
		{
			_timeSpan = TimeSpan.FromMilliseconds(_originalTimeSpan.TotalMilliseconds);
			_monitor.ChangeMode(_startDate.ToString(), _timeSpan.ToString(), "Schedule");
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
					_thread = new Thread(Task);
					_thread.Start();
				}
			}
			catch (Exception ex)
			{
				Log.WriteLogAsync(Log.Module.SchedulerHandlerThread, Log.LogTypes.Error, "Scheduler Handler Thread", "Scheduler Handler Thread Timer Event " + SchedulerName + " Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
				Monitoring.Events.WhizFlowError monitoringEvent = new Monitoring.Events.WhizFlowError();
				monitoringEvent.WhizFlow = _serviceName;
				monitoringEvent.Error = "Scheduler Handler Thread error";
				monitoringEvent.InternalException = ex.Message + Environment.NewLine + ex.StackTrace;
				monitoringEvent.Module = "SchedulerHandlerThread";
				monitoringEvent.Fire();
			}
		}
		/// <summary>
		/// The routine used inside the thread that will handle the schedule
		/// </summary>
		private void Task()
		{
			_statusRunning = true;
			try
			{
				_monitor.Status = "Processing";
				_taskProcessor.Process();
			}
			catch (AggregateException ex)
			{
				foreach (Exception e in ex.InnerExceptions)
				{
					Log.WriteLogAsync(Log.Module.SchedulerHandlerThread, Log.LogTypes.Error, "Scheduler Handler Thread", "Scheduler Handler Thread " + SchedulerName + " Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
				}
			}
			catch (Exception ex)
			{
				Log.WriteLogAsync(Log.Module.SchedulerHandlerThread, Log.LogTypes.Error, "Scheduler Handler Thread", "Scheduler Handler Thread " + SchedulerName + " Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
			}
			if (_timeSpan.TotalMilliseconds > 0) _timer.Interval = _timeSpan.TotalMilliseconds;
			// no recurring for this schedule
			if (_timeSpan.Ticks == 0)
			{
				_disabled = true;
				_monitor.Status = "Disabled";
			}
			else
			{
				_nextEvent = _nextEvent.Add(_timeSpan);
				Double interval = _nextEvent.Subtract(DateTime.Now).TotalMilliseconds;
				Boolean done = false;
				while (!done)
				{
					try
					{
						while (interval <= 0)
						{
							_nextEvent = _nextEvent.Add(_timeSpan);
							interval = _nextEvent.Subtract(DateTime.Now).TotalMilliseconds;
						}
						_monitor.Status = "Running (next " + _nextEvent.ToString() + ")";
						try
						{
							_timer.Interval = interval;
						}
						catch (Exception ex)
						{
							Log.WriteLogAsync(Log.Module.SchedulerHandlerThread, Log.LogTypes.Error, "Scheduler Handler Thread", "Scheduler Handler Thread " + SchedulerName + " Error while setting interval (" + interval.ToString() + ") : " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
							// default retry in 1 minute
							_timer.Interval = 60000;
						}
						_timer.Enabled = !_instrumentationStopped;
						done = true;
					}
					catch (Exception ex)
					{
						Log.WriteLogAsync(Log.Module.SchedulerHandlerThread, Log.LogTypes.Error, "Scheduler Handler Thread", "Scheduler Handler Thread " + SchedulerName + " Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace + Environment.NewLine + "Interval : " + interval.ToString(), _connectionString);
					}
				}
			}
			_statusRunning = false;
		}
		#region IDisposable Members
		/// <summary>
		/// Dispose method to correctly destroy this object.
		/// </summary>
		public void Dispose()
		{
			_monitor.Dispose();
			_timer.Elapsed -= new ElapsedEventHandler(TimerEvent);
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