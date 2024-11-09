using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Whiz.Framework.Configuration;
using Whiz.WhizFlow.WorkerManagement;

namespace Whiz.WhizFlow.Engine.Modules
{
	/// <summary>
	/// Worker Processor Module Class
	/// This class will load the Worker Processor Plugins and will use them
	/// </summary>
	public class WorkerProcessor : IDisposable
	{
		/// <summary>
		/// Controllers associated for this worker
		/// </summary>
		private List<WorkerControllerBase> _controllers;
		/// <summary>
		/// WhizFlow configuration for this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// Local configuration
		/// </summary>
		private GenericConfiguration _workerProcessorConfiguration;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// Scheduler name
		/// </summary>
		private String _workerName;
		/// <summary>
		/// A real thread for each worker
		/// </summary>
		private List<Thread> _threads;
		/// <summary>
		/// Constructor of the Worker Processor
		/// </summary>
		/// <param name="configuration">WhizFlow configuration for this domain</param>
		/// <param name="workerProcessorConfiguration">The Plugins information for the Scheduler Processor</param>
		/// <param name="workerName">The schedule name</param>
		public WorkerProcessor(GenericConfiguration configuration, GenericConfiguration workerProcessorConfiguration, String workerName)
		{
			_threads = new List<Thread>();
			_workerName = workerName;
			// first we load configuration
			_configuration = configuration;
			_connectionString = _configuration.Get("db").Value;
			_workerProcessorConfiguration = workerProcessorConfiguration;
			var list = workerProcessorConfiguration.GetList("plugin");

			WorkerControllerBase[] temp = new WorkerControllerBase[list.Count];

			Parallel.For(0, list.Count, (n) =>
				{
					Int32 index = n;
					Object[] parameters = new Object[4];
					parameters[0] = list[index]["configuration"][0];
					parameters[1] = configuration;
					parameters[2] = list[index]["modulename"][0].Value;
					parameters[3] = workerName;
					Assembly WorkerPlugin = Assembly.LoadFrom(list[index]["assembly"][0].Value);
					temp[n] = (WorkerControllerBase)WorkerPlugin.CreateInstance(list[index]["class"][0].Value, false, BindingFlags.CreateInstance, null, parameters, null, null);
				}
			);
			_controllers = new List<WorkerControllerBase>();
			foreach (WorkerControllerBase scb in temp)
			{
				_controllers.Add(scb);
			}
		}
		/// <summary>
		/// Process a schedule submitting it to all the loaded Plugins
		/// </summary>
		public void Run()
		{
			try
			{
				foreach(var controller in _controllers)
				{
					Thread t = new Thread(controller.Run);
					_threads.Add(t);
					t.Start();
				}
			}
			catch (Exception ex)
			{
				Log.WriteLogAsync(Log.Module.WorkerProcessor, Log.LogTypes.Error, "Worker Processor", "Worker Processor Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
				throw (ex);
			}
		}
		/// <summary>
		/// Stops all the worker inside the processor
		/// </summary>
		public void Stop()
		{
			Log.WriteLogAsync(Log.Module.WorkerProcessor, Log.LogTypes.Error, "Worker Processor", "Aborting threads", "Stop invoked", _connectionString);
			foreach (var t in _threads)
			{
				try
				{
					t.Abort();
				}
				catch(Exception ex)
				{
					Log.WriteLogAsync(Log.Module.WorkerProcessor, Log.LogTypes.Error, "Worker Processor", "Worker Processor Error while stopping threads: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
				}
			}
		}
		#region IDisposable Members
		/// <summary>
		/// Stops all the plugins and Will cycle between them calling their Dispose
		/// </summary>
		public void Dispose()
		{
			Stop();
			Parallel.ForEach(_controllers, controller => { controller.Dispose(); });
		}
		#endregion
	}
}
