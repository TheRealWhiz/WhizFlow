using Whiz.Framework.Configuration;
using Whiz.WhizFlow;
using Whiz.WhizFlow.Common.Objects;
using Whiz.WhizFlow.TasksManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhizFlowTestPlugins
{
	public class TestQueueProcessor : ControllerBase
	{
		public TestQueueProcessor(WhizFlowTask task, String queue, GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName) : base(task, queue, configuration, mainConfiguration, moduleName) { }
		public override List<Whiz.WhizFlow.TasksManagement.Objects.WorkflowRule> Process(Whiz.WhizFlow.TasksManagement.Objects.WorkflowRule rule)
		{
			//throw new Exception();
			InsertLog(Log.LogTypes.Information, "testqueueprocessor - " + Queue, CurrentTask.Content.ToString(), "processing");
			return null;
		}
		protected override void Dispose(bool _Disposing)
		{

		}
	}
}
