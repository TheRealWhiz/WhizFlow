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
	/// Scheduler Handler Module. This will handle all the Scheduler Handler Threads in the domain
	/// </summary>
	public class SchedulersHandler : IDisposable
	{
		/// <summary>
		/// Configuration of all the schedulers in this domain
		/// </summary>
		private GenericConfiguration _configuration;
		/// <summary>
		/// WhizFlow support db connection string for this domain
		/// </summary>
		private String _connectionString;
		/// <summary>
		/// List of task scheduler handler
		/// </summary>
		private List<SchedulerHandlerThread> _threads;
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
		private Monitoring.SchedulersHandler _monitor;
		/// <summary>
		/// Monitor fake used if there aren't schedulers
		/// </summary>
		private Monitoring.SchedulerHandlerThread _fakeMonitor;
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
		public SchedulersHandler(String serviceName, String domainName, GenericConfiguration configuration)
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
		/// Resets the SchedulersHandler module
		/// </summary>
		public void InstrumentationResetHandler()
		{
			CreateThreads();
			_monitor.NumberOfSHT = _threads.Count;
		}
		/// <summary>
		/// Starts all the SchedulersHandlerThreads
		/// </summary>
		public void InstrumentationStartHandler()
		{
			Parallel.ForEach(_threads, pThread =>
				{
					pThread.Start();
				}
			);
		}
		/// <summary>
		/// Stops all the SchedulersHandlerThreads
		/// </summary>
		public void InstrumentationStopHandler()
		{
			Parallel.ForEach(_threads, pThread =>
				{
					pThread.Stop();
				}
			);
		}
		/// <summary>
		/// Initialize the SchedulersHandler monitor object
		/// </summary>
		private void InitializeMonitoring()
		{
			_monitor = new Monitoring.SchedulersHandler(_serviceName, _whizFlowDomain, _threads.Count);
		}
		/// <summary>
		/// Loads configuration and create the SchedulerHandlerThreads
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
							SchedulerHandlerThread sht = _threads[n - 1];
							_threads.RemoveAt(n - 1);
							sht.Dispose();
						}
					}
					// creating the thread list
					_threads = new List<SchedulerHandlerThread>();
					foreach (GenericConfiguration s in _configuration.GetList("schedulers/scheduler"))
					{
						String schedulerName = s.Get("name").Value;
						DateTime date;
						if (s.Get("startdate").Value == "")
						{
							date = DateTime.MaxValue;
						}
						else if (s.Get("startdate").Value.ToLower() == "now")
						{
							date = DateTime.Now;
						}
						else
						{
							date = DateTime.Parse(s.Get("startdate").Value);
						}
						TimeSpan span;
						if (s.Get("timespan").Value == "")
						{
							span = TimeSpan.MaxValue;
						}
						else
						{
							span = TimeSpan.Parse(s.Get("timespan").Value);
						}
						SchedulerHandlerThread sht;
						sht = new SchedulerHandlerThread(_serviceName, _whizFlowDomain, schedulerName, _configuration, date, span, s.Get("plugins"));
						_threads.Add(sht);
					}
					Parallel.ForEach(_threads, thread =>
						{
							thread.Start();
						}
					);
					if (_threads.Count == 0)
					{
						_fakeMonitor = new Monitoring.SchedulerHandlerThread("No scheduler configured on this domain", _serviceName, _whizFlowDomain, DateTime.Now.ToString(), "0", "n/a");
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
						Log.WriteLogAsync(Log.Module.SchedulersHandler, Log.LogTypes.Error, "Schedulers Handler", "Schedulers Handler CreateThreads Error: " + e.Message, "Exception : " + e.GetType().ToString() + Environment.NewLine + e.StackTrace, _connectionString);
					}
					throw (ex);
				}
				catch (Exception ex)
				{
					Log.WriteLogAsync(Log.Module.SchedulersHandler, Log.LogTypes.Error, "Schedulers Handler", "Schedulers Handler CreateThreads Error: " + ex.Message, "Exception : " + ex.GetType().ToString() + Environment.NewLine + ex.StackTrace, _connectionString);
					throw (ex);
				}
			}
		}
		/// <summary>
		/// Gets the scheduler handler thread with the given name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public SchedulerHandlerThread GetSchedulerHandlerThread(String name)
		{
			return _threads.Where(t => t.SchedulerName == name).First();
		}
		#region IDisposable Members
		/// <summary>
		/// Dispose method. This will dispose all the SchedulerHandlerThreads object linked to the SchedulersHandler
		/// </summary>
		public void Dispose()
		{
			// we need to remove all the event handlers
			if (_threads != null)
			{
				for (Int32 n = _threads.Count; n > 0; n--)
				{
					SchedulerHandlerThread pT = _threads[n - 1];
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
