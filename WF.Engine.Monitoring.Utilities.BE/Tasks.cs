using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Collections;
using Whiz.WhizFlow.Common.Objects;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;

namespace Whiz.WhizFlow.Engine.Monitoring.Utilities.Tasks.BE
{
	/// <summary>
	/// Exposes utilities to retrieve Tasks and related informations from the WhizFlow Support Database
	/// </summary>
	public static class Tasks
	{
		/// <summary>
		/// Extracts the task content with the provided Id
		/// </summary>
		/// <param name="id">The task Id</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>The task content</returns>
		public static WhizFlowTaskContent GetTaskContent(Int32 id, String connectionString)
		{
			WhizFlowTaskContent task = null;
			using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connectionString))
			{
				sqlConn.Open();
				System.Data.SqlClient.SqlCommand sqlCom = new System.Data.SqlClient.SqlCommand("dbo.WF_TaskContent_Read", sqlConn);
				sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
				sqlCom.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = id;
				using (System.Data.SqlClient.SqlDataReader drRead = sqlCom.ExecuteReader())
				{
					if (drRead.Read())
					{
						task = new WhizFlowTaskContent();
						if (drRead["Serialized"].ToString() == "True")
						{
							if (drRead["Disk"].ToString() == "True")
							{
								task.Content = DeserializeContent(GetContent(drRead["Content"].ToString()));
							}
							else
							{
								task.Content = DeserializeContent(drRead["Content"].ToString());
							}
						}
						else
						{
							if (drRead["Disk"].ToString() == "True")
							{
								task.Content = GetContent(drRead["Content"].ToString());
							}
							else
							{
								task.Content = drRead["Content"].ToString();
							}
						}
						task.TimeStamp = (DateTime)drRead["TimeStamp"];
						task.Id = (Int32)drRead["Id"];
					}
				}
			}
			return task;
		}
		/// <summary>
		/// Extracts processing information for the specified task content id
		/// </summary>
		/// <param name="taskContentId">The task content id</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<TaskInformation> GetTaskInformation(Int32 taskContentId, String connectionString)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_Read";
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					TaskInformation t = new TaskInformation();
					t.TaskContentId = (Int32)dr["fk_TaskContentId"];
					t.Signature = (String)dr["Signature"];
					t.Time = (DateTime)dr["TimeStamp"];
					t.Queue = (String)dr["TaskQueue"];
					t.ProcessingTime = (Int32)dr["Milliseconds"];
					t.HostName = (String)dr["HostName"];
					t.Service = (String)dr["Service"];
					t.Domain = (String)dr["Domain"];
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extracts processing information for the specified task content id in a non specific serializable structure
		/// </summary>
		/// <param name="taskContentId">The task content id</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<List<Object>> GetTaskInformationSerialize(Int32 taskContentId, String connectionString)
		{
			List<List<Object>> result = new List<List<Object>>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_Read";
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					List<Object> t = new List<Object>();
					t.Add((Int32)dr["fk_TaskContentId"]);
					t.Add((String)dr["Signature"]);
					t.Add((DateTime)dr["TimeStamp"]);
					t.Add((String)dr["TaskQueue"]);
					t.Add((Int32)dr["Milliseconds"]);
					t.Add((String)dr["HostName"]);
					t.Add((String)dr["Service"]);
					t.Add((String)dr["Domain"]);
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Converts from the database saved task to a real task
		/// </summary>
		/// <param name="serializedTask">The serialized task</param>
		/// <returns>The task deserialized</returns>
		private static WhizFlowTask DeserializeTask(String serializedTask)
		{
			WhizFlowTask result;
			XmlSerializer xmlSer = new XmlSerializer(typeof(WhizFlowTask));
			result = (WhizFlowTask)xmlSer.Deserialize(new StringReader(serializedTask));
			return result;
		}
		/// <summary>
		/// Converts from the database saved task to a deserialized task (for content different from string type)
		/// </summary>
		/// <param name="serializedTaskContent">The serialized content</param>
		/// <returns>The deserialized object</returns>
		private static Object DeserializeContent(String serializedTaskContent)
		{
			Object result;
			XmlSerializer xmlSer = new XmlSerializer(typeof(Object));
			result = (Object)xmlSer.Deserialize(new StringReader(serializedTaskContent));
			return result;
		}
		/// <summary>
		/// Gets the task content from db or disk as indicated
		/// </summary>
		/// <param name="contentFile">The content file in the database field</param>
		/// <returns>The task content</returns>
		private static String GetContent(String contentFile)
		{
			StreamReader file = File.OpenText(contentFile);
			return file.ReadToEnd();
		}
		/// <summary>
		/// Extract the last (n) processing information for the specified Queue
		/// </summary>
		/// <param name="numberOfEntries">Number of information to retrieve</param>
		/// <param name="queue">The queue</param>
		/// <param name="hostname">The host who processed the tasks</param>
		/// <param name="service">The service who processed the tasks</param>
		/// <param name="domain">The domain who processed the tasks</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<TaskInformation> GetQueueProcessed(Int32 numberOfEntries, String queue, String hostname, String service, String domain, String connectionString)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_ReadByQueueLastProcessed";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("Queue", System.Data.SqlDbType.VarChar, 50).Value = queue;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = numberOfEntries;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					TaskInformation t = new TaskInformation();
					t.TaskContentId = (Int32)dr["fk_TaskContentId"];
					t.Signature = (String)dr["Signature"];
					t.Time = (DateTime)dr["TimeStamp"];
					t.Queue = (String)dr["TaskQueue"];
					t.ProcessingTime = (Int32)dr["Milliseconds"];
					t.HostName = (String)dr["HostName"];
					t.Service = (String)dr["Service"];
					t.Domain = (String)dr["Domain"];
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extract the last (n) processing information for the specified Queue in a non specific serializable structure
		/// </summary>
		/// <param name="numberOfEntries">Number of information to retrieve</param>
		/// <param name="queue">The queue</param>
		/// <param name="hostname">The host who processed the tasks</param>
		/// <param name="service">The service who processed the tasks</param>
		/// <param name="domain">The domain who processed the tasks</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<List<Object>> GetQueueProcessedSerialize(Int32 numberOfEntries, String queue, String hostname, String service, String domain, String connectionString)
		{
			List<List<Object>> result = new List<List<Object>>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_ReadByQueueLastProcessed";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("Queue", System.Data.SqlDbType.VarChar, 50).Value = queue;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = numberOfEntries;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					List<Object> t = new List<Object>();
					t.Add((Int32)dr["fk_TaskContentId"]);
					t.Add((String)dr["Signature"]);
					t.Add((DateTime)dr["TimeStamp"]);
					t.Add((String)dr["TaskQueue"]);
					t.Add((Int32)dr["Milliseconds"]);
					t.Add((String)dr["HostName"]);
					t.Add((String)dr["Service"]);
					t.Add((String)dr["Domain"]);
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extract processing information for the specified queue in the supplied timeframe
		/// </summary>
		/// <param name="from">The beginning of the time frame</param>
		/// <param name="to">The end of the time frame</param>
		/// <param name="queue">The queue</param>
		/// <param name="hostname">The host who processed the tasks</param>
		/// <param name="service">The service who processed the tasks</param>
		/// <param name="domain">The domain who processed the tasks</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<TaskInformation> GetQueueProcessed(DateTime from, DateTime to, String queue, String hostname, String service, String domain, String connectionString)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_ReadByQueueTimeFrame";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("Queue", System.Data.SqlDbType.VarChar, 50).Value = queue;
				command.Parameters.Add("From", System.Data.SqlDbType.DateTime).Value = from;
				command.Parameters.Add("To", System.Data.SqlDbType.DateTime).Value = to;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					TaskInformation t = new TaskInformation();
					t.TaskContentId = (Int32)dr["fk_TaskContentId"];
					t.Signature = (String)dr["Signature"];
					t.Time = (DateTime)dr["TimeStamp"];
					t.Queue = (String)dr["TaskQueue"];
					t.ProcessingTime = (Int32)dr["Milliseconds"];
					t.HostName = (String)dr["HostName"];
					t.Service = (String)dr["Service"];
					t.Domain = (String)dr["Domain"];
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extract processing information for the specified queue in the supplied timeframe in a non specific serializable structure
		/// </summary>
		/// <param name="from">The beginning of the time frame</param>
		/// <param name="to">The end of the time frame</param>
		/// <param name="queue">The queue</param>
		/// <param name="hostname">The host who processed the tasks</param>
		/// <param name="service">The service who processed the tasks</param>
		/// <param name="domain">The domain who processed the tasks</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<List<Object>> GetQueueProcessedSerialize(DateTime from, DateTime to, String queue, String hostname, String service, String domain, String connectionString)
		{
			List<List<Object>> result = new List<List<Object>>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_ReadByQueueTimeFrame";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("Queue", System.Data.SqlDbType.VarChar, 50).Value = queue;
				command.Parameters.Add("From", System.Data.SqlDbType.DateTime).Value = from;
				command.Parameters.Add("To", System.Data.SqlDbType.DateTime).Value = to;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					List<Object> t = new List<Object>();
					t.Add((Int32)dr["fk_TaskContentId"]);
					t.Add((String)dr["Signature"]);
					t.Add((DateTime)dr["TimeStamp"]);
					t.Add((String)dr["TaskQueue"]);
					t.Add((Int32)dr["Milliseconds"]);
					t.Add((String)dr["HostName"]);
					t.Add((String)dr["Service"]);
					t.Add((String)dr["Domain"]);
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extract processing information in the supplied timeframe
		/// </summary>
		/// <param name="from">The beginning of the time frame</param>
		/// <param name="to">The end of the time frame</param>
		/// <param name="hostname">The host who processed the tasks</param>
		/// <param name="service">The service who processed the tasks</param>
		/// <param name="domain">The domain who processed the tasks</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<TaskInformation> GetProcessed(DateTime from, DateTime to, String hostname, String service, String domain, String connectionString)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_ReadByTimeFrame";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("From", System.Data.SqlDbType.DateTime).Value = from;
				command.Parameters.Add("To", System.Data.SqlDbType.DateTime).Value = to;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					TaskInformation t = new TaskInformation();
					t.TaskContentId = (Int32)dr["fk_TaskContentId"];
					t.Signature = (String)dr["Signature"];
					t.Time = (DateTime)dr["TimeStamp"];
					t.Queue = (String)dr["TaskQueue"];
					t.ProcessingTime = (Int32)dr["Milliseconds"];
					t.HostName = (String)dr["HostName"];
					t.Service = (String)dr["Service"];
					t.Domain = (String)dr["Domain"];
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extract processing information in the supplied timeframe in a non specific serializable structure
		/// </summary>
		/// <param name="from">The beginning of the time frame</param>
		/// <param name="to">The end of the time frame</param>
		/// <param name="hostname">The host who processed the tasks</param>
		/// <param name="service">The service who processed the tasks</param>
		/// <param name="domain">The domain who processed the tasks</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<List<Object>> GetProcessedSerialize(DateTime from, DateTime to, String hostname, String service, String domain, String connectionString)
		{
			List<List<Object>> result = new List<List<Object>>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_ReadByTimeFrame";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("From", System.Data.SqlDbType.DateTime).Value = from;
				command.Parameters.Add("To", System.Data.SqlDbType.DateTime).Value = to;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					List<Object> t = new List<Object>();
					t.Add((Int32)dr["fk_TaskContentId"]);
					t.Add((String)dr["Signature"]);
					t.Add((DateTime)dr["TimeStamp"]);
					t.Add((String)dr["TaskQueue"]);
					t.Add((Int32)dr["Milliseconds"]);
					t.Add((String)dr["HostName"]);
					t.Add((String)dr["Service"]);
					t.Add((String)dr["Domain"]);
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extract the last (n) processing information
		/// </summary>
		/// <param name="numberOfEntries">Number of information to retrieve</param>
		/// <param name="hostname">The host who processed the tasks</param>
		/// <param name="service">The service who processed the tasks</param>
		/// <param name="domain">The domain who processed the tasks</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<TaskInformation> GetProcessed(Int32 numberOfEntries, String hostname, String service, String domain, String connectionString)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_ReadByLastProcessed";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = numberOfEntries;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					TaskInformation t = new TaskInformation();
					t.TaskContentId = (Int32)dr["fk_TaskContentId"];
					t.Signature = (String)dr["Signature"];
					t.Time = (DateTime)dr["TimeStamp"];
					t.Queue = (String)dr["TaskQueue"];
					t.ProcessingTime = (Int32)dr["Milliseconds"];
					t.HostName = (String)dr["HostName"];
					t.Service = (String)dr["Service"];
					t.Domain = (String)dr["Domain"];
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extract the last (n) processing information in a non speicific serializable structure
		/// </summary>
		/// <param name="numberOfEntries">Number of information to retrieve</param>
		/// <param name="hostname">The host who processed the tasks</param>
		/// <param name="service">The service who processed the tasks</param>
		/// <param name="domain">The domain who processed the tasks</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>List of task information</returns>
		public static List<List<Object>> GetProcessedSerialize(Int32 numberOfEntries, String hostname, String service, String domain, String connectionString)
		{
			List<List<Object>> result = new List<List<Object>>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_QHTPerformances_ReadByLastProcessed";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = numberOfEntries;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					List<Object> t = new List<Object>();
					t.Add((Int32)dr["fk_TaskContentId"]);
					t.Add((String)dr["Signature"]);
					t.Add((DateTime)dr["TimeStamp"]);
					t.Add((String)dr["TaskQueue"]);
					t.Add((Int32)dr["Milliseconds"]);
					t.Add((String)dr["HostName"]);
					t.Add((String)dr["Service"]);
					t.Add((String)dr["Domain"]);
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
	}
}
