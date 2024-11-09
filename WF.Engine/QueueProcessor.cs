using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whiz.WhizFlow.Common;
using Whiz.WhizFlow.TasksManagement;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Reflection;
using Whiz.WhizFlow.Common.Objects;
using Whiz.Framework.Configuration;

namespace Whiz.WhizFlow.Engine.Modules
{
	/// <summary>
	/// Queue Processor Module Class
	/// This class will load the Processor Plugins and will use them to process incoming tasks from the queue
	/// </summary>
	public class QueueProcessor : IDisposable
	{
		/// <summary>
		/// Managers used by this queue processor
		/// </summary>
		private List<ManagerBase> _managers;
		/// <summary>
		/// WhizFlow configuration for this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// Local configuration
		/// </summary>
		private GenericConfiguration _queueProcessorConfiguration;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// Queue
		/// </summary>
		private String _taskQueue;
		/// <summary>
		/// Contains the last task content id which processing results in an error
		/// </summary>
		public Int32 LastTaskContentIdWithError { get; set; }
		/// <summary>
		/// Constructor of the Queue Processor
		/// </summary>
		/// <param name="configuration">This WhizFlow domain configuration</param>
		/// <param name="queueProcessorConfiguration">Plugins information for the Queue Processor</param>
		/// <param name="taskQueue">The Queue Handler Thread Queue under which this Queue Processor will work</param>
		public QueueProcessor(GenericConfiguration configuration, GenericConfiguration queueProcessorConfiguration, String taskQueue)
		{
			_taskQueue = taskQueue;
			_configuration = configuration;
			_connectionString = _configuration.Get("db").Value;
			_queueProcessorConfiguration = queueProcessorConfiguration;
			var list = queueProcessorConfiguration.GetList("plugin");
			ManagerBase[] temp = new ManagerBase[list.Count];
			Parallel.For(0, list.Count, (n) =>
				{
					Int32 index = n;
					Object[] parameters = new Object[4];
					parameters[0] = list[index]["configuration"][0];
					parameters[1] = configuration;
					parameters[2] = list[index]["modulename"][0].Value;
					parameters[3] = taskQueue;
					Assembly ManagerPlugin = Assembly.LoadFrom(list[index]["assembly"][0].Value);
					temp[index] = (ManagerBase)ManagerPlugin.CreateInstance(list[index]["class"][0].Value, false, BindingFlags.CreateInstance, null, parameters, null, null);
				}
			);
			_managers = new List<ManagerBase>();
			foreach (ManagerBase pMB in temp)
			{
				_managers.Add(pMB);
			}
		}
		/// <summary>
		/// Process a task submitting it to all the loaded Plugins
		/// </summary>
		/// <param name="task">The task to process</param>
		public void Process(WhizFlowTask task)
		{
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			Monitoring.Events.WhizFlowTaskProcessed monitoringEvent = new Monitoring.Events.WhizFlowTaskProcessed();
			monitoringEvent.TaskContentId = task.Id;
			monitoringEvent.TaskSignature = task.Signature;
			monitoringEvent.Queue = _taskQueue;
			try
			{
				Parallel.ForEach(_managers, manager =>
					{
						manager.LastTaskContentIdWithError = LastTaskContentIdWithError;
						manager.ManageTask(task);
					}
				);
			}
			catch (Exception ex)
			{
				if (LastTaskContentIdWithError != task.Id)
				{
					foreach (Exception e in ((AggregateException)ex).InnerExceptions)
					{
						Log.WriteLogAsync(Log.Module.QueueProcessor, Log.LogTypes.Error, "Queue Processor", task.Id, "Queue Processor Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
					}
				}
				sw.Stop();
				monitoringEvent.WithErrors = true;
				monitoringEvent.Milliseconds = sw.ElapsedMilliseconds;
				monitoringEvent.Fire();
				throw (ex);
			}
			sw.Stop();
			monitoringEvent.WithErrors = false;
			monitoringEvent.Milliseconds = sw.ElapsedMilliseconds;
			monitoringEvent.Fire();
		}
		#region IDisposable Members

		/// <summary>
		/// Will cycle between all the Managers and call their Dispose
		/// </summary>
		public void Dispose()
		{
			Parallel.ForEach(_managers, pManager => { pManager.Dispose(); });
		}

		#endregion
	}
}
