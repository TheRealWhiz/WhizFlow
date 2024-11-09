using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whiz.WhizFlow.Engine.Monitoring.Utilities.WMI
{
	/// <summary>
	/// WhizFlow entities exposed by WhizFlow on WMI
	/// </summary>
	public enum Entities
	{
		/// <summary>
		/// The handler of all the queues
		/// </summary>
		QueuesHandler,
		/// <summary>
		/// The handler of a single queue
		/// </summary>
		QueueHandlerThread,
		/// <summary>
		/// The handler of all the schedulers
		/// </summary>
		SchedulersHandler,
		/// <summary>
		/// The handler of a single scheduler
		/// </summary>
		SchedulerHandlerThread,
		/// <summary>
		/// The handler of all the worker
		/// </summary>
		WorkersHandler,
		/// <summary>
		/// The handler of a single worker
		/// </summary>
		WorkerHandlerThread
	}
}
