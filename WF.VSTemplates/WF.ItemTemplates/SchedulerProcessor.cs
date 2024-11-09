using Whiz.Framework.Configuration;
using Whiz.WhizFlow.ScheduleManagement;
using System;

namespace $rootnamespace$
{
	/// <summary>
	/// WhizFlow Scheduler handling class. This class will be used when a time event happens
	/// </summary>
	public class SchedulerProcessor : SchedulerControllerBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="mainConfiguration"></param>
		/// <param name="moduleName"></param>
		/// <param name="schedulerName"></param>
		public SchedulerProcessor(GenericConfiguration configuration, GenericConfiguration mainConfiguration, String moduleName, String schedulerName)
			: base(configuration, mainConfiguration, moduleName, schedulerName)
		{ }
		/// <summary>
		/// IDisposable implementation
		/// </summary>
		/// <param name="_Disposing"></param>
		protected override void Dispose(bool _Disposing)
		{
		}
		/// <summary>
		/// Method called when a time event happens
		/// </summary>
		public override void Process()
		{
		}
	}
}
