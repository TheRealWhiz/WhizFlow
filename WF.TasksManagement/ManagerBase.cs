using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Whiz.WhizFlow.Common;
using Whiz.WhizFlow.TasksManagement;
using Whiz.WhizFlow;
using System.Threading.Tasks;
using System.Reflection;
using Whiz.WhizFlow.TasksManagement.Objects;
using Whiz.WhizFlow.Common.Objects;
using Whiz.Framework.Configuration;

namespace Whiz.WhizFlow.TasksManagement
{
	/// <summary>
	/// The class to inherits from when creating a Manager in a plugin
	/// </summary>
	public abstract class ManagerBase : IDisposable
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
		/// The general WhizFlow configuration file
		/// </summary>
		public GenericConfiguration WhizFlowConfiguration { get; set; }
		/// <summary>
		/// Connection string to the WhizFlow database
		/// </summary>
		protected String _connectionString;
		/// <summary>
		/// The task currently handled
		/// </summary>
		private Task _task = null;
		/// <summary>
		/// The task currently handled
		/// </summary>
		protected Task CurrentTask
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
		/// The collection of all the TaskWorkflows
		/// </summary>
		private TaskWorkflows _taskWorkflow;
		/// <summary>
		/// The Change Monitor that will be checked in order to recall the LoadMappings method
		/// </summary>
		protected System.Runtime.Caching.ChangeMonitor _taskWorkflowChangeMonitor = null;
		/// <summary>
		/// Contains the last task id which processing results in an error
		/// </summary>
		public Int32 LastTaskContentIdWithError { get; set; }
		/// <summary>
		/// Contains the last error in order to compare
		/// </summary>
		public Exception LastError { get; set; }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configuration">The specific configuration file</param>
		/// <param name="mainConfiguration">The WhizFlow configuration file</param>
		/// <param name="moduleName">The module name</param>
		/// <param name="queue">The queue identifier</param>
		public ManagerBase(GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName, String queue)
		{
			_queue = queue;
			Configuration = configuration;
			WhizFlowConfiguration = mainConfiguration;
			_connectionString = mainConfiguration.Get("db").Value;
			ModuleName = moduleName;
			_taskWorkflow = LoadWorkflows();
		}
		/// <summary>
		/// A method that must be implemented that will return all the TaskWorkflows for the plugin
		/// </summary>
		/// <returns>The plugin TaskWorkflows</returns>
		public abstract TaskWorkflows LoadWorkflows();
		/// <summary>
		/// Main method that handle an incoming task
		/// </summary>
		/// <param name="task">The task to handle</param>
		public void ManageTask(WhizFlowTask task)
		{
#if DEBUG
			System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
			stopWatch.Start();
#endif
			TaskWorkflow taskWorkflow = null;
#if DEBUG
			Log.WriteLogAsync(Log.Module.ManagerBase, Log.LogTypes.OperationLog, "Manager", task.Id, "Manager " + ModuleName + " on queue " + Queue, "ManageTask method invoked", _connectionString);
#endif
			// if the implementation of LoadMappings sets the changemonitor
			if (_taskWorkflowChangeMonitor != null)
			{
				// check if some dependency has been changed
				if (_taskWorkflowChangeMonitor.HasChanged)
				{
					// and reloads the mappings
					_taskWorkflow = LoadWorkflows();
				}
			}
			try
			{
				taskWorkflow = _taskWorkflow.GetTaskHandler(task.Signature);
				if (taskWorkflow != null)
				{
					Object[] parameters = new Object[5];
					parameters[0] = task;
					parameters[1] = Queue;
					parameters[2] = Configuration;
					parameters[3] = WhizFlowConfiguration;
					parameters[4] = ModuleName;
					ControllerBase taskHandler = (ControllerBase)Activator.CreateInstance(taskWorkflow.ControllerType, parameters);

					for (Int32 n = 0; n < taskWorkflow.OrderSequence.Count; n++)
					{
#if DEBUG
						Log.WriteLogAsync(Log.Module.ManagerBase, Log.LogTypes.OperationLog, "Manager", task.Id, "Manager " + ModuleName + " on queue " + Queue, "ManageTask starts to invoke process methods of the Controller " + taskHandler.GetType().ToString() + " for the order " + taskWorkflow.OrderSequence[n].ToString(), _connectionString);
#endif
						System.Collections.Concurrent.ConcurrentBag<WorkflowRule> dynamicStep = new System.Collections.Concurrent.ConcurrentBag<WorkflowRule>();

						Parallel.ForEach(taskWorkflow.OrdersWorkflowRules[taskWorkflow.OrderSequence[n]], wr =>
						{
							List<WorkflowRule> resultWR = taskHandler.Process(wr);
							if (resultWR != null)
							{
								foreach (WorkflowRule w in resultWR)
								{
									dynamicStep.Add(w);
								}
							}
						});
						while (dynamicStep.Count > 0)
						{
							System.Collections.Concurrent.ConcurrentBag<WorkflowRule> newDynamicStep = new System.Collections.Concurrent.ConcurrentBag<WorkflowRule>();

							Parallel.ForEach(dynamicStep, wr =>
							{
								List<WorkflowRule> resultWR = taskHandler.Process(wr);
								if (resultWR != null)
								{
									foreach (WorkflowRule w in resultWR)
									{
										newDynamicStep.Add(w);
									}
								}
							});
							dynamicStep = newDynamicStep;
						}
					}
				}
#if DEBUG
				stopWatch.Stop();
				Log.WriteLogAsync(Log.Module.ManagerBase, Log.LogTypes.OperationLog, "Manager", task.Id, "Manager " + ModuleName + " on queue " + Queue, "ManageTask completes ", _connectionString);
#endif
			}
			catch (AggregateException ex)
			{
				if (LastTaskContentIdWithError != task.Id)
				{
					foreach (Exception e in ex.InnerExceptions)
					{
						Log.WriteLogAsync(Log.Module.ManagerBase, Log.LogTypes.Error, "Manager", task.Id, "Manager " + ModuleName + " on queue " + Queue + " Error : " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
					}
				}
				throw (ex);
			}
			catch (Exception ex)
			{
				if (LastTaskContentIdWithError != task.Id)
				{
					Log.WriteLogAsync(Log.Module.ManagerBase, Log.LogTypes.Error, "Manager", task.Id, "Manager " + ModuleName + " on queue " + Queue + " Error : " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
				}
				throw (ex);
			}
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
		~ManagerBase()
		{
			Dispose(false);
		}
		#endregion
	}
}
