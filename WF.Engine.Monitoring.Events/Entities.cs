using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Instrumentation;

[assembly: Instrumented(@"root\Whiz\WhizFlow")]
namespace Whiz.WhizFlow.Engine.Monitoring.Events
{
	/// <summary>
	/// Installer
	/// </summary>
	[System.ComponentModel.RunInstaller(true)]
	public class MyInstall : DefaultManagementProjectInstaller { }

	/// <summary>
	/// WhizFlow Error Event for WMI
	/// </summary>
	[InstrumentationClass(System.Management.Instrumentation.InstrumentationType.Event)]
	public class WhizFlowError : BaseEvent
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public WhizFlowError()
		{
		}
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		public String WhizFlow { get; set; }
		/// <summary>
		/// Error description
		/// </summary>
		public String Error { get; set; }
		/// <summary>
		/// Module in which errors occurred
		/// </summary>
		public String Module { get; set; }
		/// <summary>
		/// The complete exception
		/// </summary>
		public String InternalException { get; set; }
	}
	/// <summary>
	/// WhizFlow task processed event for wmi
	/// </summary>
	[InstrumentationClass(System.Management.Instrumentation.InstrumentationType.Event)]
	public class WhizFlowTaskProcessed : BaseEvent
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public WhizFlowTaskProcessed()
		{
		}
		/// <summary>
		/// Elapsed time from the start of the processing and the end
		/// </summary>
		public Int64 Milliseconds { get; set; }
		/// <summary>
		/// The processing completes succesfully or with errors
		/// </summary>
		public Boolean WithErrors { get; set; }
		/// <summary>
		/// The task content Id
		/// </summary>
		public Int32 TaskContentId { get; set; }
		/// <summary>
		/// The task associated signature
		/// </summary>
		public String TaskSignature { get; set; }
		/// <summary>
		/// The Queue under which the task is processed
		/// </summary>
		public String Queue { get; set; }
	}
	/// <summary>
	/// WhizFlow scheduler processed event for wmi
	/// </summary>
	[InstrumentationClass(System.Management.Instrumentation.InstrumentationType.Event)]
	public class WhizFlowSchedulerProcessed : BaseEvent
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public WhizFlowSchedulerProcessed()
		{
		}
		/// <summary>
		/// The processing completes succesfully or with errors
		/// </summary>
		public Boolean WithErrors { get; set; }
		/// <summary>
		/// The Scheduler name
		/// </summary>
		public String SchedulerName { get; set; }
	}
}
