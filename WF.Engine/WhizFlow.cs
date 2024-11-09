using Whiz.Framework.Configuration;
using Whiz.WhizFlow.Common.Objects;
using Whiz.WhizFlow.Engine.Modules;
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Logs;
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Tasks;
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Tasks.BE;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Whiz.WhizFlow.Engine
{
	/// <summary>
	/// The master class containing all the queues and schedules for an app domain
	/// </summary>
	public class WhizFlow : MarshalByRefObject
	{
		private String _serviceName;
		private String _domain;
		private String _connectionString;
		private delegate void HttpDelegate(GenericConfiguration configuration, HttpListenerRequest request, HttpListenerResponse response);
		private Dictionary<String, Tuple<MethodInfo, GenericConfiguration>> _httpCommands;
		private Dictionary<String, String> _configsEnv;
		private GenericConfiguration _domainConfiguration;
		private System.Diagnostics.PerformanceCounter _logs;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configuration">The configuration for this WhizFlow Domain</param>
		/// <param name="serviceName">The WhizFlow service name</param>
		/// <param name="domainName">The WhizFlow internal appdomain name</param>
		public WhizFlow(GenericConfiguration configuration, String serviceName, String domainName)
		{
			ReplaceEnvironment(configuration);
			_domainConfiguration = configuration;
			_logs = new System.Diagnostics.PerformanceCounter(Monitoring.PerformanceCounters.Utilities.FormatPerformanceCounterCategoryName(serviceName), Monitoring.PerformanceCounters.Utilities.LOGS_COUNTER_NAME, String.Format("{0}_Logs", domainName), false);
			Log.Init(configuration.Get("db").Value, _logs, serviceName, domainName);
			Log.WriteLogAsync(Log.Module.AppDomainManager, Log.LogTypes.Information, "WhizFlowManager", "WhizFlow " + serviceName + " init", "", configuration.Get("db").Value);
			Queues = new QueuesHandler(serviceName, domainName, configuration);
			Schedulers = new SchedulersHandler(serviceName, domainName, configuration);
			Workers = new WorkersHandler(serviceName, domainName, configuration);
			_serviceName = serviceName;
			_domain = domainName;
			_connectionString = configuration.Get("db").Value;
			CreateHttpCommandDelegate(configuration);
		}
		/// <summary>
		/// Starts this WhizFlow domain
		/// </summary>
		public void Start()
		{
			Queues.InstrumentationStartHandler();
			Schedulers.InstrumentationStartHandler();
			Workers.InstrumentationStartHandler();
		}
		/// <summary>
		/// Stops this WhizFlow domain
		/// </summary>
		public void Stop()
		{
			Queues.InstrumentationStopHandler();
			Schedulers.InstrumentationStopHandler();
			Workers.InstrumentationStopHandler();
		}
		private void CreateHttpCommandDelegate(GenericConfiguration configuration)
		{
			_httpCommands = new Dictionary<String, Tuple<MethodInfo, GenericConfiguration>>();
			foreach (GenericConfiguration g in configuration.Get("commands").GetList("command"))
			{
				Assembly asm = Assembly.LoadFrom(g["assembly"][0].Value);
				_httpCommands.Add(g["name"][0].Value, new Tuple<MethodInfo, GenericConfiguration>(asm.GetType(g["class"][0].Value).GetMethod(g["method"][0].Value, BindingFlags.Public | BindingFlags.Static), g["configuration"][0]));
			}
		}
		private void ReplaceEnvironment(GenericConfiguration configuration)
		{
			_configsEnv = new Dictionary<String, String>();
			foreach (var c in configuration.Get("environment").Configs)
			{
				if ((String.IsNullOrEmpty(configuration.Get("environment/" + c.Key + "/" + Environment.MachineName).Value)) && (String.IsNullOrEmpty(configuration.Get("environment/" + c.Key + "/" + "machinename_" + Environment.MachineName).Value)))
				{
					_configsEnv.Add("$[" + c.Key + "]$", configuration.Get("environment/" + c.Key + "/default").Value);
				}
				else if (!String.IsNullOrEmpty(configuration.Get("environment/" + c.Key + "/" + "machinename_" + Environment.MachineName).Value))
				{
					_configsEnv.Add("$[" + c.Key + "]$", configuration.Get("environment/" + c.Key + "/" + "machinename_" + Environment.MachineName).Value);
				}
				else
				{
					_configsEnv.Add("$[" + c.Key + "]$", configuration.Get("environment/" + c.Key + "/" + Environment.MachineName).Value);
				}
			}
			NavigateConfiguration(configuration);
		}
		private void ReplaceValue(GenericConfiguration configuration)
		{
			foreach (var e in _configsEnv)
			{
				if ((configuration.Value != null) && (configuration.Value.Contains(e.Key)))
				{
					configuration.Value = configuration.Value.Replace(e.Key, e.Value);
				}
			}
		}
		private void NavigateConfiguration(GenericConfiguration configuration)
		{
			if (configuration.Configs.Count() > 0)
			{
				foreach (var c in configuration.Configs)
				{
					foreach (var d in c.Value)
					{
						ReplaceValue(d);
						NavigateConfiguration(d);
					}
				}
			}
		}
		/// <summary>
		/// Execute a command arrived via the WhizFlow http interface
		/// </summary>
		/// <param name="command"></param>
		/// <param name="queryString"></param>
		/// <param name="payload"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public String HttpCommand(String command, NameValueCollection queryString, String payload, String method)
		{
			try
			{
				Object[] parameters = new Object[5];
				parameters[0] = _domainConfiguration;
				parameters[1] = _httpCommands[command].Item2;
				parameters[2] = queryString;
				parameters[3] = payload;
				parameters[4] = method;
				return (String)_httpCommands[command].Item1.Invoke(null, parameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal String SchedulersManagement(HttpInterfaceCommand command, NameValueCollection parameters)
		{
			try
			{
				switch (command)
				{
					case HttpInterfaceCommand.ActivateDomainScheduler:
						Schedulers.GetSchedulerHandlerThread(parameters["scheduler"]).Start();
						break;
					case HttpInterfaceCommand.DeactivateDomainScheduler:
						Schedulers.GetSchedulerHandlerThread(parameters["scheduler"]).Stop();
						break;
					case HttpInterfaceCommand.ChangeDomainSchedulerRecurrence:
						Schedulers.GetSchedulerHandlerThread(parameters["scheduler"]).ChangeRecurrence(Double.Parse(parameters["ms"]));
						break;
					case HttpInterfaceCommand.RestoreDomainSchedulerRecurrence:
						Schedulers.GetSchedulerHandlerThread(parameters["scheduler"]).RestoreRecurrence();
						break;
				}
				return "Ok";
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}
		/// <summary>
		/// Returns logs for this domain in a binary serialized fashion
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns>Base64 string of the binaryformatted logs</returns>
		internal String GetLogs1(NameValueCollection parameters)
		{
			try
			{
				Int32 numOfLogs = Int32.Parse(parameters["entries"]);
				Monitoring.Utilities.Logs.Modules module = (Monitoring.Utilities.Logs.Modules)Int32.Parse(parameters["module"]);
				Monitoring.Utilities.Logs.LogTypes types = (Monitoring.Utilities.Logs.LogTypes)Int32.Parse(parameters["type"]);
				String host = String.IsNullOrEmpty(parameters["allhosts"]) ? Environment.MachineName : null;
				String service = String.IsNullOrEmpty(parameters["allservices"]) ? _serviceName : null;
				String domain = String.IsNullOrEmpty(parameters["alldomains"]) ? _domain : null;
				List<List<Object>> logs = Whiz.WhizFlow.Engine.Monitoring.Utilities.Logs.BE.Logs.GetLogsSerialize(numOfLogs, module, types, host, service, domain, _connectionString);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, logs);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		internal String GetLogs2(NameValueCollection parameters)
		{
			try
			{
				//GetLogs(DateTime from, DateTime to, Module module, LogTypes logType, String hostName, String service, String domain, String connectionString)
				DateTime from = DateTime.Parse(parameters["from"]);
				DateTime to = DateTime.Parse(parameters["to"]);
				Monitoring.Utilities.Logs.Modules module = (Monitoring.Utilities.Logs.Modules)Int32.Parse(parameters["module"]);
				Monitoring.Utilities.Logs.LogTypes types = (Monitoring.Utilities.Logs.LogTypes)Int32.Parse(parameters["type"]);
				String host = String.IsNullOrEmpty(parameters["allhosts"]) ? Environment.MachineName : null;
				String service = String.IsNullOrEmpty(parameters["allservices"]) ? _serviceName : null;
				String domain = String.IsNullOrEmpty(parameters["alldomains"]) ? _domain : null;
				List<List<Object>> logs = Whiz.WhizFlow.Engine.Monitoring.Utilities.Logs.BE.Logs.GetLogsSerialize(from, to, module, types, host, service, domain, _connectionString);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, logs);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		internal String GetLogs3(NameValueCollection parameters)
		{
			try
			{
				//GetLogs(Module module, LogTypes logType, String hostname, String service, String domain, Int32 taskContentId, String connectionString)
				Monitoring.Utilities.Logs.Modules module = (Monitoring.Utilities.Logs.Modules)Int32.Parse(parameters["module"]);
				Monitoring.Utilities.Logs.LogTypes types = (Monitoring.Utilities.Logs.LogTypes)Int32.Parse(parameters["type"]);
				Int32 taskContentId = Int32.Parse(parameters["taskcontentid"]);
				String host = String.IsNullOrEmpty(parameters["allhosts"]) ? Environment.MachineName : null;
				String service = String.IsNullOrEmpty(parameters["allservices"]) ? _serviceName : null;
				String domain = String.IsNullOrEmpty(parameters["alldomains"]) ? _domain : null;
				List<List<Object>> logs = Whiz.WhizFlow.Engine.Monitoring.Utilities.Logs.BE.Logs.GetLogsSerialize(module, types, host, service, domain, taskContentId, _connectionString);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, logs);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		internal String GetTaskContent(NameValueCollection parameters)
		{
			try
			{
				WhizFlowTaskContent ptc = Monitoring.Utilities.Tasks.BE.Tasks.GetTaskContent(Int32.Parse(parameters["taskcontentid"]), _connectionString);
				List<Object> l = new List<Object>();
				l.Add(ptc.Id);
				l.Add(ptc.TimeStamp);
				l.Add(ptc.Content);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, l);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		internal String GetTaskInformations1(NameValueCollection parameters)
		{
			try
			{
				List<List<Object>> l = Tasks.GetTaskInformationSerialize(Int32.Parse(parameters["taskcontentid"]), _connectionString);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, l);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		internal String GetTaskInformations2(NameValueCollection parameters)
		{
			try
			{
				String host = String.IsNullOrEmpty(parameters["allhosts"]) ? Environment.MachineName : null;
				String service = String.IsNullOrEmpty(parameters["allservices"]) ? _serviceName : null;
				String domain = String.IsNullOrEmpty(parameters["alldomains"]) ? _domain : null;
				Int32 entries = Int32.Parse(parameters["entries"]);
				List<List<Object>> l = Tasks.GetProcessedSerialize(entries, host, service, domain, _connectionString);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, l);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		internal String GetTaskInformations3(NameValueCollection parameters)
		{
			try
			{
				String host = String.IsNullOrEmpty(parameters["allhosts"]) ? Environment.MachineName : null;
				String service = String.IsNullOrEmpty(parameters["allservices"]) ? _serviceName : null;
				String domain = String.IsNullOrEmpty(parameters["alldomains"]) ? _domain : null;
				String queue = parameters["queue"];
				Int32 entries = Int32.Parse(parameters["entries"]);
				List<List<Object>> l = Tasks.GetQueueProcessedSerialize(entries, queue, host, service, domain, _connectionString);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, l);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		internal String GetTaskInformations4(NameValueCollection parameters)
		{
			try
			{
				DateTime from = DateTime.Parse(parameters["from"]);
				DateTime to = DateTime.Parse(parameters["to"]);
				String host = String.IsNullOrEmpty(parameters["allhosts"]) ? Environment.MachineName : null;
				String service = String.IsNullOrEmpty(parameters["allservices"]) ? _serviceName : null;
				String domain = String.IsNullOrEmpty(parameters["alldomains"]) ? _domain : null;
				List<List<Object>> l = Tasks.GetProcessedSerialize(from, to, host, service, domain, _connectionString);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, l);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		internal String GetTaskInformations5(NameValueCollection parameters)
		{
			try
			{
				DateTime from = DateTime.Parse(parameters["from"]);
				DateTime to = DateTime.Parse(parameters["to"]);
				String host = String.IsNullOrEmpty(parameters["allhosts"]) ? Environment.MachineName : null;
				String service = String.IsNullOrEmpty(parameters["allservices"]) ? _serviceName : null;
				String domain = String.IsNullOrEmpty(parameters["alldomains"]) ? _domain : null;
				String queue = parameters["queue"];
				List<List<Object>> l = Tasks.GetQueueProcessedSerialize(from, to, queue, host, service, domain, _connectionString);
				System.IO.Stream s = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(s, l);
				s.Seek(0, 0);
				return System.Convert.ToBase64String(((MemoryStream)s).ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		/// <summary>
		/// This domain QueuesHandler
		/// </summary>
		public QueuesHandler Queues { get; set; }
		/// <summary>
		/// This domain SchedulersHandler
		/// </summary>
		public SchedulersHandler Schedulers { get; set; }
		/// <summary>
		/// This domain WorkersHandler
		/// </summary>
		public WorkersHandler Workers { get; set; }
		/// <summary>
		/// Override in order to have infinite lifetime of the object
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			return null;
		}
		/// <summary>
		/// Dispose correctly the WhizFlow Domain
		/// </summary>
		public void Dispose()
		{
			Queues.Dispose();
			Schedulers.Dispose();
			Workers.Dispose();
			_logs.RemoveInstance();
		}
	}

}
