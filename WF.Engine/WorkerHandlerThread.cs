using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whiz.Framework.Configuration;
using Whiz.WhizFlow.Engine.Monitoring;

namespace Whiz.WhizFlow.Engine.Modules
{
	/// <summary>
	/// Worker Handler Thread
	/// </summary>
	public class WorkerHandlerThread
	{
		/// <summary>
		/// The WorkerProcessor that will process all the files
		/// </summary>
		private WorkerProcessor _workerProcessor;
		/// <summary>
		/// Monitor object
		/// </summary>
		private Monitoring.WorkerHandlerThread _monitor;
		/// <summary>
		/// WhizFlow service instance name
		/// </summary>
		private String _serviceName;
		/// <summary>
		/// The worker name assigned to the thread
		/// </summary>
		public String WorkerName { get; set; }
		/// <summary>
		/// The WhizFlow main configuration for this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// Contructor. If you use this constructor you MUST set all the class property manually before invoke the Start method.
		/// </summary>
		public WorkerHandlerThread()
		{
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="serviceName">WhizFlow service instance name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		/// <param name="workerName">The name of the worker</param>
		/// <param name="configuration">WhizFlow Configuration</param>
		/// <param name="workerConfiguration">The configuration for the WorkerProcessor</param>
		public WorkerHandlerThread(String serviceName, String domainName, String workerName, GenericConfiguration configuration, GenericConfiguration workerConfiguration)
		{
			_serviceName = serviceName;
			WorkerName = workerName;
			_configuration = configuration;
			_connectionString = configuration.Get("db").Value;
			_workerProcessor = new WorkerProcessor(configuration, workerConfiguration, workerName);
			InitializeMonitoring(serviceName, domainName, workerName);
		}
		/// <summary>
		/// Initialize monitoring
		/// </summary>
		/// <param name="serviceName">WhizFlow service instance name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		/// <param name="workerName">The worker name</param>
		private void InitializeMonitoring(String serviceName, String domainName, String workerName)
		{
			_monitor = new Monitoring.WorkerHandlerThread(workerName, serviceName, domainName);
		}
		/// <summary>
		/// Starts the WorkerHandlerThread
		/// </summary>
		public void Start()
		{
			try
			{
				_workerProcessor.Run();
			}
			catch (AggregateException ex)
			{
				foreach (Exception e in ex.InnerExceptions)
				{
					Log.WriteLogAsync(Log.Module.WorkerHandlerThread, Log.LogTypes.Error, "Worker Handler Thread", "Worker Handler Thread " + WorkerName + " Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
				}
			}
			catch (Exception ex)
			{
				Log.WriteLogAsync(Log.Module.WorkerHandlerThread, Log.LogTypes.Error, "Worker Handler Thread", "Worker Handler Thread " + WorkerName + " Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
			}
			_monitor.Status = "Running (since " + DateTime.Now.ToString() + ")";
		}
		/// <summary>
		/// Stops the processor. Current processing will be aborted.
		/// </summary>
		public void Stop()
		{
			_workerProcessor.Stop();
			_monitor.Status = "Stopped";
		}
		#region IDisposable Members
		/// <summary>
		/// Dispose method to correctly destroy this object.
		/// </summary>
		public void Dispose()
		{
			Stop();
			_monitor.Dispose();
			_workerProcessor.Dispose();
			GC.SuppressFinalize(this);
		}
		#endregion

	}
}
