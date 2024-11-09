using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Whiz.WhizFlow.Common;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Whiz.WhizFlow.Common.Objects;

namespace Whiz.WhizFlow
{
	/// <summary>
	/// The most common operations on the database for WhizFlow
	/// </summary>
	public static class DAL
	{
		/// <summary>
		/// Save a task for the given task content in the given task queue on the database.
		/// </summary>
		/// <param name="task">The task who need to be processed</param>
		/// <param name="queue">The task queue</param>
		/// <param name="connectionString">The database on which save the task</param>
		public static void SaveTask(WhizFlowTask task, String queue, String connectionString)
		{
			SaveTask(task.Id, task.Signature, queue, connectionString);
		}
		/// <summary>
		/// Setups a dedicated queue
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="connectionString"></param>
		public static void SetupDedicatedQueue(String queue, String connectionString)
		{
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_GenerateDedicatedQueue";
				command.Parameters.Add("QueueName", System.Data.SqlDbType.VarChar, 50).Value = queue;
				command.Connection = connection;
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		/// <summary>
		/// Save a task for the given task content in the given dedicated task queue on the database.
		/// </summary>
		/// <param name="task">The task who need to be processed</param>
		/// <param name="queue">The task queue</param>
		/// <param name="connectionString">The database on which save the task</param>
		public static void SaveTaskDedicated(WhizFlowTask task, String queue, String connectionString)
		{
			SaveTaskDedicated(task.Id, task.Signature, queue, connectionString);
		}
		/// <summary>
		/// Save a task for the given task content in the given task queue on the database.
		/// </summary>
		/// <param name="taskContentId">The task content id</param>
		/// <param name="signature">The signature associated with the task</param>
		/// <param name="queue">The task queue</param>
		/// <param name="connectionString">The database on which save the task</param>
		public static void SaveTask(Int32 taskContentId, String signature, String queue, String connectionString)
		{
			SqlCommand pCommand;
			using (SqlConnection pConnection = new SqlConnection(connectionString))
			{
				pConnection.Open();
				pCommand = new SqlCommand();
				pCommand.CommandType = System.Data.CommandType.StoredProcedure;
				pCommand.CommandText = "dbo.WF_Task_Write";
				pCommand.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				pCommand.Parameters.Add("TaskQueue", System.Data.SqlDbType.VarChar, 50).Value = queue;
				pCommand.Parameters.Add("Signature", System.Data.SqlDbType.VarChar, 50).Value = signature;
				pCommand.Connection = pConnection;
				pCommand.ExecuteNonQuery();
				pConnection.Close();
			}
		}
		/// <summary>
		/// Save a task for the given task content in the given task queue on the database.
		/// </summary>
		/// <param name="taskContentId">The task content id</param>
		/// <param name="signature">The signature associated with the task</param>
		/// <param name="taskQueue">The task queue</param>
		/// <param name="connectionString">The database on which save the task</param>
		public static void SaveTaskDedicated(Int32 taskContentId, String signature, String taskQueue, String connectionString)
		{
			SqlCommand pCommand;
			using (SqlConnection pConnection = new SqlConnection(connectionString))
			{
				pConnection.Open();
				pCommand = new SqlCommand();
				pCommand.CommandType = System.Data.CommandType.StoredProcedure;
				pCommand.CommandText = String.Format("dbo.WF_Task_{0}_Write", taskQueue);
				pCommand.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				pCommand.Parameters.Add("Signature", System.Data.SqlDbType.VarChar, 50).Value = signature;
				pCommand.Connection = pConnection;
				pCommand.ExecuteNonQuery();
				pConnection.Close();
			}
		}
		/// <summary>
		/// Save a given task content on the database.
		/// </summary>
		/// <param name="taskContent">The taskContent to save</param>
		/// <param name="connectionString">The database on which save the task content</param>
		/// <param name="disk">The task content will be saved to disk</param>
		/// <param name="diskThreshold">When the size of content is greater than this, the task content will be saved to disk</param>
		/// <param name="basePath">The base path from which WhizFlow creates his structure for task content disk save</param>
		/// <returns>The unique generated id of the saved task content</returns>
		public static Int32 SaveTaskContent(WhizFlowTaskContent taskContent, String connectionString, Boolean disk, Int32 diskThreshold, String basePath)
		{
			SqlDataReader dr;
			SqlCommand command;
			Int32 result = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String content;
				StringWriter stringWriter = null;
				if (!(taskContent.Content.GetType() == typeof(String)))
				{
					XmlSerializer xmlSer = new XmlSerializer(typeof(Object));
					stringWriter = new StringWriter();
					xmlSer.Serialize(stringWriter, taskContent.Content);
					content = stringWriter.ToString();
				}
				else
				{
					content = (String)taskContent.Content;
				}
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_TaskContent_Write";
				String destinationDir;
				String fileName;
				String finalFile = "";
				Boolean effectiveDisk = false;
				if (disk)
				{
					if ((diskThreshold <= 0) || (content.Length > diskThreshold))
					{
						destinationDir = basePath + DateTime.Now.Date.ToString("yyyy-MM-dd") + @"\" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString();
						fileName = Guid.NewGuid().ToString();
						if (!Directory.Exists(destinationDir))
						{ Directory.CreateDirectory(destinationDir); }
						finalFile = String.Concat(destinationDir, @"\", fileName);
						StreamWriter pFile = File.CreateText(finalFile);
						pFile.Write(content);
						pFile.Close();
						effectiveDisk = true;
						command.Parameters.Add("Content", System.Data.SqlDbType.VarChar, -1).Value = finalFile;
					}
					else
					{
						command.Parameters.Add("Content", System.Data.SqlDbType.VarChar, -1).Value = content;
					}
				}
				else
				{
					command.Parameters.Add("Content", System.Data.SqlDbType.VarChar, -1).Value = content;
				}
				command.Parameters.Add("Timestamp", System.Data.SqlDbType.DateTime).Value = taskContent.TimeStamp;
				command.Parameters.Add("Serialized", System.Data.SqlDbType.Bit).Value = (!(taskContent.Content.GetType() == typeof(String)));
				command.Parameters.Add("Disk", System.Data.SqlDbType.Bit).Value = effectiveDisk;
				command.Connection = connection;
				dr = command.ExecuteReader();
				if (dr.Read())
				{
					result = Int32.Parse(dr["Id"].ToString());
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Save a given task content on the database.
		/// </summary>
		/// <param name="taskContent">The task content to save</param>
		/// <param name="connectionString">The database on which save the task content</param>
		/// <returns>The unique generated id of the saved task content</returns>
		public static Int32 SaveTaskContent(WhizFlowTaskContent taskContent, String connectionString)
		{
			SqlDataReader dr;
			SqlCommand command;
			Int32 result = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String content;
				StringWriter stringWriter = null;
				if (!(taskContent.Content.GetType() == typeof(String)))
				{
					XmlSerializer xmlSer = new XmlSerializer(typeof(Object));
					stringWriter = new StringWriter();
					xmlSer.Serialize(stringWriter, taskContent.Content);
					content = stringWriter.ToString();
				}
				else
				{
					content = (String)taskContent.Content;
				}
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_TaskContent_Write";
				command.Parameters.Add("Content", System.Data.SqlDbType.VarChar, -1).Value = content;
				command.Parameters.Add("Timestamp", System.Data.SqlDbType.DateTime).Value = taskContent.TimeStamp;
				command.Parameters.Add("Serialized", System.Data.SqlDbType.Bit).Value = (!(taskContent.Content.GetType() == typeof(String)));
				command.Parameters.Add("Disk", System.Data.SqlDbType.Bit).Value = false;
				command.Connection = connection;
				dr = command.ExecuteReader();
				if (dr.Read())
				{
					result = Int32.Parse(dr["Id"].ToString());
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Extracts the task content with the provided Id
		/// </summary>
		/// <param name="id">The task Id</param>
		/// <param name="connectionString">The WhizFlow Database connection string</param>
		/// <returns>The task content</returns>
		public static WhizFlowTaskContent ReadTaskContent(Int32 id, String connectionString)
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
		/// Saves a WhizFlow configuration
		/// </summary>
		/// <param name="configuration">The configuration to save</param>
		/// <param name="connectionString">WhizFlow database connection string</param>
		public static void SaveWhizFlowConfiguration(WhizFlowConfiguration configuration, String connectionString)
		{
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Configuration_Write";
				command.Parameters.Add("Hostname", System.Data.SqlDbType.VarChar, 255).Value = configuration.Hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = configuration.Service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = configuration.Domain;
				if (configuration.Configuration != null) command.Parameters.Add("Configuration", System.Data.SqlDbType.VarChar, -1).Value = configuration.Configuration;
				else command.Parameters.Add("Configuration", System.Data.SqlDbType.VarChar, -1).Value = DBNull.Value;
				command.Parameters.Add("Active", System.Data.SqlDbType.Bit).Value = configuration.Active;
				command.Connection = connection;
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		/// <summary>
		/// Reads all the active configurations for the given host/service
		/// </summary>
		/// <param name="hostname">The host name</param>
		/// <param name="service">The service name</param>
		/// <param name="connectionString">WhizFlow database connection string</param>
		/// <returns>List of WhizFlowConfigurations</returns>
		public static List<WhizFlowConfiguration> ReadWhizFlowConfigurations(String hostname, String service, String connectionString)
		{
			List<WhizFlowConfiguration> configs = new List<WhizFlowConfiguration>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Configurations_Read";
				command.Parameters.Add("Hostname", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					WhizFlowConfiguration c = new WhizFlowConfiguration();
					c.Active = (Boolean)dr["Active"];
					c.Configuration = (String)dr["Configuration"];
					c.Domain = (String)dr["Domain"];
					c.Hostname = (String)dr["Hostname"];
					c.Service = (String)dr["Service"];
					c.Id = (Int32)dr["Id"];
					configs.Add(c);
				}
				dr.Close();
				connection.Close();
			}
			return configs;
		}
		/// <summary>
		/// Gets all the stored WhizFlow configurations stored in the database
		/// </summary>
		/// <param name="connectionString">WhizFlow database connection string</param>
		/// <returns>List of WhizFlowConfigurations</returns>
		public static List<WhizFlowConfiguration> ReadAllWhizFlowConfigurations(String connectionString)
		{
			List<WhizFlowConfiguration> configs = new List<WhizFlowConfiguration>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Configurations_ReadAll";
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					WhizFlowConfiguration c = new WhizFlowConfiguration();
					c.Active = (Boolean)dr["Active"];
					c.Configuration = (String)dr["Configuration"];
					c.Domain = (String)dr["Domain"];
					c.Hostname = (String)dr["Hostname"];
					c.Service = (String)dr["Service"];
					c.Id = (Int32)dr["Id"];
					configs.Add(c);
				}
				dr.Close();
				connection.Close();
			}
			return configs;
		}
		/// <summary>
		/// Reads a specific configuration for the given host/service/domain
		/// </summary>
		/// <param name="hostname"></param>
		/// <param name="service"></param>
		/// <param name="domain"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static WhizFlowConfiguration ReadWhizFlowConfiguration(String hostname, String service, String domain, String connectionString)
		{
			WhizFlowConfiguration result = null;
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Configuration_Read";
				command.Parameters.Add("Hostname", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					result = new WhizFlowConfiguration();
					result.Active = (Boolean)dr["Active"];
					result.Configuration = (String)dr["Configuration"];
					result.Domain = (String)dr["Domain"];
					result.Hostname = (String)dr["Hostname"];
					result.Service = (String)dr["Service"];
					result.Id = (Int32)dr["Id"];
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostname"></param>
		/// <param name="service"></param>
		/// <param name="domain"></param>
		/// <param name="connectionString"></param>
		public static void DeleteWhizFlowConfiguration(String hostname, String service, String domain, String connectionString)
		{
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Configuration_Delete";
				command.Parameters.Add("Hostname", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Connection = connection;
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
	}
}
