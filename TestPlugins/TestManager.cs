using Whiz.Framework.Configuration;
using Whiz.WhizFlow.TasksManagement;
using Whiz.WhizFlow.TasksManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhizFlowTestPlugins
{
	public class TestManager : ManagerBase
	{

		public TestManager(GenericConfiguration local, GenericConfiguration main, String moduleName, String queueName)
			: base(local, main, moduleName, queueName)
		{
		}

		public override TaskWorkflows LoadWorkflows()
		{
			TaskWorkflows tws = new TaskWorkflows();
			TaskWorkflow tw = new TaskWorkflow();
			WorkflowRule wr = new WorkflowRule();
			wr.Order = 0;
			wr.OperationParameter = "test";
			tw.TaskRules.Add(wr);
			tw.Signature = "1";
			tw.ControllerType = typeof(TestQueueProcessor);
			tw.CalculateWorkflow();
			tws.Workflows.Add(tw);
			return tws;
		}

		protected override void Dispose(bool _Disposing)
		{
		}
	}
}
