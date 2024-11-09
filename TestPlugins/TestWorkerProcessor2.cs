using Whiz.Framework.Configuration;
using Whiz.WhizFlow.ScheduleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Whiz.WhizFlow.WorkerManagement;

namespace WhizFlowTestPlugins
{
	public class TestWorkerProcessor2 : WorkerControllerBase
	{
		public TestWorkerProcessor2(GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName, String workerName)
			: base(configuration, mainConfiguration, moduleName, workerName)
		{ }
		protected override void Dispose(bool _Disposing)
		{
		}
		public override void Run()
		{
			while (true)
			{
				InsertLog(Whiz.WhizFlow.Log.LogTypes.Information, this.GetHashCode().ToString(), "processed", "infos");
				Thread.Sleep(1000);
			}
		}
	}
}
