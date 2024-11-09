using System;
using System.Collections.Generic;
using System.Linq;
using Whiz.WhizFlow.Common;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Reflection;
using Whiz.WhizFlow.ScheduleManagement;
using Whiz.Framework.Configuration;

namespace Whiz.WhizFlow.Engine.Modules
{
	/// <summary>
	/// Scheduler Processor Module Class
	/// This class will load the Scheduler Processor Plugins and will use them when scheduled
	/// </summary>
	public class SchedulerProcessor : IDisposable
	{
		/// <summary>
		/// Controllers associated for this scheduler
		/// </summary>
		private List<SchedulerControllerBase> _controllers;
		/// <summary>
		/// WhizFlow configuration for this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// Local configuration
		/// </summary>
		private GenericConfiguration _schedulerProcessorConfiguration;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// Scheduler name
		/// </summary>
		private String _schedulerName;
		/// <summary>
		/// Constructor of the Scheduler Processor
		/// </summary>
		/// <param name="configuration">WhizFlow configuration for this domain</param>
		/// <param name="schedulerProcessorConfiguration">The Plugins information for the Scheduler Processor</param>
		/// <param name="schedulerName">The schedule name</param>
		public SchedulerProcessor(GenericConfiguration configuration, GenericConfiguration schedulerProcessorConfiguration, String schedulerName)
		{
			_schedulerName = schedulerName;
			// first we load configuration
			_configuration = configuration;
			_connectionString = _configuration.Get("db").Value;
			_schedulerProcessorConfiguration = schedulerProcessorConfiguration;
			var list = schedulerProcessorConfiguration.GetList("plugin");

			SchedulerControllerBase[] temp = new SchedulerControllerBase[list.Count];

			Parallel.For(0, list.Count, (n) =>
				{
					Int32 index = n;
					Object[] parameters = new Object[4];
					parameters[0] = list[index]["configuration"][0];
					parameters[1] = configuration;
					parameters[2] = list[index]["modulename"][0].Value;
					parameters[3] = schedulerName;
					Assembly SchedulePlugin = Assembly.LoadFrom(list[index]["assembly"][0].Value);
					temp[n] = (SchedulerControllerBase)SchedulePlugin.CreateInstance(list[index]["class"][0].Value, false, BindingFlags.CreateInstance, null, parameters, null, null);
				}
			);
			_controllers = new List<SchedulerControllerBase>();
			foreach (SchedulerControllerBase scb in temp)
			{
				_controllers.Add(scb);
			}
		}
		/// <summary>
		/// Process a schedule submitting it to all the loaded Plugins
		/// </summary>
		public void Process()
		{
			Monitoring.Events.WhizFlowSchedulerProcessed monitoringEvent = new Monitoring.Events.WhizFlowSchedulerProcessed();
			monitoringEvent.SchedulerName = _schedulerName;
			try
			{
				Parallel.ForEach(_controllers, controller => { controller.Process(); });
			}
			catch (Exception ex)
			{
				foreach (Exception e in ((AggregateException)ex).InnerExceptions)
				{
					Log.WriteLogAsync(Log.Module.SchedulerProcessor, Log.LogTypes.Error, "Scheduler Processor", "Scheduler Processor Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
				}
				monitoringEvent.WithErrors = true;
				monitoringEvent.Fire();
				throw (ex);
			}
			monitoringEvent.Fire();
		}
		#region IDisposable Members
		/// <summary>
		/// Will cycle between all the Managers and call their Dispose
		/// </summary>
		public void Dispose()
		{
			Parallel.ForEach(_controllers, controller => { controller.Dispose(); });
		}
		#endregion
	}
}
