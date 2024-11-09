using Whiz.Framework.Configuration;
using System;

namespace Whiz.WhizFlow.ScheduleManagement
{
	/// <summary>
	/// The class to inherits from when creating a Scheduler Controller in a plugin
	/// </summary>
	public abstract class SchedulerControllerBase : IDisposable
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
		/// The schedule identifier
		/// </summary>
		private String _scheduleName;
		/// <summary>
		/// The actual schedule identifier (readonly)
		/// </summary>
		protected String ScheduleName
		{
			get { return _scheduleName; }
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="scheduleName">The schedule name</param>
		public SchedulerControllerBase(String scheduleName)
		{
			_scheduleName = scheduleName;
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configuration">The specific configuration</param>
		/// <param name="mainConfiguration">The WhizFlow configuration</param>
		/// <param name="moduleName">The module name</param>
		/// <param name="scheduleName">The schedule name</param>
		public SchedulerControllerBase(GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName, String scheduleName)
		{
			Configuration = configuration;
			WhizFlowConfiguration = mainConfiguration;
			_connectionString = mainConfiguration.Get("db").Value;
			ModuleName = moduleName;
			_scheduleName = scheduleName;
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
		~SchedulerControllerBase()
		{
			Dispose(false);
		}
		#endregion
		/// <summary>
		/// The defined process method
		/// </summary>
		public abstract void Process();
		/// <summary>
		/// Insert a log
		/// </summary>
		/// <param name="logType">Type of log</param>
		/// <param name="obj">A object description in order to retrieve the log</param>
		/// <param name="message">Message to log</param>
		/// <param name="additionalInformation">Additional information to log</param>
		protected virtual void InsertLog(Log.LogTypes logType, String obj, String message, String additionalInformation)
		{
			Log.WriteLogAsync(Log.Module.SchedulerControllerPlugin, logType, obj, message, additionalInformation, DateTime.Now, _connectionString);
		}
	}
}
