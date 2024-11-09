using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Threading;
using System.Data.SqlClient;
using Whiz.WhizFlow.Common;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Whiz.WhizFlow.Common.Objects;
using System.Security.Principal;
using Whiz.Framework.Configuration;

namespace Whiz.WhizFlow.Engine.Modules
{
	/// <summary>
	/// Worker Handler Module. This will handle all the Worker Handler Threads runnning in the domain
	/// </summary>
	public class WorkersHandler : IDisposable
	{
		/// <summary>
		/// Configuration of all the workers in this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// List of worker handler
		/// </summary>
		private List<WorkerHandlerThread> _threads;
		/// <summary>
		/// The WhizFlow service instance name
		/// </summary>
		private String _serviceName;
		/// <summary>
		/// The WhizFlow internal appdomain name
		/// </summary>
		private String _whizFlowDomain;
		/// <summary>
		/// The module monitor object
		/// </summary>
		private Monitoring.WorkersHandler _monitor;
		/// <summary>
		/// Monitor fake used if there aren't workers
		/// </summary>
		private Monitoring.WorkerHandlerThread _fakeMonitor;
		/// <summary>
		/// The windows identity of the service
		/// </summary>
		private WindowsIdentity _identity;
		/// <summary>
		/// SchedulersHandler Constructor
		/// </summary>
		/// <param name="serviceName">The WhizFlow service instance name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		/// <param name="configuration">The WhizFlow configuration</param>
		public WorkersHandler(String serviceName, String domainName, GenericConfiguration configuration)
		{
			_serviceName = serviceName;
			_whizFlowDomain = domainName;
			_configuration = configuration;
			_connectionString = _configuration.Get("db").Value;
			_identity = WindowsIdentity.GetCurrent();
			CreateThreads();
			InitializeMonitoring();
		}
		/// <summary>
		/// Starts all the WorkerHandlerThreads
		/// </summary>
		public void InstrumentationStartHandler()
		{
			Parallel.ForEach(_threads, thread =>
			{
				thread.Start();
			}
			);
		}
		/// <summary>
		/// Stops all the WorkerHandlerThreads
		/// </summary>
		public void InstrumentationStopHandler()
		{
			Parallel.ForEach(_threads, thread =>
			{
				thread.Stop();
			}
			);
		}
		/// <summary>
		/// Initialize the WorkersHandler monitor object
		/// </summary>
		private void InitializeMonitoring()
		{
			_monitor = new Monitoring.WorkersHandler(_serviceName, _whizFlowDomain, _threads.Count);
		}
		/// <summary>
		/// Loads configuration and create the WorkerHandlerThreads
		/// </summary>
		public void CreateThreads()
		{
			using (_identity.Impersonate())
			{
				try
				{
					// remove all the threads
					if (_threads != null)
					{
						for (Int32 n = _threads.Count; n > 0; n--)
						{
							WorkerHandlerThread wht = _threads[n - 1];
							_threads.RemoveAt(n - 1);
							wht.Dispose();
						}
					}
					// creating the thread list
					_threads = new List<WorkerHandlerThread>();
					foreach (GenericConfiguration s in _configuration.GetList("workers/worker"))
					{
						String workerName = s.Get("name").Value;
						WorkerHandlerThread wht;
						wht = new WorkerHandlerThread(_serviceName, _whizFlowDomain, workerName, _configuration, s.Get("plugins"));
						_threads.Add(wht);
					}
					Parallel.ForEach(_threads, thread =>
						{
							thread.Start();
						}
					);
					if (_threads.Count == 0)
					{
						_fakeMonitor = new Monitoring.WorkerHandlerThread("No workers configured on this domain", _serviceName, _whizFlowDomain);
					}
					else
					{
						_fakeMonitor = null;
					}
				}
				catch (AggregateException ex)
				{
					foreach (Exception e in ex.InnerExceptions)
					{
						Log.WriteLogAsync(Log.Module.WorkersHandler, Log.LogTypes.Error, "Workers Handler", "Workers Handler CreateThreads Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
					}
					throw (ex);
				}
				catch (Exception ex)
				{
					Log.WriteLogAsync(Log.Module.SchedulersHandler, Log.LogTypes.Error, "Workers Handler", "Workers Handler CreateThreads Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
					throw (ex);
				}
			}
		}
		/// <summary>
		/// Gets the scheduler handler thread with the given name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public WorkerHandlerThread GetWorkerHandlerThread(String name)
		{
			return _threads.Where(t => t.WorkerName == name).First();
		}
		#region IDisposable Members
		/// <summary>
		/// Dispose method. This will dispose all the WorkerHandlerThreads linked to the WorkersHandler
		/// </summary>
		public void Dispose()
		{
			// we need to remove all the event handlers
			if (_threads != null)
			{
				for (Int32 n = _threads.Count; n > 0; n--)
				{
					WorkerHandlerThread pT = _threads[n - 1];
					_threads.RemoveAt(n - 1);
					pT.Dispose();
				}
			}
			// remove monitor objects
			_monitor.Dispose();
			if (_fakeMonitor != null) _fakeMonitor.Dispose();
		}
		#endregion
	}
}
