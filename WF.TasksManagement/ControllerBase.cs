using Whiz.Framework.Configuration;
using Whiz.WhizFlow.Common.Objects;
using Whiz.WhizFlow.TasksManagement.Objects;
using System;
using System.Collections.Generic;

namespace Whiz.WhizFlow.TasksManagement
{
	/// <summary>
	/// This class is the base for every controller.
	///	Contains the definition of the essentials methods for the WhizFlow and for the configurations
	/// </summary>
	public abstract class ControllerBase : IDisposable
	{
		/// <summary>
		/// The module name
		/// </summary>
		public String ModuleName { get; set; }
		/// <summary>
		/// The specific plugin configuration
		/// </summary>
		public GenericConfiguration Configuration { get; set; }
		/// <summary>
		/// The general WhizFlow configuration
		/// </summary>
		public GenericConfiguration WhizFlowConfiguration { get; set; }
		/// <summary>
		/// Connection string to the WhizFlow database
		/// </summary>
		protected String _connectionString;
		/// <summary>
		/// The task currently handled
		/// </summary>
		private WhizFlowTask _task = null;
		/// <summary>
		/// The task currently handled
		/// </summary>
		protected WhizFlowTask CurrentTask
		{
			get { return _task; }
		}
		/// <summary>
		/// The actual queue identifier
		/// </summary>
		private String _queue;
		/// <summary>
		/// The actual queue identifier (readonly)
		/// </summary>
		protected String Queue
		{
			get { return _queue; }
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="task">The task to handle</param>
		/// <param name="queue">The queue identifier</param>
		/// <param name="mainConfiguration">This WhizFlow domain configuration</param>
		/// <param name="configuration">This queue local configuration</param>
		/// <param name="moduleName">The module name</param>
		public ControllerBase(WhizFlowTask task, String queue, GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName)
		{
			_task = task;
			_queue = queue;
			Configuration = configuration;
			WhizFlowConfiguration = mainConfiguration;
			_connectionString = mainConfiguration.Get("db").Value;
			ModuleName = moduleName;
		}
		/// <summary>
		/// The defined process method
		/// </summary>
		/// <param name="rule">The rules for the ingestion</param>
		/// <returns>A list of WorkflowRules that will be added to the preconfigured parallel WorkflowRule's List</returns>
		public abstract List<WorkflowRule> Process(WorkflowRule rule);
		/// <summary>
		/// Insert a log
		/// </summary>
		/// <param name="logType">Type of log</param>
		/// <param name="obj">A object description in order to retrieve the log</param>
		/// <param name="message">Message to log</param>
		/// <param name="additionalInformation">Additional information to log</param>
		protected virtual void InsertLog(Log.LogTypes logType, String obj, String message, String additionalInformation)
		{
			Log.WriteLogAsync(Log.Module.ControllerPlugin, logType, obj, CurrentTask.Id, message, additionalInformation, DateTime.Now, _connectionString);
		}
		#region IDisposable Pattern
		#region IDisposable Members
		/// <summary>
		/// IDisposable pattern implementation
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// IDisposable pattern implementation which must be done by derived classes<br/>
		/// If _Disposing equals true, the method has been called directly or indirectly by WhizFlow infrastructure. Managed and unmanaged resources can be disposed.<br/>If _Disposing equals false, the method has been called by the runtime from inside the finalizer, and you should not reference other objects. Only unmanaged resources can be disposed.
		/// </summary>
		/// <param name="_Disposing">If _Disposing equals true, the method has been called directly or indirectly by WhizFlow infrastructure. Managed and unmanaged resources can be disposed.<br/>If _Disposing equals false, the method has been called by the runtime from inside the finalizer, and you should not reference other objects. Only unmanaged resources can be disposed.</param>
		protected abstract void Dispose(Boolean _Disposing);
		#endregion
		/// <summary>
		/// Explicit destructor to implement the IDisposable pattern
		/// </summary>
		~ControllerBase()
		{
			Dispose(false);
		}
		#endregion
	}
}
