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
	/// Queue Handler Module. This will handle all the Pipe Handler Threads assigned to the queues
	/// </summary>
	public class QueuesHandler : IDisposable
	{
		/// <summary>
		/// Configuration of all the queues in this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// List of task queue handler
		/// </summary>
		private List<QueueHandlerThread> _threads;
		/// <summary>
		/// The WhizFlow service instance name
		/// </summary>
		private String _serviceName;
		/// <summary>
		/// The WhizFlow internal appdomain name
		/// </summary>
		private String _domainName;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// The module monitor object
		/// </summary>
		private Monitoring.QueuesHandler _monitor;
		/// <summary>
		/// Monitor fake used if there aren't queues
		/// </summary>
		private Monitoring.QueueHandlerThread _fakeMonitor;
		/// <summary>
		/// The windows identity of the service
		/// </summary>
		private WindowsIdentity _identity;
		/// <summary>
		/// QueuesHandler Constructor
		/// </summary>
		/// <param name="serviceName">The WhizFlow service instance name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		/// <param name="configuration">The configuration for this instance</param>
		public QueuesHandler(String serviceName, String domainName, GenericConfiguration configuration)
		{
			_serviceName = serviceName;
			_domainName = domainName;
			_configuration = configuration;
			_connectionString = _configuration.Get("db").Value;
			_identity = WindowsIdentity.GetCurrent();
			CreateThreads();
			InitializeMonitoring();
		}
		/// <summary>
		/// Resets the QueuesHandler module
		/// </summary>
		public void InstrumentationResetHandler()
		{
			CreateThreads();
			_monitor.NumberOfQHT = _threads.Count;
		}
		/// <summary>
		/// Starts all the QueueHandlerThreads
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
		/// Stops all the QueueHandlerThreads
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
		/// Initialize the QueuesHandler monitor object
		/// </summary>
		private void InitializeMonitoring()
		{
			_monitor = new Monitoring.QueuesHandler(_serviceName, _domainName, _threads.Count);
		}
		/// <summary>
		/// Loads configuration and create the QueueHandlerThreads
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
							QueueHandlerThread pht = _threads[n - 1];
							_threads.RemoveAt(n - 1);
							pht.Dispose();
						}
					}
					// creating the thread list
					_threads = new List<QueueHandlerThread>();
					foreach (GenericConfiguration q in _configuration.GetList("queues/queue"))
					{
						String queue = q.Get("name").Value;
						Double frequency = Double.Parse(q.Get("frequency").Value);
						Boolean repeat = Boolean.Parse(q.Get("repeatcycle").Value);
						Int32 numberOfTasks = Int32.Parse(q.Get("numberoftasks").Value);
						Boolean dedicated = q.Get("dedicated", "0").Value == "1";
						QueueHandlerThread pht;
						pht = new QueueHandlerThread(_serviceName, _domainName, queue, _configuration, frequency, repeat, numberOfTasks, dedicated, q.Get("plugins"));
						_threads.Add(pht);
					}
					Parallel.ForEach(_threads, thread =>
						{
							thread.Start();
						}
					);
					if (_threads.Count == 0)
					{
						_fakeMonitor = new Monitoring.QueueHandlerThread("No queue configured on this domain", _serviceName, _domainName, 0, "n/a");
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
						Log.WriteLogAsync(Log.Module.QueuesHandler, Log.LogTypes.Error, "Queues Handler", "Queues Handler CreateThreads Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
					}
					throw (ex);
				}
				catch (Exception ex)
				{
					Log.WriteLogAsync(Log.Module.QueuesHandler, Log.LogTypes.Error, "Queues Handler", "Queues Handler CreateThreads Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
					throw (ex);
				}
			}
		}
		#region IDisposable Members
		/// <summary>
		/// Dispose method. This will dispose all the QueueHandlerThreads object linked to the QueuesHandler
		/// </summary>
		public void Dispose()
		{
			// we need to remove all the event handlers
			if (_threads != null)
			{
				for (Int32 n = _threads.Count; n > 0; n--)
				{
					QueueHandlerThread pT = _threads[n - 1];
					_threads.RemoveAt(n - 1);
					pT.Dispose();
				}
			}
			// remove events and monitor objects
			_monitor.Dispose();
			if (_fakeMonitor != null) _fakeMonitor.Dispose();
		}
		#endregion
	}
}
