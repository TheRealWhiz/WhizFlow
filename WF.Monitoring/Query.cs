using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Whiz.WhizFlow.Common.Objects;
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Logs;
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Tasks;
using Whiz.WhizFlow.Engine.Monitoring.Utilities.WMI;

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// Static helper class for a comfortable use of the WhizFlow http endpoint
	/// </summary>
	public static class Query
	{
		private static System.Globalization.CultureInfo _numberCultureInfo = null;
		/// <summary>
		/// WhizFlow configuration class for dynamic domains
		/// </summary>
		public class WhizFlowConfiguration
		{
			// This class is the same contained in the DAL (WhizFlowConfiguration.cs)
			/// <summary>
			/// Configuration id
			/// </summary>
			public Int32 Id { get; set; }
			/// <summary>
			/// Hostname of the configuration
			/// </summary>
			public String Hostname { get; set; }
			/// <summary>
			/// Service name of the configuration
			/// </summary>
			public String Service { get; set; }
			/// <summary>
			/// Domain name
			/// </summary>
			public String Domain { get; set; }
			/// <summary>
			/// Configuration xml
			/// </summary>
			public String Configuration { get; set; }
			/// <summary>
			/// Active indicator (auto start at service startup)
			/// </summary>
			public Boolean Active { get; set; }
		}
		/// <summary>
		/// WhizFlow queues handler representation
		/// </summary>
		public class QueuesHandler
		{
			/// <summary>
			/// WhizFlow name (Service)
			/// </summary>
			public String WhizFlow { get; set; }
			/// <summary>
			/// Start time of the instance
			/// </summary>
			public DateTime RunningSince { get; set; }
			/// <summary>
			/// Queues handled by this instance
			/// </summary>
			public Int32 NumberOfQueues { get; set; }
			/// <summary>
			/// Domain of the instance
			/// </summary>
			public String Domain { get; set; }
			/// <summary>
			/// Last reset of this instance
			/// </summary>
			public String LastReset { get; set; }
		}
		/// <summary>
		/// Queue representation
		/// </summary>
		public class QueueHandlerThread
		{
			/// <summary>
			/// WhizFlow name (Service)
			/// </summary>
			public String WhizFlow { get; set; }
			/// <summary>
			/// Domain of the instance
			/// </summary>
			public String Domain { get; set; }
			/// <summary>
			/// Queue name
			/// </summary>
			public String Queue { get; set; }
			/// <summary>
			/// Queue working mode
			/// </summary>
			public String Mode { get; set; }
			/// <summary>
			/// Queue status
			/// </summary>
			public String Status { get; set; }
			/// <summary>
			/// Items in the queue
			/// </summary>
			public Int32 ItemsInQueue { get; set; }
			/// <summary>
			/// Last queue thread run
			/// </summary>
			public String LastRunning { get; set; }
		}
		/// <summary>
		/// Schedulers handler representation
		/// </summary>
		public class SchedulersHandler
		{
			/// <summary>
			/// WhizFlow name (Service)
			/// </summary>
			public String WhizFlow { get; set; }
			/// <summary>
			/// Domain of the instance
			/// </summary>
			public String Domain { get; set; }
			/// <summary>
			/// Start time of the instance
			/// </summary>
			public DateTime RunningSince { get; set; }
			/// <summary>
			/// Number of schedulers in this instance
			/// </summary>
			public Int32 NumberOfSchedulers { get; set; }
			/// <summary>
			/// Instance last reset
			/// </summary>
			public String LastReset { get; set; }
		}
		/// <summary>
		/// Scheduler representation
		/// </summary>
		public class SchedulerHandlerThread
		{
			/// <summary>
			/// WhizFlow name (Service)
			/// </summary>
			public String WhizFlow { get; set; }
			/// <summary>
			/// Domain of the instance
			/// </summary>
			public String Domain { get; set; }
			/// <summary>
			/// Scheduler name
			/// </summary>
			public String Scheduler { get; set; }
			/// <summary>
			/// Scheduler mode
			/// </summary>
			public String Mode { get; set; }
			/// <summary>
			/// Scheduler status
			/// </summary>
			public String Status { get; set; }
			/// <summary>
			/// Scheduler last execution
			/// </summary>
			public String LastRunning { get; set; }
		}
		/// <summary>
		/// Worker representation
		/// </summary>
		public class WorkerHandlerThread
		{
			/// <summary>
			/// Scheduler name
			/// </summary>
			public String WorkerName { get; set; }
			/// <summary>
			/// WhizFlow service instance name
			/// </summary>
			public String WhizFlow { get; set; }
			/// <summary>
			/// WhizFlow internal domain
			/// </summary>
			public String Domain { get; set; }
			/// <summary>
			/// Status
			/// </summary>
			public String Status { get; set; }
		}
		/// <summary>
		/// Gets the number of processed tasks until now for the given WhizFlow, domain and queue
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain"></param>
		/// <param name="queue"></param>
		/// <returns></returns>
		public static float QueueProcessedTasksGet(String whizFlow, String domain, String queue)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/pc/t?domain=" + domain + "&queue=" + queue, false));
			return float.Parse(x.Element("whizFlow").Element("pc").Element("t").Value);
		}
		/// <summary>
		/// Gets the number of processed tasks in the last second for the given whizFlow, domain and queue
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain"></param>
		/// <param name="queue"></param>
		/// <returns></returns>
		public static float QueueProcessedTasksPerSecondGet(String whizFlow, String domain, String queue)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/pc/tps?domain=" + domain + "&queue=" + queue, false));
			if (_numberCultureInfo == null)
			{
				if (x.Element("whizFlow").Element("pc").Element("tps").Value.Contains(".") && System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != ".")
				{
					_numberCultureInfo = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
					_numberCultureInfo.NumberFormat.NumberDecimalSeparator = ".";
				}
				else if (x.Element("whizFlow").Element("pc").Element("tps").Value.Contains(",") && System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != ",")
				{
					_numberCultureInfo = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
					_numberCultureInfo.NumberFormat.NumberDecimalSeparator = ",";
				}
			}
			return float.Parse(x.Element("whizFlow").Element("pc").Element("tps").Value, _numberCultureInfo);
		}
		/// <summary>
		/// Gets the number of logs still unwritten in the logger for the specified domain
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain"></param>
		/// <returns></returns>
		public static float DomainUnwrittenLogsGetCount(String whizFlow, String domain)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/pc/logs?domain=" + domain, false));
			return float.Parse(x.Element("whizFlow").Element("pc").Element("logs").Value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns></returns>
		public static List<QueuesHandler> QueuesHandlersGet(String whizFlow)
		{
			List<QueuesHandler> result = new List<QueuesHandler>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobjects?entity=" + ((Int32)Entities.QueuesHandler).ToString(), false));
			foreach (XElement xe in x.XPathSelectElements("//object"))
			{
				QueuesHandler q = new QueuesHandler();
				q.Domain = xe.Element("WhizFlowDomain").Value;
				q.LastReset = xe.Element("LastReset").Value;
				q.NumberOfQueues = Int32.Parse(xe.Element("NumberOfQHT").Value);
				q.WhizFlow = xe.Element("WhizFlow").Value;
				q.RunningSince = DateTime.Parse(xe.Element("RunningSince").Value);
				result.Add(q);
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static QueuesHandler QueuesHandlerGet(String whizFlow, String name)
		{
			QueuesHandler result = new QueuesHandler();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobject?entity=" + ((Int32)Entities.QueuesHandler).ToString() + "&name=" + name, false));
			XElement xe = x.XPathSelectElement("//object");
			result.Domain = xe.Element("WhizFlowDomain").Value;
			result.LastReset = xe.Element("LastReset").Value;
			result.NumberOfQueues = Int32.Parse(xe.Element("NumberOfQHT").Value);
			result.WhizFlow = xe.Element("WhizFlow").Value;
			result.RunningSince = DateTime.Parse(xe.Element("RunningSince").Value);
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns></returns>
		public static List<QueueHandlerThread> QueueHandlerThreadsGet(String whizFlow)
		{
			List<QueueHandlerThread> result = new List<QueueHandlerThread>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobjects?entity=" + ((Int32)Entities.QueueHandlerThread).ToString(), false));
			foreach (XElement xe in x.XPathSelectElements("//object"))
			{
				QueueHandlerThread q = new QueueHandlerThread();
				q.Domain = xe.Element("WhizFlowDomain").Value;
				q.Status = xe.Element("Status").Value;
				q.ItemsInQueue = Int32.Parse(xe.Element("ItemsInQueue").Value);
				q.WhizFlow = xe.Element("WhizFlow").Value;
				q.Queue = xe.Element("Queue").Value;
				q.LastRunning = xe.Element("LastRunning").Value;
				q.Mode = xe.Element("Mode").Value;
				result.Add(q);
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static QueueHandlerThread QueueHandlerThreadGet(String whizFlow, String name)
		{
			QueueHandlerThread result = new QueueHandlerThread();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobject?entity=" + ((Int32)Entities.QueueHandlerThread).ToString() + "&name=" + name, false));
			XElement xe = x.XPathSelectElement("//object");
			result.Domain = xe.Element("WhizFlowDomain").Value;
			result.Status = xe.Element("Status").Value;
			result.ItemsInQueue = Int32.Parse(xe.Element("ItemsInQueue").Value);
			result.WhizFlow = xe.Element("WhizFlow").Value;
			result.Queue = xe.Element("Queue").Value;
			result.LastRunning = xe.Element("LastRunning").Value;
			result.Mode = xe.Element("Mode").Value;
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns></returns>
		public static List<SchedulersHandler> SchedulersHandlersGet(String whizFlow)
		{
			List<SchedulersHandler> result = new List<SchedulersHandler>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobjects?entity=" + ((Int32)Entities.SchedulersHandler).ToString(), false));
			foreach (XElement xe in x.XPathSelectElements("//object"))
			{
				SchedulersHandler q = new SchedulersHandler();
				q.Domain = xe.Element("WhizFlowDomain").Value;
				q.LastReset = xe.Element("LastReset").Value;
				q.NumberOfSchedulers = Int32.Parse(xe.Element("NumberOfSHT").Value);
				q.WhizFlow = xe.Element("WhizFlow").Value;
				q.RunningSince = DateTime.Parse(xe.Element("RunningSince").Value);
				result.Add(q);
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static SchedulersHandler SchedulersHandlerGet(String whizFlow, String name)
		{
			SchedulersHandler result = new SchedulersHandler();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobject?entity=" + ((Int32)Entities.SchedulersHandler).ToString() + "&name=" + name, false));
			XElement xe = x.XPathSelectElement("//object");
			result.Domain = xe.Element("WhizFlowDomain").Value;
			result.LastReset = xe.Element("LastReset").Value;
			result.NumberOfSchedulers = Int32.Parse(xe.Element("NumberOfSHT").Value);
			result.WhizFlow = xe.Element("WhizFlow").Value;
			result.RunningSince = DateTime.Parse(xe.Element("RunningSince").Value);
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns></returns>
		public static List<SchedulerHandlerThread> SchedulerHandlerThreadsGet(String whizFlow)
		{
			List<SchedulerHandlerThread> result = new List<SchedulerHandlerThread>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobjects?entity=" + ((Int32)Entities.SchedulerHandlerThread).ToString(), false));
			foreach (XElement xe in x.XPathSelectElements("//object"))
			{
				SchedulerHandlerThread q = new SchedulerHandlerThread();
				q.Domain = xe.Element("WhizFlowDomain").Value;
				q.Status = xe.Element("Status").Value;
				q.WhizFlow = xe.Element("WhizFlow").Value;
				q.Scheduler = xe.Element("SchedulerName").Value;
				q.LastRunning = xe.Element("LastRunning").Value;
				q.Mode = xe.Element("Mode").Value;
				result.Add(q);
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static SchedulerHandlerThread SchedulerHandlerThreadGet(String whizFlow, String name)
		{
			SchedulerHandlerThread result = new SchedulerHandlerThread();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobject?entity=" + ((Int32)Entities.SchedulerHandlerThread).ToString() + "&name=" + name, false));
			XElement xe = x.XPathSelectElement("//object");
			result.Domain = xe.Element("WhizFlowDomain").Value;
			result.Status = xe.Element("Status").Value;
			result.WhizFlow = xe.Element("WhizFlow").Value;
			result.Scheduler = xe.Element("SchedulerName").Value;
			result.LastRunning = xe.Element("LastRunning").Value;
			result.Mode = xe.Element("Mode").Value;
			return result;
		}
		/// <summary>
		/// Retrieves the list of all the workers running in a WhizFLow
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns></returns>
		public static List<WorkerHandlerThread> WorkerHandlerThreadsGet(String whizFlow)
		{
			List<WorkerHandlerThread> result = new List<WorkerHandlerThread>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/wmi/getobjects?entity=" + ((Int32)Entities.WorkerHandlerThread).ToString(), false));
			foreach (XElement xe in x.XPathSelectElements("//object"))
			{
				WorkerHandlerThread q = new WorkerHandlerThread();
				q.WorkerName = xe.Element("WorkerName").Value;
				q.Domain = xe.Element("WhizFlowDomain").Value;
				q.Status = xe.Element("Status").Value;
				q.WhizFlow = xe.Element("WhizFlow").Value;
				q.Status = xe.Element("Status").Value;
				result.Add(q);
			}
			return result;
		}
		/// <summary>
		/// Retrieves the list of domains running
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns>All the domain names</returns>
		public static List<String> DomainsGet(String whizFlow)
		{
			List<String> result = new List<String>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domains/list", false));
			foreach (XElement xe in x.XPathSelectElements("//domain"))
			{
				result.Add(xe.Value);
			}
			return result;
		}
		/// <summary>
		/// Starts all the domains of the specified host (static domains and active dynamic domains)
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns>Operation result</returns>
		public static Boolean DomainsStart(String whizFlow)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domains/start", false));
			if (x.XPathSelectElements("//domain").Count() > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// Stops all the domains running in the specified host. Exception will be thrown when the service throws exception.
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns>Always true</returns>
		public static Boolean DomainsStop(String whizFlow)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domains/stop", false));
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain"></param>
		/// <returns></returns>
		public static Boolean DomainStart(String whizFlow, String domain)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domain/start?domain=" + domain, false));
			return true;
		}
		/// <summary>
		/// Stops the specified domain
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain to stop</param>
		/// <returns>True</returns>
		public static Boolean DomainStop(String whizFlow, String domain)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domain/stop?domain=" + domain, false));
			return true;
		}
		/// <summary>
		/// Adds a dynamic domain to the instance saving the configuration as active and starting it
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="configuration">Domain xml configuration</param>
		/// <returns>True</returns>
		public static Boolean DomainAdd(String whizFlow, String configuration)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domain/add", "POST", configuration, maskException: false));
			return true;
		}
		/// <summary>
		/// Activate a dynamic domain configuration for auto-startup
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain name</param>
		/// <returns>True</returns>
		public static Boolean DomainConfigurationActivate(String whizFlow, String domain)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domainconfiguration/activate?domain=" + domain, false));
			return true;
		}
		/// <summary>
		/// Deactivate a dynamic domain configuration for auto-startup
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain name</param>
		/// <returns>True</returns>
		public static Boolean DomainConfigurationDeactivate(String whizFlow, String domain)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domainconfiguration/deactivate?domain=" + domain, false));
			return true;
		}
		/// <summary>
		/// Retrieves a dynamic domain configuration
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain name</param>
		/// <returns>A WhizFlowConfiguration object with all the informations of the dynamic domain</returns>
		public static WhizFlowConfiguration DomainConfigurationGet(String whizFlow, String domain)
		{
			String config = HttpUtilities.HttpCall(whizFlow + "/domainconfiguration/get?domain=" + domain, false);
			XDocument x = XDocument.Parse(config);
			XElement el = x.XPathSelectElement("//commandResponse/configurations/configuration");
			String list = HttpUtilities.HttpCall(whizFlow + "/domainconfiguration/list", false);
			WhizFlowConfiguration c = new WhizFlowConfiguration();
			c.Configuration = el.Element("xml").Element("domain").ToString();
			c.Active = Boolean.Parse(el.Element("active").Value);
			c.Id = Int32.Parse(el.Element("id").Value);
			c.Service = el.Element("service").Value;
			c.Hostname = el.Element("host").Value;
			c.Domain = el.Element("domain").Value;
			return c;
		}
		/// <summary>
		/// Retrieves all the dynamic domain configurations on the specified host database
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <returns>List of WhizFlowConfiguration object with all the informations</returns>
		public static List<WhizFlowConfiguration> DomainConfigurationsGetList(String whizFlow)
		{
			List<WhizFlowConfiguration> result = new List<WhizFlowConfiguration>();
			String list = HttpUtilities.HttpCall(whizFlow + "/domainconfiguration/list", false);
			XDocument x = XDocument.Parse(list);
			foreach (XElement el in x.XPathSelectElements("//commandResponse/configurations/configuration"))
			{
				WhizFlowConfiguration c = new WhizFlowConfiguration();
				c.Configuration = el.Element("xml").Element("domain").ToString();
				c.Active = Boolean.Parse(el.Element("active").Value);
				c.Id = Int32.Parse(el.Element("id").Value);
				c.Service = el.Element("service").Value;
				c.Hostname = el.Element("host").Value;
				c.Domain = el.Element("domain").Value;
				result.Add(c);
			}
			return result;
		}
		/// <summary>
		/// Delete a dynamic domain configuration
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain name</param>
		/// <returns>True</returns>
		public static Boolean DomainConfigurationDelete(String whizFlow, String domain)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domainconfiguration/delete?domain=" + domain, false));
			return true;
		}
		/// <summary>
		/// Add or update a dynamic domain configuration
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="configuration">Domain xml configuration</param>
		/// <returns>True</returns>
		public static Boolean DomainConfigurationSave(String whizFlow, String configuration)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domainconfiguration/save", "POST", configuration, maskException: false));
			return true;
		}
		/// <summary>
		/// Activate a scheduler in a domain
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain name</param>
		/// <param name="scheduler">Scheduler name</param>
		/// <returns>True</returns>
		public static Boolean DomainSchedulerActivate(String whizFlow, String domain, String scheduler)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domainscheduler/activate?domain=" + domain + "&scheduler=" + scheduler, false));
			return true;
		}
		/// <summary>
		/// Deactivate a scheduler in a domain
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain name</param>
		/// <param name="scheduler">Scheduler name</param>
		/// <returns></returns>
		public static Boolean DomainSchedulerDeactivate(String whizFlow, String domain, String scheduler)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domainscheduler/deactivate?domain=" + domain + "&scheduler=" + scheduler, false));
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain name</param>
		/// <param name="scheduler">Scheduler name</param>
		/// <param name="milliseconds"></param>
		/// <returns></returns>
		public static Boolean DomainSchedulerChangeRecurrence(String whizFlow, String domain, String scheduler, Int32 milliseconds)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domainscheduler/changerecurrence?domain=" + domain + "&scheduler=" + scheduler + "&ms=" + milliseconds.ToString(), false));
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint in the form http://{address}:{port}</param>
		/// <param name="domain">Domain name</param>
		/// <param name="scheduler">Scheduler name</param>
		/// <returns></returns>
		public static Boolean DomainSchedulerRestoreRecurrence(String whizFlow, String domain, String scheduler)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/domainscheduler/restorerecurrence?domain=" + domain + "&scheduler=" + scheduler, false));
			return true;
		}
		public static List<LogEntry> LogsGet1(String whizFlow, String domain, Int32 entries, Modules module, LogTypes type, Boolean allHosts, Boolean allServices, Boolean allDomains)
		{
			List<LogEntry> result = new List<LogEntry>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/logs/get1?domain=" + domain + "&entries=" + entries.ToString() + "&module=" + ((Int32)module).ToString() + "&type=" + ((Int32)type).ToString() + (allHosts ? "&allhosts=1" : "") + (allServices ? "&allservices=1" : "") + (allDomains ? "&alldomains=1" : ""), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<List<Object>> tempresult = (List<List<Object>>)binaryFormatter.Deserialize(ms);
			foreach (var t in tempresult)
			{
				result.Add(LogEntry.FromSerializedObject(t));
			}
			return result;
		}
		public static List<LogEntry> LogsGet2(String whizFlow, String domain, DateTime from, DateTime to, Modules module, LogTypes type, Boolean allHosts, Boolean allServices, Boolean allDomains)
		{
			List<LogEntry> result = new List<LogEntry>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/logs/get2?domain=" + domain + "&from=" + from.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + "&to=" + to.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + "&module=" + ((Int32)module).ToString() + "&type=" + ((Int32)type).ToString() + (allHosts ? "&allhosts=1" : "") + (allServices ? "&allservices=1" : "") + (allDomains ? "&alldomains=1" : ""), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<List<Object>> tempresult = (List<List<Object>>)binaryFormatter.Deserialize(ms);
			foreach (var t in tempresult)
			{
				result.Add(LogEntry.FromSerializedObject(t));
			}
			return result;
		}
		public static List<LogEntry> LogsGet3(String whizFlow, String domain, Int32 taskContentId, Modules module, LogTypes type, Boolean allHosts, Boolean allServices, Boolean allDomains)
		{
			List<LogEntry> result = new List<LogEntry>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/logs/get3?domain=" + domain + "&taskcontentid=" + taskContentId.ToString() + "&module=" + ((Int32)module).ToString() + "&type=" + ((Int32)type).ToString() + (allHosts ? "&allhosts=1" : "") + (allServices ? "&allservices=1" : "") + (allDomains ? "&alldomains=1" : ""), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<List<Object>> tempresult = (List<List<Object>>)binaryFormatter.Deserialize(ms);
			foreach (var t in tempresult)
			{
				result.Add(LogEntry.FromSerializedObject(t));
			}
			return result;
		}
		public static WhizFlowTaskContent TaskContentGet(String whizFlow, String domain, Int32 taskContentId)
		{
			WhizFlowTaskContent result = new WhizFlowTaskContent();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/taskcontent/get?domain=" + domain + "&taskcontentid=" + taskContentId.ToString(), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<Object> tempresult = (List<Object>)binaryFormatter.Deserialize(ms);
			result.Id = (Int32)tempresult[0];
			result.TimeStamp = (DateTime)tempresult[1];
			result.Content = (Object)tempresult[2];
			return result;
		}
		public static List<TaskInformation> TaskInformationsGet(String whizFlow, String domain, Int32 taskContentId)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/taskinformations/getbytaskcontentid?domain=" + domain + "&taskcontentid=" + taskContentId.ToString(), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<List<Object>> tempresult = (List<List<Object>>)binaryFormatter.Deserialize(ms);
			foreach (var t in tempresult)
			{
				result.Add(TaskInformation.FromSerializedObject(t));
			}
			return result;
		}
		public static List<TaskInformation> TaskInformationsGetProcessed(String whizFlow, String domain, Int32 entries, Boolean allHosts, Boolean allServices, Boolean allDomains)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/taskinformations/getprocessed?domain=" + domain + "&entries=" + entries.ToString() + (allHosts ? "&allhosts=1" : "") + (allServices ? "&allservices=1" : "") + (allDomains ? "&alldomains=1" : ""), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<List<Object>> tempresult = (List<List<Object>>)binaryFormatter.Deserialize(ms);
			foreach (var t in tempresult)
			{
				result.Add(TaskInformation.FromSerializedObject(t));
			}
			return result;
		}
		public static List<TaskInformation> TaskInformationsGetProcessed(String whizFlow, String domain, String queue, Int32 entries, Boolean allHosts, Boolean allServices, Boolean allDomains)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/taskinformations/getqueueprocessed?queue=" + queue + "&domain=" + domain + "&entries=" + entries.ToString() + (allHosts ? "&allhosts=1" : "") + (allServices ? "&allservices=1" : "") + (allDomains ? "&alldomains=1" : ""), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<List<Object>> tempresult = (List<List<Object>>)binaryFormatter.Deserialize(ms);
			foreach (var t in tempresult)
			{
				result.Add(TaskInformation.FromSerializedObject(t));
			}
			return result;
		}
		public static List<TaskInformation> TaskInformationsGetProcessed(String whizFlow, String domain, DateTime from, DateTime to, Boolean allHosts, Boolean allServices, Boolean allDomains)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/taskinformations/getprocessedtimeframe?domain=" + domain + "&from=" + from.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + "&to=" + to.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + (allHosts ? "&allhosts=1" : "") + (allServices ? "&allservices=1" : "") + (allDomains ? "&alldomains=1" : ""), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<List<Object>> tempresult = (List<List<Object>>)binaryFormatter.Deserialize(ms);
			foreach (var t in tempresult)
			{
				result.Add(TaskInformation.FromSerializedObject(t));
			}
			return result;
		}
		public static List<TaskInformation> TaskInformationsGetProcessed(String whizFlow, String domain, String queue, DateTime from, DateTime to, Boolean allHosts, Boolean allServices, Boolean allDomains)
		{
			List<TaskInformation> result = new List<TaskInformation>();
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/taskinformations/getqueueprocessedtimeframe?queue=" + queue + "&domain=" + domain + "&from=" + from.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + "&to=" + to.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + (allHosts ? "&allhosts=1" : "") + (allServices ? "&allservices=1" : "") + (allDomains ? "&alldomains=1" : ""), false));
			String ser = x.XPathSelectElement("//commandResponse").Value;
			Byte[] s = System.Convert.FromBase64String(ser);
			MemoryStream ms = new MemoryStream(s);
			ms.Seek(0, 0);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			List<List<Object>> tempresult = (List<List<Object>>)binaryFormatter.Deserialize(ms);
			foreach (var t in tempresult)
			{
				result.Add(TaskInformation.FromSerializedObject(t));
			}
			return result;
		}
		/// <summary>
		/// Returns the service name of the queried whizFlow host
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint</param>
		/// <returns>Service name</returns>
		public static String ServiceNameGet(String whizFlow)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/service/getname", false));
			String result = x.XPathSelectElement("//commandResponse").Value;
			return result;
		}
		/// <summary>
		/// Returns the hostname where the queried whizFlow is running
		/// </summary>
		/// <param name="whizFlow">WhizFlow endpoint</param>
		/// <returns>Hostname</returns>
		public static String HostNameGet(String whizFlow)
		{
			XDocument x = XDocument.Parse(HttpUtilities.HttpCall(whizFlow + "/service/gethost", false));
			String result = x.XPathSelectElement("//commandResponse").Value;
			return result;
		}
	}
}
