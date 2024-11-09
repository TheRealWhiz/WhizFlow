using Whiz.Framework.Configuration;
using Whiz.WhizFlow.TasksManagement;
using Whiz.WhizFlow.TasksManagement.Objects;
using System;

namespace $rootnamespace$
{
	/// <summary>
	/// Manager Class. This class is used by WhizFlow when a task is present in the queue.
	/// WhizFlow ask to this implementation what to do with this task basing on the TaskWorkflows
	/// provided by the LoadWorkflows method
	/// </summary>
	public class Manager : ManagerBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="local"></param>
		/// <param name="main"></param>
		/// <param name="moduleName"></param>
		/// <param name="queueName"></param>
		public Manager(GenericConfiguration local, GenericConfiguration main, String moduleName, String queueName)
			: base(local, main, moduleName, queueName)
		{ }
		/// <summary>
		/// Provide to WhizFlow the workflows configuration
		/// </summary>
		/// <returns></returns>
		public override TaskWorkflows LoadWorkflows()
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
