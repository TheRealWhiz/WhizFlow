using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whiz.WhizFlow.Engine.Monitoring.PerformanceCounters
{
	/// <summary>
	/// PerformanceCounters utility class
	/// Allows to create or destroy the WhizFlow instance specific performance counter category
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// Logs performance counter
		/// </summary>
		public const String LOGS_COUNTER_NAME = "Logs";
		/// <summary>
		/// Performance counter name
		/// </summary>
		public const String RECEIVED_TASKS_PERFORMANCE_COUNTER_NAME = "Tasks";
		/// <summary>
		/// Performance counter name
		/// </summary>
		public const String RECEIVED_TASKS_PER_SECOND_PERFORMANCE_COUNTER_NAME = "TasksPerSecond";
		/// <summary>
		/// Formats the category name for the performance counters
		/// </summary>
		/// <param name="whizFlowName">WhizFlow service instance name</param>
		/// <returns>The category name</returns>
		public static String FormatPerformanceCounterCategoryName(String whizFlowName)
		{
			return String.Format("Whiz.WhizFlow.{0}", whizFlowName);
		}
		/// <summary>
		/// Creates all the necessary performance counters for the given WhizFlow service instance
		/// </summary>
		/// <param name="whizFlowName">WhizFlow service instance name</param>
		public static void CreateWhizFlowPerformanceCounters(String whizFlowName)
		{
			if (!(System.Diagnostics.PerformanceCounterCategory.Exists(FormatPerformanceCounterCategoryName(whizFlowName))))
			{
				System.Diagnostics.CounterCreationDataCollection pPFDataCollection = new System.Diagnostics.CounterCreationDataCollection();
				System.Diagnostics.CounterCreationData pf1 = new System.Diagnostics.CounterCreationData();
				pf1.CounterType = System.Diagnostics.PerformanceCounterType.NumberOfItems64;
				pf1.CounterName = RECEIVED_TASKS_PERFORMANCE_COUNTER_NAME;
				pPFDataCollection.Add(pf1);
				System.Diagnostics.CounterCreationData pf2 = new System.Diagnostics.CounterCreationData();
				pf2.CounterType = System.Diagnostics.PerformanceCounterType.RateOfCountsPerSecond64;
				pf2.CounterName = RECEIVED_TASKS_PER_SECOND_PERFORMANCE_COUNTER_NAME;
				pPFDataCollection.Add(pf2);
				System.Diagnostics.CounterCreationData pf3 = new System.Diagnostics.CounterCreationData();
				pf3.CounterType = System.Diagnostics.PerformanceCounterType.NumberOfItems64;
				pf3.CounterName = LOGS_COUNTER_NAME;
				pPFDataCollection.Add(pf3);
				System.Diagnostics.PerformanceCounterCategory.Create(FormatPerformanceCounterCategoryName(whizFlowName), "WhizFlow Performance Counters", System.Diagnostics.PerformanceCounterCategoryType.MultiInstance, pPFDataCollection);
			}
		}
		/// <summary>
		/// Removes all the performance counters for the given WhizFlow service instance
		/// </summary>
		/// <param name="whizFlowName">WhizFlow service instance name</param>
		public static void DeleteWhizFlowPerformanceCounters(String whizFlowName)
		{
			if (System.Diagnostics.PerformanceCounterCategory.Exists(FormatPerformanceCounterCategoryName(whizFlowName)))
			{
				System.Diagnostics.PerformanceCounterCategory.Delete(FormatPerformanceCounterCategoryName(whizFlowName));
			}
		}

	}
}
