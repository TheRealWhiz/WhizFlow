using Whiz.Framework.Configuration;
using Whiz.WhizFlow.WorkerManagement;
using System;

namespace $rootnamespace$
{
	/// <summary>
	/// WhizFlow Worker handling class
	/// </summary>
	public class WorkerProcessor : WorkerControllerBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="mainConfiguration"></param>
		/// <param name="moduleName"></param>
		/// <param name="workerName"></param>
		public WorkerProcessor(GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName, String workerName)
			: base(configuration, mainConfiguration, moduleName, workerName)
		{ }
		/// <summary>
		/// IDisposable implementation
		/// </summary>
		/// <param name="_Disposing"></param>
		protected override void Dispose(bool _Disposing)
		{
		}
		/// <summary>
		/// Your worker code called by WhizFlow
		/// </summary>
		public override void Run()
		{
		}
	}
}
