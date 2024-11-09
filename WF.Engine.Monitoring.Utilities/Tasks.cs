using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whiz.WhizFlow.Engine.Monitoring.Utilities.Tasks
{
	/// <summary>
	/// Task processing information class
	/// </summary>
	public class TaskInformation
	{
		/// <summary>
		/// HostName
		/// </summary>
		public String HostName { get; set; }
		/// <summary>
		/// WhizFlow Service name
		/// </summary>
		public String Service { get; set; }
		/// <summary>
		/// WhizFlow Domain
		/// </summary>
		public String Domain { get; set; }
		/// <summary>
		/// Task content id
		/// </summary>
		public Int32 TaskContentId { get; set; }
		/// <summary>
		/// Processing time in milliseconds
		/// </summary>
		public Int32 ProcessingTime { get; set; }
		/// <summary>
		/// The signature given to the task
		/// </summary>
		public String Signature { get; set; }
		/// <summary>
		/// The queue assigned to the task
		/// </summary>
		public String Queue { get; set; }
		/// <summary>
		/// The timestamp of the information
		/// </summary>
		public DateTime Time { get; set; }
		/// <summary>
		/// Rebuild a TaskInformation object starting from a non specific structure
		/// </summary>
		/// <param name="serializedObject">The non specific structure</param>
		/// <returns></returns>
		public static TaskInformation FromSerializedObject(List<Object> serializedObject)
		{
			TaskInformation result = new TaskInformation();
			result.TaskContentId = (Int32)serializedObject[0];
			result.Signature = (String)serializedObject[1];
			result.Time = (DateTime)serializedObject[2];
			result.Queue = (String)serializedObject[3];
			result.ProcessingTime = (Int32)serializedObject[4];
			result.HostName = (String)serializedObject[5];
			result.Service = (String)serializedObject[6];
			result.Domain = (String)serializedObject[7];
			return result;
		}
	}
}
