using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Collections;
using System.Data.SqlClient;

namespace Whiz.WhizFlow.Engine.Monitoring.Utilities.Logs
{
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
		ModuleSpecificLog,
		/// <summary>
		/// All log types
		/// </summary>
		All
	}
	/// <summary>
	/// The various entities that could log in WhizFlow
	/// </summary>
	public enum Modules
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
		WorkerControllerPlugin,
		/// <summary>
		/// All the entities
		/// </summary>
		All
	}
	/// <summary>
	/// Ordering of the logs
	/// </summary>
	public enum Ordering
	{
		/// <summary>
		/// Ascending order
		/// </summary>
		Ascending,
		/// <summary>
		/// Descending order
		/// </summary>
		Descending
	}
	/// <summary>
	/// The WhizFlow Log Entry class
	/// </summary>
	[Serializable]
	public class LogEntry
	{
		/// <summary>
		/// Log Id
		/// </summary>
		public Int32 Id { get; set; }
		/// <summary>
		/// WhizFlow Service name
		/// </summary>
		public String Service { get; set; }
		/// <summary>
		/// WhizFlow Domain
		/// </summary>
		public String Domain { get; set; }
		/// <summary>
		/// HostName on which the log entry was generated
		/// </summary>
		public String HostName { get; set; }
		/// <summary>
		/// The WhizFlow module
		/// </summary>
		public Modules Module { get; set; }
		/// <summary>
		/// Type of the log
		/// </summary>
		public LogTypes LogType { get; set; }
		/// <summary>
		/// Descriptive field
		/// </summary>
		public String Object { get; set; }
		/// <summary>
		/// If not null, the task content id in processing when the log entry was generated
		/// </summary>
		public Int32? TaskContentId { get; set; }
		/// <summary>
		/// Descriptive field
		/// </summary>
		public String Message { get; set; }
		/// <summary>
		/// Descriptive field
		/// </summary>
		public String AdditionalInformation { get; set; }
		/// <summary>
		/// Timestamp of the databse when the log entry was persisted
		/// </summary>
		public DateTime Time { get; set; }
		/// <summary>
		/// Rebuild a LogEntry object starting from a non specific structure
		/// </summary>
		/// <param name="serializedObject"></param>
		/// <returns></returns>
		public static LogEntry FromSerializedObject(List<Object> serializedObject)
		{
			LogEntry result = new LogEntry();
			result.Id = (Int32)serializedObject[0];
			result.HostName = (String)serializedObject[1];
			result.Service = (String)serializedObject[2];
			result.Time = (DateTime)serializedObject[3];
			result.AdditionalInformation = (String)serializedObject[4];
			result.TaskContentId = (Int32?)serializedObject[5];
			result.Message = (String)serializedObject[6];
			result.Object = (String)serializedObject[7];
			result.Domain = (String)serializedObject[8];
			result.LogType = (LogTypes)(Int32)serializedObject[9];
			result.Module = (Modules)(Int32)serializedObject[10];
			return result;
		}
	}
}
