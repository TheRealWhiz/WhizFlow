using Whiz.Framework.Configuration;
using Whiz.WhizFlow.Common.Objects;
using Whiz.WhizFlow.TasksManagement;
using System;
using System.Collections.Generic;

namespace $rootnamespace$
{
	/// <summary>
	/// WhizFlow Task handling class. TaskWorkflow have reference to class inherited from ControllerBase
	/// in order to handle Tasks.
	/// </summary>
	public class QueueProcessor : ControllerBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="task"></param>
		/// <param name="queue"></param>
		/// <param name="configuration"></param>
		/// <param name="mainConfiguration"></param>
		/// <param name="moduleName"></param>
		public QueueProcessor(WhizFlowTask task, String queue, GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName)
			: base(task, queue, configuration, mainConfiguration, moduleName)
		{ }
		/// <summary>
		/// This is the method called by WhizFlow to process a step of the workflow. The rule passed may contain additional informations
		/// specified by the manager in order to correctly handle the task. This method could optionally return other workflow steps that will
		/// be executed immediately after the end of the current step but before the next step
		/// </summary>
		/// <param name="rule"></param>
		/// <returns></returns>
		public override List<Whiz.WhizFlow.TasksManagement.Objects.WorkflowRule> Process(Whiz.WhizFlow.TasksManagement.Objects.WorkflowRule rule)
		{
			return null;
		}
		/// <summary>
		/// IDisposable implementation
		/// </summary>
		/// <param name="_Disposing"></param>
		protected override void Dispose(bool _Disposing)
		{

		}
	}
}
