using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Whiz.WhizFlow.Common.Objects
{
	/// <summary>
	/// The internal task of WhizFlow constituted by a WhizFlowTaskContent associated with a Task
	/// </summary>
	public class WhizFlowTask
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public WhizFlowTask()
		{ }
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="content">The WhizFlow task content from which start</param>
		public WhizFlowTask(WhizFlowTaskContent content)
		{
			Id = content.Id;
			TimeStamp = content.TimeStamp;
			Content = content.Content;
		}
		/// <summary>
		/// This is the unique id in the database of the content. This field will be populated only when the task content is persisted on the db
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Signature of the task in order to determine how to process it
		/// </summary>
		public string Signature { get; set; }
		/// <summary>
		/// Associated time indication
		/// </summary>
		public DateTime TimeStamp { get; set; }
		/// <summary>
		/// The content of the Task
		/// </summary>
		public Object Content { get; set; }
	}
	/// <summary>
	/// The content of a WhizFlowTask
	/// </summary>
	public class WhizFlowTaskContent
	{
		/// <summary>
		/// Unique identifier of the content
		/// </summary>
		public Int32 Id { get; set; }
		/// <summary>
		/// Content
		/// </summary>
		public Object Content { get; set; }
		/// <summary>
		/// Timestamp of the content
		/// </summary>
		public DateTime TimeStamp { get; set; }
	}
}
