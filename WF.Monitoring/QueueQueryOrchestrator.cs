using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// Delegate for the QueueQueryOrchestrator events
	/// </summary>
	public delegate void QueuesUpdate();
	/// <summary>
	/// Query orchestrator for the queues in order to avoid every form calls directly the service
	/// </summary>
	public static class QueueQueryOrchestrator
	{
		/// <summary>
		/// Event fired when the QueueQueryOrchestrator calls the service and updates its information
		/// </summary>
		public static event QueuesUpdate Update;
		private static List<String> _hosts;
		private static Dictionary<String, List<Query.QueueHandlerThread>> _datas;
		private static System.Timers.Timer _timer;
		static QueueQueryOrchestrator()
		{
			_hosts = new List<String>();
			_datas = new Dictionary<string, List<Query.QueueHandlerThread>>();
			_timer = new System.Timers.Timer();
			_timer.Interval = 5000;
			_timer.Elapsed += _timer_Elapsed;
			_timer.Start();
		}
		static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Parallel.ForEach(_hosts, (s) =>
			{
				_datas[s] = Query.QueueHandlerThreadsGet(s);
			});
			if (Update != null) Update();
		}
		/// <summary>
		/// Enlist a WhizFlow host for orchestration
		/// </summary>
		/// <param name="host">WhizFlow host in the form http://{address}:{port}</param>
		public static void AddHost(String host)
		{
			if (!_hosts.Contains(host.ToLower()))
			{
				_hosts.Add(host.ToLower());
			}
		}
		/// <summary>
		/// Retrieves the datas for a specific host
		/// </summary>
		/// <param name="host">WhizFlow host in the form http://{address}:{port}</param>
		/// <returns>List of queue representation</returns>
		public static List<Query.QueueHandlerThread> GetQueueHandlerThreads(String host)
		{
			return _datas[host.ToLower()];
		}
	}
}
