using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Instrumentation;

[assembly: WmiConfiguration(@"root\Whiz\WhizFlow", HostingModel = ManagementHostingModel.Decoupled)]
namespace Whiz.WhizFlow.Engine.Monitoring
{
	/// <summary>
	/// Installer component
	/// </summary>
	[System.ComponentModel.RunInstaller(true)]
	public class MyInstall : DefaultManagementInstaller { }
	/// <summary>
	/// This class provides a monitor/instrumentation class for the QueuesHandler module of WhizFlow
	/// </summary>
	[ManagementEntity]
	public class QueuesHandler : IDisposable
	{
		/// <summary>
		/// PipeHandler monitor object constructor
		/// </summary>
		/// <param name="whizFlowName">The WhizFlow service instance name</param>
		/// <param name="whizFlowDomain">The WhizFlow internal appdomain name</param>
		/// <param name="numberOfQHT">Number of Queue Handler threads</param>
		public QueuesHandler(String whizFlowName, String whizFlowDomain, Int32 numberOfQHT)
		{
			WhizFlow = whizFlowName;
			RunningSince = DateTime.Now.ToString();
			NumberOfQHT = numberOfQHT;
			WhizFlowDomain = whizFlowDomain;
			InstrumentationManager.Publish(this);
		}
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		[ManagementKey]
		public String WhizFlow { get; set; }
		/// <summary>
		/// WhizFlow internal domain
		/// </summary>
		[ManagementKey]
		public String WhizFlowDomain { get; set; }
		/// <summary>
		/// Number of Queue Handler Threads
		/// </summary>
		[ManagementProbe]
		public Int32 NumberOfQHT { get; set; }
		/// <summary>
		/// Start time of this Queue Handler
		/// </summary>
		[ManagementProbe]
		public String RunningSince { get; set; }
		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			InstrumentationManager.Revoke(this);
		}
		#endregion
	}
	/// <summary>
	/// Queue Handler thread monitoring class
	/// </summary>
	[ManagementEntity]
	public class QueueHandlerThread : IDisposable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="queue">Queue name</param>
		/// <param name="whizFlowName">WhizFlow service instance name</param>
		/// <param name="whizFlowDomain">The WhizFlow internal appdomain name</param>
		/// <param name="frequency">Frequency of polling</param>
		/// <param name="mode">Working mode</param>
		public QueueHandlerThread(String queue, String whizFlowName, String whizFlowDomain, Double frequency, String mode)
		{
			Queue = String.Format(queue);
			Mode = String.Format("{0} ({1} ms)", mode, frequency);
			WhizFlow = whizFlowName;
			WhizFlowDomain = whizFlowDomain;
			InstrumentationManager.Publish(this);
		}
		/// <summary>
		/// Queue name
		/// </summary>
		[ManagementKey]
		public String Queue { get; set; }
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		[ManagementKey]
		public String WhizFlow { get; set; }
		/// <summary>
		/// WhizFlow internal domain
		/// </summary>
		[ManagementKey]
		public String WhizFlowDomain { get; set; }
		/// <summary>
		/// Working mode
		/// </summary>
		[ManagementProbe]
		public String Mode { get; set; }
		/// <summary>
		/// Status
		/// </summary>
		[ManagementProbe]
		public String Status { get; set; }
		/// <summary>
		/// Last running of the Queue Handler Thread
		/// </summary>
		[ManagementProbe]
		public String LastRunning { get; set; }
		/// <summary>
		/// Number of items actually in the queue
		/// </summary>
		[ManagementProbe]
		public Int32 ItemsInQueue { get; set; }
		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			InstrumentationManager.Revoke(this);
		}

		#endregion
	}
	#region Queue Status
	/// <summary>
	/// Queue monitoring class
	/// </summary>
	public class QueueStatus : IDisposable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="queue">Queue name</param>
		/// <param name="whizFlowName">WhizFlow service instance name</param>
		/// <param name="whizFlowDomain">The WhizFlow internal appdomain name</param>
		public QueueStatus(String queue, String whizFlowName, String whizFlowDomain)
		{
			Queue = String.Format(queue);
			WhizFlow = whizFlowName;
			WhizFlowDomain = whizFlowDomain;
			InstrumentationManager.Publish(this);
		}
		/// <summary>
		/// Queue name
		/// </summary>
		[ManagementKey]
		public String Queue { get; set; }
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		[ManagementKey]
		public String WhizFlow { get; set; }
		/// <summary>
		/// WhizFlow internal domain
		/// </summary>
		[ManagementKey]
		public String WhizFlowDomain { get; set; }
		/// <summary>
		/// Status of the queue
		/// </summary>
		[ManagementProbe]
		public String Status { get; set; }
		/// <summary>
		/// Last update of the status
		/// </summary>
		[ManagementProbe]
		public String LastRunning { get; set; }
		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			InstrumentationManager.Revoke(this);
		}
		#endregion
	}
	#endregion
	#region Scheduler Handler
	/// <summary>
	/// Scheduler Handler monitoring/instrumentation class
	/// </summary>
	[ManagementEntity]
	public class SchedulersHandler : IDisposable
	{
		/// <summary>
		/// SchedulerHandler monitor object constructor
		/// </summary>
		/// <param name="whizFlowName">The WhizFlow service instance name</param>
		/// <param name="whizFlowDomain">The WhizFlow internal appdomain name</param>
		/// <param name="numberOfSHT">Number of SchedulerHandlerThreads</param>
		public SchedulersHandler(String whizFlowName, String whizFlowDomain, Int32 numberOfSHT)
		{
			WhizFlow = whizFlowName;
			RunningSince = DateTime.Now.ToString();
			WhizFlowDomain = whizFlowDomain;
			NumberOfSHT = numberOfSHT;
			InstrumentationManager.Publish(this);
		}
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		[ManagementKey]
		public String WhizFlow { get; set; }
		/// <summary>
		/// WhizFlow internal domain
		/// </summary>
		[ManagementKey]
		public String WhizFlowDomain { get; set; }
		/// <summary>
		/// Number of Scheduler Handler Threads
		/// </summary>
		[ManagementProbe]
		public Int32 NumberOfSHT { get; set; }
		/// <summary>
		/// Start time of this Scheduler Handler
		/// </summary>
		[ManagementProbe]
		public String RunningSince { get; set; }
		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			InstrumentationManager.Revoke(this);
		}
		#endregion
	}
	#endregion
	#region Scheduler Handler Thread
	/// <summary>
	/// Scheduler Handler Thread monitoring/instrumentation class
	/// </summary>
	[ManagementEntity]
	public class SchedulerHandlerThread : IDisposable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="schedulerName">Scheduler name</param>
		/// <param name="whizFlowName">WhizFlow service instance name</param>
		/// <param name="whizFlowDomain">The WhizFlow internal appdomain name</param>
		/// <param name="startDate">The start date of the Scheduler</param>
		/// <param name="interval">The configured interval of the Scheduler</param>
		/// <param name="mode">Working mode</param>
		public SchedulerHandlerThread(String schedulerName, String whizFlowName, String whizFlowDomain, String startDate, String interval, String mode)
		{
			SchedulerName = String.Format(schedulerName);
			Mode = String.Format("{0} (Start: {1}, Recurrance: {2})", mode, startDate, interval);
			WhizFlow = whizFlowName;
			WhizFlowDomain = whizFlowDomain;
			InstrumentationManager.Publish(this);
		}
		/// <summary>
		/// Changes the mode property for the monitoring
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="interval"></param>
		/// <param name="mode"></param>
		public void ChangeMode(String startDate, String interval, String mode)
		{
			Mode = String.Format("{0} (Start: {1}, Recurrance: {2})", mode, startDate, interval);
		}
		/// <summary>
		/// Scheduler name
		/// </summary>
		[ManagementKey]
		public String SchedulerName { get; set; }
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		[ManagementKey]
		public String WhizFlow { get; set; }
		/// <summary>
		/// WhizFlow internal domain
		/// </summary>
		[ManagementKey]
		public String WhizFlowDomain { get; set; }
		/// <summary>
		/// Working mode
		/// </summary>
		[ManagementProbe]
		public String Mode { get; set; }
		/// <summary>
		/// Status
		/// </summary>
		[ManagementProbe]
		public String Status { get; set; }
		/// <summary>
		/// Last running of the SHT
		/// </summary>
		[ManagementProbe]
		public String LastRunning { get; set; }
		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			InstrumentationManager.Revoke(this);
		}
		#endregion
	}
	#endregion
	#region SchedulerStatus
	/// <summary>
	/// Scheduler Status monitoring class
	/// </summary>
	[ManagementEntity]
	public class SchedulerStatus : IDisposable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="schedulerName">Scheduler name</param>
		/// <param name="whizFlowName">WhizFlow service instance name</param>
		/// <param name="whizFlowDomain">The WhizFlow internal appdomain name</param>
		public SchedulerStatus(String schedulerName, String whizFlowName, String whizFlowDomain)
		{
			SchedulerName = String.Format(schedulerName);
			WhizFlow = whizFlowName;
			WhizFlowDomain = whizFlowDomain;
			InstrumentationManager.Publish(this);
		}
		/// <summary>
		/// Scheduler name
		/// </summary>
		[ManagementKey]
		public String SchedulerName { get; set; }
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		[ManagementKey]
		public String WhizFlow { get; set; }
		/// <summary>
		/// WhizFlow internal domain
		/// </summary>
		[ManagementKey]
		public String WhizFlowDomain { get; set; }
		/// <summary>
		/// Status
		/// </summary>
		[ManagementProbe]
		public String Status { get; set; }
		/// <summary>
		/// Last update
		/// </summary>
		[ManagementProbe]
		public String LastUpdate { get; set; }
		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			InstrumentationManager.Revoke(this);
		}
		#endregion
	}
	#endregion
	#region "Worker Handler"
	/// <summary>
	/// Worker Handler monitoring class
	/// </summary>
	[ManagementEntity]
	public class WorkersHandler : IDisposable
	{
		/// <summary>
		/// SchedulerHandler monitor object constructor
		/// </summary>
		/// <param name="whizFlowName">The WhizFlow service instance name</param>
		/// <param name="whizFlowDomain">The WhizFlow internal appdomain name</param>
		/// <param name="numberOfWHT">Number of Worker Handler threads</param>
		public WorkersHandler(String whizFlowName, String whizFlowDomain, Int32 numberOfWHT)
		{
			WhizFlow = whizFlowName;
			RunningSince = DateTime.Now.ToString();
			WhizFlowDomain = whizFlowDomain;
			NumberOfWHT = numberOfWHT;
			InstrumentationManager.Publish(this);
		}
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		[ManagementKey]
		public String WhizFlow { get; set; }
		/// <summary>
		/// WhizFlow internal domain
		/// </summary>
		[ManagementKey]
		public String WhizFlowDomain { get; set; }
		/// <summary>
		/// Number of Worker Handler Threads
		/// </summary>
		[ManagementProbe]
		public Int32 NumberOfWHT { get; set; }
		/// <summary>
		/// Start time of this Worker Handler
		/// </summary>
		[ManagementProbe]
		public String RunningSince { get; set; }
		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			InstrumentationManager.Revoke(this);
		}
		#endregion
	}
	#endregion
	#region "Worker Handler Thread"
	/// <summary>
	/// Worker Handler Thread monitoring class
	/// </summary>
	[ManagementEntity]
	public class WorkerHandlerThread : IDisposable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="workerName">Scheduler name</param>
		/// <param name="whizFlowName">WhizFlow service instance name</param>
		/// <param name="whizFlowDomain">The WhizFlow internal appdomain name</param>
		public WorkerHandlerThread(String workerName, String whizFlowName, String whizFlowDomain)
		{
			WorkerName = String.Format(workerName);
			WhizFlow = whizFlowName;
			WhizFlowDomain = whizFlowDomain;
			InstrumentationManager.Publish(this);
		}
		/// <summary>
		/// Scheduler name
		/// </summary>
		[ManagementKey]
		public String WorkerName { get; set; }
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		[ManagementKey]
		public String WhizFlow { get; set; }
		/// <summary>
		/// WhizFlow internal domain
		/// </summary>
		[ManagementKey]
		public String WhizFlowDomain { get; set; }
		/// <summary>
		/// Status
		/// </summary>
		[ManagementProbe]
		public String Status { get; set; }

		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			InstrumentationManager.Revoke(this);
		}
		#endregion
	}
	#endregion

}
