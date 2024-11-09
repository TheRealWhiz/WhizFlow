using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whiz.WhizFlow.TasksManagement.Objects
{
	/// <summary>
	/// Class that represents a TaskWorkflow, how an implemented Manager should handle a particular incoming task
	/// </summary>
	public class TaskWorkflow
	{
		/// <summary>
		/// A free text used over all the architecture to sign a task and to categorize it
		/// </summary>
		public String Signature { get; set; }
		/// <summary>
		/// A reference to a type that can handle tasks signed with the signature
		/// </summary>
		public Type ControllerType { get; set; }
		/// <summary>
		/// Lists of WorkflowRule in order to handle correctly the task
		/// </summary>
		public List<WorkflowRule> TaskRules { get; set; }
		/// <summary>
		/// The specified order sequence for this workflow
		/// </summary>
		public List<Int32> OrderSequence { get; set; }
		/// <summary>
		/// An exploded order-rules dictionary
		/// </summary>
		public Dictionary<Int32, List<WorkflowRule>> OrdersWorkflowRules { get; set; }
		/// <summary>
		/// Constructor
		/// </summary>
		public TaskWorkflow()
		{
			TaskRules = new List<WorkflowRule>();
			OrderSequence = new List<Int32>();
			OrdersWorkflowRules = new Dictionary<Int32, List<WorkflowRule>>();
		}
		/// <summary>
		/// Calculates the workflow for quicker processing
		/// </summary>
		public void CalculateWorkflow()
		{
			OrderSequence = TaskRules.Select<WorkflowRule, Int32>(r => r.Order).Distinct().ToList();
			OrderSequence = OrderSequence.OrderBy(r => r).ToList();
			for (Int32 n = 0; n < OrderSequence.Count; n++)
			{
				OrdersWorkflowRules.Add(OrderSequence[n], TaskRules.Where(t => t.Order == OrderSequence[n]).ToList());
			}
		}
	}
	/// <summary>
	/// A collection of TaskWorkflow
	/// </summary>
	public class TaskWorkflows
	{
		/// <summary>
		/// The List of TaskWorkflowMapping
		/// </summary>
		public List<TaskWorkflow> Workflows { get; set; }
		/// <summary>
		/// Constructor
		/// </summary>
		public TaskWorkflows()
		{
			Workflows = new List<TaskWorkflow>();
		}
		/// <summary>
		/// Retrieves the TaskWorkflow that specify handling rules for a given signature (if any)
		/// </summary>
		/// <param name="signature">The given signature</param>
		/// <returns>The TaskWorkflow or null</returns>
		public TaskWorkflow GetTaskHandler(String signature)
		{
			return Workflows.Where(o => signature.Equals(o.Signature)).SingleOrDefault();
		}
	}

}