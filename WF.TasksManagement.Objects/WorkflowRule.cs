using System;

namespace Whiz.WhizFlow.TasksManagement.Objects
{
	/// <summary>
	/// Class that represent a single Rule for handling a input Task
	/// </summary>
	public class WorkflowRule
	{
		/// <summary>
		/// Order for this rule in the whole workflow. Rule with the same order could be executed in parallel.
		/// </summary>
		public Int32 Order { get; set; }
		/// <summary>
		/// Free field used by implementation for giving additional information or datas for the workflow
		/// </summary>
		public Object OperationParameter { get; set; }
		/// <summary>
		/// Constructor
		/// </summary>
		public WorkflowRule() { }
	}
}
