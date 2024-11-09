using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Whiz.WhizFlow.Common;
using Whiz.WhizFlow.Common.Objects;
using System.Collections.Concurrent;
using System.Threading;

namespace Whiz.WhizFlow
{
	/// <summary>
	/// Common Log functions for WhizFlow
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// Internal Log item class
		/// </summary>
		internal class LogEntry
		{
			/// <summary>
			/// Domain
			/// </summary>
			public String Domain { get; set; }
			/// <summary>
			/// Module
			/// </summary>
			public Module Module { get; set; }
			/// <summary>
			/// LogType
			/// </summary>
			public LogTypes LogType { get; set; }
			/// <summary>
			/// Object
			/// </summary>
			public String Object { get; set; }
			/// <summary>
			/// Task content id
			/// </summary>
			public Int32? TaskContentId { get; set; }
			/// <summary>
			/// Message
			/// </summary>
			public String Message { get; set; }
			/// <summary>
			/// Additional information
			/// </summary>
			public String AdditionalInformation { get; set; }
			/// <summary>
			/// The timestamp of the log
			/// </summary>
			public DateTime Time { get; set; }
		}
		/// <summary>
		/// Connection string for the log process
		/// </summary>
		private static String _connectionString;
		/// <summary>
		/// Domain for the log process
		/// </summary>
		private static String _domain;
		/// <summary>
		/// Service name
		/// </summary>
		private static String _serviceName;
		/// <summary>
		/// The log queue
		/// </summary>
		private static ConcurrentQueue<LogEntry> _queue = new ConcurrentQueue<LogEntry>();
		/// <summary>
		/// The log thread
		/// </summary>
		private static Thread _logThread = new Thread(LogProcess);
		/// <summary>
		/// Lock object
		/// </summary>
		private static Object _threadLock = new Object();
		/// <summary>
		/// Performance counter indicating logs in memory
		/// </summary>
		private static System.Diagnostics.PerformanceCounter _logs;
		/// <summary>
		/// Timer for the performance counter
		/// </summary>
		private static System.Timers.Timer _timer;
		/// <summary>
		/// Init the Log Process
		/// </summary>
		/// <param name="connectionString">WhizFlow main connectionString</param>
		/// <param name="logPerformanceCounter">Optional WhizFlow performance counter to keep trace of the logs in memory. If not available pass null</param>
		/// <param name="serviceName">WhizFlow service name</param>
		/// <param name="domain">WhizFlow domain</param>
		public static void Init(String connectionString, System.Diagnostics.PerformanceCounter logPerformanceCounter, String serviceName, String domain)
		{
			_logs = logPerformanceCounter;
			if (logPerformanceCounter != null)
			{
				_logs.RawValue = 0;
				_timer = new System.Timers.Timer(500);
				_timer.Elapsed += _timer_Elapsed;
				_timer.Start();
			}
			lock (_threadLock)
			{
				_domain = domain;
				_serviceName = serviceName;
				_connectionString = connectionString;
				_logThread = new Thread(LogProcess);
				_logThread.Start();
			}
		}
		/// <summary>
		/// Timer handler event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			_logs.RawValue = _queue.Count;
		}
		/// <summary>
		/// Dispose the Log Process
		/// </summary>
		public static void Dispose()
		{
			while (_logThread.ThreadState != ThreadState.Stopped)
			{ }
			_timer.Stop();
			_timer.Elapsed -= _timer_Elapsed;
		}
		/// <summary>
		/// Function that handles async logs
		/// </summary>
		private static void LogProcess()
		{
			try
			{
				LogEntry l;
				while (_queue.TryDequeue(out l))
				{
					if (l.TaskContentId == null)
					{
						WriteLog(_serviceName, l.Domain, l.Module, l.LogType, l.Object, l.Message, l.AdditionalInformation, l.Time, _connectionString);
					}
					else
					{
						WriteLog(_serviceName, l.Domain, l.Module, l.LogType, l.Object, (Int32)l.TaskContentId, l.Message, l.AdditionalInformation, l.Time, _connectionString);
					}
				}
			}
			catch (Exception ex)
			{
				//sql azure support means no hard exit when sql is not available
				//throw ex;
			}
		}
		/// <summary>
		/// The various Log types used in WhizFlow
		/// </summary>
		public enum LogTypes
		{
			/// <summary>
			/// A information, a non necessary log often used for tracing debug
			/// </summary>
			Information,
			/// <summary>
			/// A handled error that results in an alternative processing or handling
			/// </summary>
			Warning,
			/// <summary>
			/// A error, used when some interventions should be done
			/// </summary>
			Error,
			/// <summary>
			/// A monitor information
			/// </summary>
			OperationLog,
			/// <summary>
			/// A log for some specific module
			/// </summary>
			ModuleSpecificLog
		}
		/// <summary>
		/// The various entities that could log in WhizFlow
		/// </summary>
		public enum Module
		{
			/// <summary>
			/// The base class for Manager
			/// </summary>
			ManagerBase,
			/// <summary>
			/// An implented Manager (plugin)
			/// </summary>
			ManagerPlugin,
			/// <summary>
			/// The base Controller class
			/// </summary>
			ControllerBase,
			/// <summary>
			/// An implemented Controller (plugin)
			/// </summary>
			ControllerPlugin,
			/// <summary>
			/// The Queues Handler Threads handler
			/// </summary>
			QueuesHandler,
			/// <summary>
			/// The single Queue Handler thread
			/// </summary>
			QueueHandlerThread,
			/// <summary>
			/// The Queue Processor
			/// </summary>
			QueueProcessor,
			/// <summary>
			/// The schedulers handler threads handler
			/// </summary>
			SchedulersHandler,
			/// <summary>
			/// The single scheduler handler thread
			/// </summary>
			SchedulerHandlerThread,
			/// <summary>
			/// The scheduler processor
			/// </summary>
			SchedulerProcessor,
			/// <summary>
			/// The scheduler controller base class
			/// </summary>
			SchedulerControllerBase,
			/// <summary>
			/// An implemented scheduler controller (plugin)
			/// </summary>
			SchedulerControllerPlugin,
			/// <summary>
			/// The App domain manager
			/// </summary>
			AppDomainManager,
			/// <summary>
			/// The WhizFlow manager inside an app domain
			/// </summary>
			WhizFlowManager,
			/// <summary>
			/// The Workers Handler
			/// </summary>
			WorkersHandler,
			/// <summary>
			/// The Worker Handler Thread
			/// </summary>
			WorkerHandlerThread,
			/// <summary>
			/// THe Worker Processor
			/// </summary>
			WorkerProcessor,
			/// <summary>
			/// An implemented worker controller plugin
			/// </summary>
			WorkerControllerPlugin
		}
		/// <summary>
		/// Writes a log line into the log table
		/// </summary>
		/// <param name="service">WhizFlow service name</param>
		/// <param name="domain">The WhizFlow domain</param>
		/// <param name="module">The module where the log is written</param>
		/// <param name="logType">The type of the log</param>
		/// <param name="obj">The object information field</param>
		/// <param name="taskContentId">The task content Id</param>
		/// <param name="message">The log</param>
		/// <param name="additionalInformation">Additional information for the log</param>
		/// <param name="time">Time of the log</param>
		/// <param name="connectionString">The database on which save the log</param>
		public static void WriteLog(String service, String domain, Module module, LogTypes logType, String obj, Int32 taskContentId, String message, String additionalInformation, DateTime time, String connectionString)
		{
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Log_Write";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = Environment.MachineName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				command.Parameters.Add("Object", System.Data.SqlDbType.VarChar, -1).Value = obj;
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				command.Parameters.Add("Message", System.Data.SqlDbType.VarChar, -1).Value = message;
				command.Parameters.Add("AdditionalInformation", System.Data.SqlDbType.VarChar, -1).Value = additionalInformation;
				command.Parameters.Add("Time", System.Data.SqlDbType.DateTime).Value = time;
				command.Connection = connection;
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		/// <summary>
		/// Writes a log line into the log table
		/// </summary>
		/// <param name="service">WhizFlow service name</param>
		/// <param name="domain">The WhizFlow domain</param>
		/// <param name="module">The module where the log is written</param>
		/// <param name="logType">The type of the log</param>
		/// <param name="obj">The object information field</param>
		/// <param name="message">The log</param>
		/// <param name="additionalInformation">Additional information for the log</param>
		/// <param name="time">Time of the log</param>
		/// <param name="connectionString">The database on which save the log</param>
		public static void WriteLog(String service, String domain, Module module, LogTypes logType, String obj, String message, String additionalInformation, DateTime time, String connectionString)
		{
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Log_Write";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = Environment.MachineName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				command.Parameters.Add("Object", System.Data.SqlDbType.VarChar, -1).Value = obj;
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = null;
				command.Parameters.Add("Message", System.Data.SqlDbType.VarChar, -1).Value = message;
				command.Parameters.Add("AdditionalInformation", System.Data.SqlDbType.VarChar, -1).Value = additionalInformation;
				command.Parameters.Add("Time", System.Data.SqlDbType.DateTime).Value = time;
				command.Connection = connection;
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		/// <summary>
		/// Writes a log line into the log table
		/// </summary>
		/// <param name="service">WhizFlow service name</param>
		/// <param name="domain">The WhizFlow domain</param>
		/// <param name="module">The module where the log is written</param>
		/// <param name="logType">The type of the log</param>
		/// <param name="obj">The object information field</param>
		/// <param name="taskContentId">The task content Id</param>
		/// <param name="message">The log</param>
		/// <param name="additionalInformation">Additional information for the log</param>
		/// <param name="connectionString">The database on which save the log</param>
		public static void WriteLog(String service, String domain, Module module, LogTypes logType, String obj, Int32 taskContentId, String message, String additionalInformation, String connectionString)
		{
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Log_Write";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = Environment.MachineName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				command.Parameters.Add("Object", System.Data.SqlDbType.VarChar, -1).Value = obj;
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				command.Parameters.Add("Message", System.Data.SqlDbType.VarChar, -1).Value = message;
				command.Parameters.Add("AdditionalInformation", System.Data.SqlDbType.VarChar, -1).Value = additionalInformation;
				command.Parameters.Add("Time", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
				command.Connection = connection;
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		/// <summary>
		/// Writes a log line into the log table
		/// </summary>
		/// <param name="service">WhizFlow service name</param>
		/// <param name="domain">The WhizFlow domain</param>
		/// <param name="module">The module where the log is written</param>
		/// <param name="logType">The type of the log</param>
		/// <param name="obj">The object information field</param>
		/// <param name="message">The log</param>
		/// <param name="additionalInformation">Additional information for the log</param>
		/// <param name="connectionString">The database on which save the log</param>
		public static void WriteLog(String service, String domain, Module module, LogTypes logType, String obj, String message, String additionalInformation, String connectionString)
		{
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Log_Write";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = Environment.MachineName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				command.Parameters.Add("Object", System.Data.SqlDbType.VarChar, -1).Value = obj;
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = null;
				command.Parameters.Add("Message", System.Data.SqlDbType.VarChar, -1).Value = message;
				command.Parameters.Add("AdditionalInformation", System.Data.SqlDbType.VarChar, -1).Value = additionalInformation;
				command.Parameters.Add("Time", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
				command.Connection = connection;
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		/// <summary>
		/// Writes a log line into the log table asynchronously
		/// </summary>
		/// <param name="module">The module where the log is written</param>
		/// <param name="logType">The type of the log</param>
		/// <param name="obj">The object information field</param>
		/// <param name="taskContentId">The Task Content Id</param>
		/// <param name="message">The log</param>
		/// <param name="additionalInformation">Additional information for the log</param>
		/// <param name="connectionString">The database on which save the log</param>
		public static void WriteLogAsync(Module module, LogTypes logType, String obj, Int32 taskContentId, String message, String additionalInformation, String connectionString)
		{
			LogEntry log = new LogEntry();
			log.Domain = _domain;
			log.AdditionalInformation = additionalInformation;
			log.TaskContentId = taskContentId;
			log.LogType = logType;
			log.Message = message;
			log.Module = module;
			log.Object = obj;
			log.Time = DateTime.Now;
			_queue.Enqueue(log);
			lock (_threadLock)
			{
				if (_logThread.ThreadState != ThreadState.Running)
				{
					_logThread = new Thread(LogProcess);
					_logThread.Start();
				}
			}
		}
		/// <summary>
		/// Writes a log line into the log table asynchronously
		/// </summary>
		/// <param name="module">The module where the log is written</param>
		/// <param name="logType">The type of the log</param>
		/// <param name="obj">The object information field</param>
		/// <param name="message">The log</param>
		/// <param name="additionalInformation">Additional information for the log</param>
		/// <param name="connectionString">The database on which save the log</param>
		public static void WriteLogAsync(Module module, LogTypes logType, String obj, String message, String additionalInformation, String connectionString)
		{
			LogEntry log = new LogEntry();
			log.Domain = _domain;
			log.AdditionalInformation = additionalInformation;
			log.TaskContentId = null;
			log.LogType = logType;
			log.Message = message;
			log.Module = module;
			log.Object = obj;
			log.Time = DateTime.Now;
			_queue.Enqueue(log);
			lock (_threadLock)
			{
				if (_logThread.ThreadState != ThreadState.Running)
				{
					_logThread = new Thread(LogProcess);
					_logThread.Start();
				}
			}
		}
		/// <summary>
		/// Writes a log line into the log table asynchronously
		/// </summary>
		/// <param name="module">The module where the log is written</param>
		/// <param name="logType">The type of the log</param>
		/// <param name="obj">The object information field</param>
		/// <param name="taskContentId">The Task Content Id</param>
		/// <param name="message">The log</param>
		/// <param name="additionalInformation">Additional information for the log</param>
		/// <param name="time">Time of the log</param>
		/// <param name="connectionString">The database on which save the log</param>
		public static void WriteLogAsync(Module module, LogTypes logType, String obj, Int32 taskContentId, String message, String additionalInformation, DateTime time, String connectionString)
		{
			LogEntry log = new LogEntry();
			log.Domain = _domain;
			log.AdditionalInformation = additionalInformation;
			log.TaskContentId = taskContentId;
			log.LogType = logType;
			log.Message = message;
			log.Module = module;
			log.Object = obj;
			log.Time = time;
			_queue.Enqueue(log);
			lock (_threadLock)
			{
				if (_logThread.ThreadState != ThreadState.Running)
				{
					_logThread = new Thread(LogProcess);
					_logThread.Start();
				}
			}
		}
		/// <summary>
		/// Writes a log line into the log table asynchronously
		/// </summary>
		/// <param name="module">The module where the log is written</param>
		/// <param name="logType">The type of the log</param>
		/// <param name="obj">The object information field</param>
		/// <param name="message">The log</param>
		/// <param name="additionalInformation">Additional information for the log</param>
		/// <param name="time">Time of the log</param>
		/// <param name="connectionString">The database on which save the log</param>
		public static void WriteLogAsync(Module module, LogTypes logType, String obj, String message, String additionalInformation, DateTime time, String connectionString)
		{
			LogEntry log = new LogEntry();
			log.Domain = _domain;
			log.AdditionalInformation = additionalInformation;
			log.TaskContentId = null;
			log.LogType = logType;
			log.Message = message;
			log.Module = module;
			log.Object = obj;
			log.Time = time;
			_queue.Enqueue(log);
			lock (_threadLock)
			{
				if (_logThread.ThreadState != ThreadState.Running)
				{
					_logThread = new Thread(LogProcess);
					_logThread.Start();
				}
			}
		}
		/// <summary>
		/// Writes a queue handler thread performance log into the WhizFlow's performances table
		/// </summary>
		/// <param name="service">The service instance of WhizFlow</param>
		/// <param name="domain">The domain of the service instance</param>
		/// <param name="taskContentId">The task content id</param>
		/// <param name="milliseconds">Milliseconds needed for processing</param>
		/// <param name="signature">The signature associated with the instance of the task content</param>
		/// <param name="taskQueue">The task queue processing it</param>
		/// <param name="connectionString">Connection string to the database</param>
		public static void WriteQHTPerformance(String service, String domain, Int32 taskContentId, Int32 milliseconds, String signature, String taskQueue, String connectionString)
		{
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformance_Write";
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				command.Parameters.Add("Milliseconds", System.Data.SqlDbType.Int).Value = milliseconds;
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = Environment.MachineName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("Signature", System.Data.SqlDbType.VarChar, 50).Value = signature;
				command.Parameters.Add("TaskQueue", System.Data.SqlDbType.VarChar, 50).Value = taskQueue;
				command.Connection = connection;
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
	}
}
