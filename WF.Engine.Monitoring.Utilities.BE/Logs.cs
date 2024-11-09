using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Collections;
using System.Data.SqlClient;

namespace Whiz.WhizFlow.Engine.Monitoring.Utilities.Logs.BE
{
	/// <summary>
	/// Exposes methods to access WhizFlow Logs
	/// </summary>
	public static class Logs
	{
		/// <summary>
		/// Returns a list of log entries that meets the expected criteria
		/// </summary>
		/// <param name="numberOfEntries">Number of log entries to return (maximum)</param>
		/// <param name="module">Module to search</param>
		/// <param name="logType">Log type to search</param>
		/// <param name="hostName">Machine where log entry were generated</param>
		/// <param name="service">WhizFlow service name who generated the log</param>
		/// <param name="domain">Domain who generated the log</param>
		/// <param name="connectionString">WhizFlow database connection string</param>
		/// <returns>The list of log entries</returns>
		public static List<LogEntry> GetLogs(Int32 numberOfEntries, Modules module, LogTypes logType, String hostName, String service, String domain, String connectionString)
		{
			List<LogEntry> result = new List<LogEntry>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Logs_ReadByLastProcessed";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = numberOfEntries;
				if (module == Modules.All)
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				}
				if (logType == LogTypes.All)
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				}
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					LogEntry t = new LogEntry();
					t.Id = Int32.Parse(dr["Id"].ToString());
					t.HostName = dr["HostName"].ToString();
					t.Service = (String)dr["Service"];
					t.Time = (DateTime)dr["Time"];
					t.AdditionalInformation = dr["AdditionalInformation"].ToString();
					if (dr["fk_TaskContentId"] != DBNull.Value)
					{
						t.TaskContentId = Int32.Parse(dr["fk_TaskContentId"].ToString());
					}
					else
					{
						t.TaskContentId = null;
					}
					t.Message = dr["Message"].ToString();
					t.Object = dr["Object"].ToString();
					t.Domain = (String)dr["Domain"];
					t.LogType = (LogTypes)Int32.Parse(dr["LogTypeId"].ToString());
					t.Module = (Modules)Int32.Parse(dr["ModuleId"].ToString());
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Returns a list of log entries that meets the expected criteria in a non specific serializable structure
		/// </summary>
		/// <param name="numberOfEntries">Number of log entries to return (maximum)</param>
		/// <param name="module">Module to search</param>
		/// <param name="logType">Log type to search</param>
		/// <param name="hostName">Machine where log entry were generated</param>
		/// <param name="service">WhizFlow service name who generated the log</param>
		/// <param name="domain">Domain who generated the log</param>
		/// <param name="connectionString">WhizFlow database connection string</param>
		/// <returns>The list of log entries</returns>
		public static List<List<Object>> GetLogsSerialize(Int32 numberOfEntries, Modules module, LogTypes logType, String hostName, String service, String domain, String connectionString)
		{
			List<List<Object>> result = new List<List<Object>>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Logs_ReadByLastProcessed";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = numberOfEntries;
				if (module == Modules.All)
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				}
				if (logType == LogTypes.All)
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				}
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					List<Object> t = new List<Object>();
					t.Add(Int32.Parse(dr["Id"].ToString()));
					t.Add(dr["HostName"].ToString());
					t.Add((String)dr["Service"]);
					t.Add((DateTime)dr["Time"]);
					t.Add(dr["AdditionalInformation"].ToString());
					if (dr["fk_TaskContentId"] != DBNull.Value)
					{
						t.Add(Int32.Parse(dr["fk_TaskContentId"].ToString()));
					}
					else
					{
						t.Add(null);
					}
					t.Add(dr["Message"].ToString());
					t.Add(dr["Object"].ToString());
					t.Add((String)dr["Domain"]);
					t.Add(Int32.Parse(dr["LogTypeId"].ToString()));
					t.Add(Int32.Parse(dr["ModuleId"].ToString()));
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Returns a list of log entries that meets the expected criteria
		/// </summary>
		/// <param name="from">Date from</param>
		/// <param name="to">Date to</param>
		/// <param name="module">Module to search</param>
		/// <param name="logType">Log type to search</param>
		/// <param name="hostName">Machine where log entry were generated</param>
		/// <param name="service">WhizFlow service name who generated the log</param>
		/// <param name="domain">WhizFlow Domain</param>
		/// <param name="connectionString">WhizFlow database connection string</param>
		/// <returns>The list of log entries ordered ascending</returns>
		public static List<LogEntry> GetLogs(DateTime from, DateTime to, Modules module, LogTypes logType, String hostName, String service, String domain, String connectionString)
		{
			List<LogEntry> result = new List<LogEntry>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Logs_Read";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = null;
				if (module == Modules.All)
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				}
				if (logType == LogTypes.All)
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				}
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = null;
				command.Parameters.Add("From", System.Data.SqlDbType.DateTime, -1).Value = from;
				command.Parameters.Add("To", System.Data.SqlDbType.DateTime, -1).Value = to;
				command.Parameters.Add("Ascending", System.Data.SqlDbType.Bit).Value = 0;
				command.Parameters.Add("Mode", System.Data.SqlDbType.Int).Value = 2;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					LogEntry t = new LogEntry();
					t.Id = Int32.Parse(dr["Id"].ToString());
					t.HostName = dr["HostName"].ToString();
					t.Domain = (String)dr["Domain"];
					t.Service = (String)dr["Service"];
					t.Time = (DateTime)dr["Time"];
					t.AdditionalInformation = dr["AdditionalInformation"].ToString();
					if (dr["fk_TaskContentId"] != DBNull.Value)
					{
						t.TaskContentId = Int32.Parse(dr["fk_TaskContentId"].ToString());
					}
					else
					{
						t.TaskContentId = null;
					}
					t.Message = dr["Message"].ToString();
					t.Object = dr["Object"].ToString();
					t.LogType = (LogTypes)Int32.Parse(dr["LogTypeId"].ToString());
					t.Module = (Modules)Int32.Parse(dr["ModuleId"].ToString());
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Returns a list of log entries that meets the expected criteria in a non specific serializable structure
		/// </summary>
		/// <param name="from">Date from</param>
		/// <param name="to">Date to</param>
		/// <param name="module">Module to search</param>
		/// <param name="logType">Log type to search</param>
		/// <param name="hostName">Machine where log entry were generated</param>
		/// <param name="service">WhizFlow service name who generated the log</param>
		/// <param name="domain">WhizFlow Domain</param>
		/// <param name="connectionString">WhizFlow database connection string</param>
		/// <returns>The list of log entries ordered ascending</returns>
		public static List<List<Object>> GetLogsSerialize(DateTime from, DateTime to, Modules module, LogTypes logType, String hostName, String service, String domain, String connectionString)
		{
			List<List<Object>> result = new List<List<Object>>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Logs_Read";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostName;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = null;
				if (module == Modules.All)
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				}
				if (logType == LogTypes.All)
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				}
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = null;
				command.Parameters.Add("From", System.Data.SqlDbType.DateTime, -1).Value = from;
				command.Parameters.Add("To", System.Data.SqlDbType.DateTime, -1).Value = to;
				command.Parameters.Add("Ascending", System.Data.SqlDbType.Bit).Value = 0;
				command.Parameters.Add("Mode", System.Data.SqlDbType.Int).Value = 2;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					List<Object> t = new List<Object>();
					t.Add(Int32.Parse(dr["Id"].ToString()));
					t.Add(dr["HostName"].ToString());
					t.Add((String)dr["Service"]);
					t.Add((DateTime)dr["Time"]);
					t.Add(dr["AdditionalInformation"].ToString());
					if (dr["fk_TaskContentId"] != DBNull.Value)
					{
						t.Add(Int32.Parse(dr["fk_TaskContentId"].ToString()));
					}
					else
					{
						t.Add(null);
					}
					t.Add(dr["Message"].ToString());
					t.Add(dr["Object"].ToString());
					t.Add((String)dr["Domain"]);
					t.Add(Int32.Parse(dr["LogTypeId"].ToString()));
					t.Add(Int32.Parse(dr["ModuleId"].ToString()));
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Returns a list of log entries that meets the expected criteria
		/// </summary>
		/// <param name="taskContentId">The task content id</param>
		/// <param name="module">Module to search</param>
		/// <param name="logType">Log type to search</param>
		/// <param name="hostname">Machine where log entry were generated</param>
		/// <param name="service">WhizFlow service name who generated the log</param>
		/// <param name="domain">WhizFlow Domain</param>
		/// <param name="connectionString">WhizFlow database connection string</param>
		/// <returns>The list of log entries ordered ascending</returns>
		public static List<LogEntry> GetLogs(Modules module, LogTypes logType, String hostname, String service, String domain, Int32 taskContentId, String connectionString)
		{
			List<LogEntry> result = new List<LogEntry>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Logs_Read";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = null;
				if (module == Modules.All)
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				}
				if (logType == LogTypes.All)
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				}
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				command.Parameters.Add("From", System.Data.SqlDbType.DateTime, -1).Value = null;
				command.Parameters.Add("To", System.Data.SqlDbType.DateTime, -1).Value = null;
				command.Parameters.Add("Ascending", System.Data.SqlDbType.Bit).Value = 0;
				command.Parameters.Add("Mode", System.Data.SqlDbType.Int).Value = 3;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					LogEntry t = new LogEntry();
					t.Id = Int32.Parse(dr["Id"].ToString());
					t.HostName = dr["HostName"].ToString();
					t.Domain = (String)dr["Domain"];
					t.Service = (String)dr["Service"];
					t.Time = (DateTime)dr["Time"];
					t.AdditionalInformation = dr["AdditionalInformation"].ToString();
					t.TaskContentId = Int32.Parse(dr["fk_TaskContentId"].ToString());
					t.Message = dr["Message"].ToString();
					t.Object = dr["Object"].ToString();
					t.LogType = (LogTypes)Int32.Parse(dr["LogTypeId"].ToString());
					t.Module = (Modules)Int32.Parse(dr["ModuleId"].ToString());
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Returns a list of log entries that meets the expected criteria in a non specific serializable structure
		/// </summary>
		/// <param name="taskContentId">The task content id</param>
		/// <param name="module">Module to search</param>
		/// <param name="logType">Log type to search</param>
		/// <param name="hostname">Machine where log entry were generated</param>
		/// <param name="service">WhizFlow service name who generated the log</param>
		/// <param name="domain">WhizFlow Domain</param>
		/// <param name="connectionString">WhizFlow database connection string</param>
		/// <returns>The list of log entries ordered ascending</returns>
		public static List<List<Object>> GetLogsSerialize(Modules module, LogTypes logType, String hostname, String service, String domain, Int32 taskContentId, String connectionString)
		{
			List<List<Object>> result = new List<List<Object>>();
			SqlCommand command;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				command = new SqlCommand();
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = "dbo.WF_Logs_Read";
				command.Parameters.Add("HostName", System.Data.SqlDbType.VarChar, 255).Value = hostname;
				command.Parameters.Add("Service", System.Data.SqlDbType.VarChar, 255).Value = service;
				command.Parameters.Add("Domain", System.Data.SqlDbType.VarChar, 255).Value = domain;
				command.Parameters.Add("NumberOfEntries", System.Data.SqlDbType.Int).Value = null;
				if (module == Modules.All)
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("ModuleId", System.Data.SqlDbType.Int).Value = module;
				}
				if (logType == LogTypes.All)
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = null;
				}
				else
				{
					command.Parameters.Add("LogTypeId", System.Data.SqlDbType.Int).Value = logType;
				}
				command.Parameters.Add("TaskContentId", System.Data.SqlDbType.Int).Value = taskContentId;
				command.Parameters.Add("From", System.Data.SqlDbType.DateTime, -1).Value = null;
				command.Parameters.Add("To", System.Data.SqlDbType.DateTime, -1).Value = null;
				command.Parameters.Add("Ascending", System.Data.SqlDbType.Bit).Value = 0;
				command.Parameters.Add("Mode", System.Data.SqlDbType.Int).Value = 3;
				command.Connection = connection;
				SqlDataReader dr = command.ExecuteReader();
				while (dr.Read())
				{
					List<Object> t = new List<Object>();
					t.Add(Int32.Parse(dr["Id"].ToString()));
					t.Add(dr["HostName"].ToString());
					t.Add((String)dr["Service"]);
					t.Add((DateTime)dr["Time"]);
					t.Add(dr["AdditionalInformation"].ToString());
					t.Add(Int32.Parse(dr["fk_TaskContentId"].ToString()));
					t.Add(dr["Message"].ToString());
					t.Add(dr["Object"].ToString());
					t.Add((String)dr["Domain"]);
					t.Add(Int32.Parse(dr["LogTypeId"].ToString()));
					t.Add(Int32.Parse(dr["ModuleId"].ToString()));
					result.Add(t);
				}
				dr.Close();
				connection.Close();
			}
			return result;
		}
	}
}
