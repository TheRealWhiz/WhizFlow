using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Collections;
using System.Diagnostics;

namespace Whiz.WhizFlow.Engine.Monitoring.Utilities.PerformanceCounters.BE
{
	/// <summary>
	/// Retrieves information exposed by WhizFlow on Performance Counters
	/// </summary>
	public static class PerformanceCounters
	{

		static Hashtable ht = new Hashtable();

		/// <summary>
		/// Retrieves how many task were processed on the specified queue
		/// </summary>
		/// <param name="machine">The machine where WhizFlow is running</param>
		/// <param name="whizFlow">The WhizFlow instance</param>
		/// <param name="domain">The internal domain</param>
		/// <param name="queue">The queue</param>
		/// <returns>The counter of processed tasks</returns>
		public static float GetQueueProcessedTasks(String machine, String whizFlow, String domain, String queue)
		{
			System.Diagnostics.PerformanceCounter tasks = new System.Diagnostics.PerformanceCounter(String.Format("Whiz.WhizFlow.{0}", whizFlow), "Tasks", String.Format("Tasks_{1}_{0}", queue, domain), machine);
			return tasks.NextValue();
		}
		/// <summary>
		/// Retrieves the number of logs to be written
		/// </summary>
		/// <param name="machine">The machine where WhizFlow is running</param>
		/// <param name="whizFlow">The WhizFlow instance</param>
		/// <param name="domain">The internal domain</param>
		/// <returns>Logs still to be written in the domain</returns>
		public static float GetLogs(String machine, String whizFlow, String domain)
		{
			System.Diagnostics.PerformanceCounter tasks = new System.Diagnostics.PerformanceCounter(String.Format("Whiz.WhizFlow.{0}", whizFlow), "Logs", String.Format("{0}_Logs", domain), machine);
			return tasks.NextValue();
		}
		/// <summary>
		/// Retrieves the throughput of a queue (tasks per second)
		/// </summary>
		/// <param name="machine">The machine where WhizFlow is running</param>
		/// <param name="whizFlow">The WhizFlow instance</param>
		/// <param name="domain">The internal domain</param>
		/// <param name="queue">The queue</param>
		/// <returns>The counter of processed tasks per second</returns>
		public static float GetQueueProcessedTasksPerSecond(String machine, String whizFlow, String domain, String queue)
		{
			System.Diagnostics.PerformanceCounter tasksPerSecond = new System.Diagnostics.PerformanceCounter(String.Format("Whiz.WhizFlow.{0}", whizFlow), "TasksPerSecond", String.Format("Tasks_Per_Second_{1}_{0}", queue, domain), machine);
			float res = 0;
			if (ht.ContainsKey(machine + whizFlow + queue))
			{
				CounterSample now = tasksPerSecond.NextSample();
				res = CounterSampleCalculator.ComputeCounterValue((CounterSample)ht[machine + whizFlow + queue], now);
				ht[machine + whizFlow + queue] = now;
			}
			else
			{
				ht.Add(machine + whizFlow + queue, tasksPerSecond.NextSample());
				CounterSample now = tasksPerSecond.NextSample();
				res = CounterSampleCalculator.ComputeCounterValue((CounterSample)ht[machine + whizFlow + queue], now);
			}
			return res;
		}
	}
}
