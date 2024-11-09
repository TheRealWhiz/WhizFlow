using Whiz.Framework.Configuration;
using Whiz.WhizFlow.ScheduleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhizFlowTestPlugins
{
	public class TestSchedulerProcessor : SchedulerControllerBase
	{
		public TestSchedulerProcessor(GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName, String schedulerName)
			: base(configuration, mainConfiguration, moduleName, schedulerName)
		{ }
		protected override void Dispose(bool _Disposing)
		{
		}
		public override void Process()
		{
			//InsertLog(Whiz.WhizFlow.Log.LogTypes.Information, this.GetHashCode().ToString(), "processed", "infos");
		}
	}
}
