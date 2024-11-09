using Whiz.Framework.Configuration;
using Whiz.Framework.Configuration.Xml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Whiz.WhizFlow.Engine
{
	internal enum HttpInterfaceCommand
	{
		ListAppDomains,
		StartAppDomains,
		StopAppDomains,
		StartAppDomain,
		StopAppDomain,
		AddAppDomain,
		ActivateDomainConfiguration,
		DeactivateDomainConfiguration,
		GetDomainConfiguration,
		SaveDomainConfiguration,
		DeleteDomainConfiguration,
		ListDomainConfigurations,
		ActivateDomainScheduler,
		DeactivateDomainScheduler,
		ChangeDomainSchedulerRecurrence,
		RestoreDomainSchedulerRecurrence,
		ListDomainQueues,
		ListDomainSchedulers,
		GetLogs1,
		GetLogs2,
		GetLogs3,
		GetTaskContent,
		GetTaskInformations1,
		GetTaskInformations2,
		GetTaskInformations3,
		GetTaskInformations4,
		GetTaskInformations5,
		ListDomainWorkers
	}
	internal class HttpInterfaceEventArgs
	{
		internal HttpInterfaceCommand Command { get; set; }
		internal Object Parameters { get; set; }
		internal Object Response { get; set; }
	}
	internal class HttpInterfaceDomainCommandEventArgs
	{
		internal NameValueCollection QueryString { get; set; }
		internal String Payload { get; set; }
		internal String Domain { get; set; }
		internal String Command { get; set; }
		internal String Response { get; set; }
		internal String Method { get; set; }
	}
	/// <summary>
	/// WhizFlow http interface
	/// </summary>
	internal class HttpInterface
	{
		internal delegate void HttpInterfaceCommandDelegate(Object sender, HttpInterfaceEventArgs e);
		internal delegate void HttpInterfaceDomainCommandDelegate(Object sender, HttpInterfaceDomainCommandEventArgs e);
		internal event HttpInterfaceCommandDelegate HttpCommand;
		internal event HttpInterfaceDomainCommandDelegate HttpDomainCommand;
		/// <summary>
		/// The http listener
		/// </summary>
		private HttpListener _localHost;
		/// <summary>
		/// The actual service name
		/// </summary>
		private String _serviceName;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="serviceName">The actual service name</param>
		public HttpInterface(String serviceName)
		{
			_localHost = new HttpListener();
			_serviceName = serviceName;
		}
		/// <summary>
		/// Setups the http
		/// </summary>
		/// <param name="prefix"></param>
		public void Setup(String prefix)
		{
			_localHost.Prefixes.Add(prefix);
		}
		/// <summary>
		/// Start the http
		/// </summary>
		public void Start()
		{
			_localHost.Start();
			_localHost.BeginGetContext(new AsyncCallback(RequestCallback), _localHost);
		}
		/// <summary>
		/// Stop the http
		/// </summary>
		public void Stop()
		{
			_localHost.Stop();
		}
		/// <summary>
		/// Request handler
		/// </summary>
		/// <param name="result"></param>
		private void RequestCallback(IAsyncResult result)
		{
			try
			{
				HttpListener local = (HttpListener)result.AsyncState;
				HttpListenerContext context = local.EndGetContext(result);

				_localHost.BeginGetContext(new AsyncCallback(RequestCallback), _localHost);

				HttpListenerRequest request = context.Request;
				HttpListenerResponse response = context.Response;
				String responseString = "WhizFlow Http Interface";
				String domain;
				String queue;
				Int32 entity;
				String name;
				try
				{
					if (request.Url.AbsolutePath.StartsWith("/domaincommand/"))
					{
						if (HttpDomainCommand != null)
						{

							String[] parts = request.Url.AbsolutePath.Split('/');
							HttpInterfaceDomainCommandEventArgs h = new HttpInterfaceDomainCommandEventArgs();
							h.Domain = parts[2];
							h.Command = parts[3];
							h.Method = request.HttpMethod;
							h.QueryString = request.QueryString;
							if (request.HttpMethod == "PUT" || request.HttpMethod == "POST")
							{
								String reqRaw = (new StreamReader(context.Request.InputStream)).ReadToEnd();
								h.Payload = reqRaw;
							}
							else
							{
								h.Payload = null;
							}
							HttpDomainCommand(this, h);
							responseString = h.Response;
						}
					}
					else
					{
						switch (request.Url.AbsolutePath)
						{
							// the logs must be get by the domains because of the different database configuration
							case "/logs/get1":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.GetLogs1;
									e.Parameters = request.QueryString;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/logs/get2":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.GetLogs2;
									e.Parameters = request.QueryString;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/logs/get3":
								// gets the logs
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.GetLogs3;
									e.Parameters = request.QueryString;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/pc/t":
								// tasks processed per queue
								domain = request.QueryString["domain"];
								queue = request.QueryString["queue"];
								responseString = "<whizFlow><pc><t>" + Whiz.WhizFlow.Engine.Monitoring.Utilities.PerformanceCounters.BE.PerformanceCounters.GetQueueProcessedTasks(Environment.MachineName, _serviceName, domain, queue).ToString() + "</t><service>" + _serviceName + "</service><domain>" + domain + "</domain><queue>" + queue + "</queue></pc></whizFlow>";
								break;
							case "/pc/tps":
								// tasks per second processed per queue
								domain = request.QueryString["domain"];
								queue = request.QueryString["queue"];
								responseString = "<whizFlow><pc><tps>" + Whiz.WhizFlow.Engine.Monitoring.Utilities.PerformanceCounters.BE.PerformanceCounters.GetQueueProcessedTasksPerSecond(Environment.MachineName, _serviceName, domain, queue).ToString() + "</tps><service>" + _serviceName + "</service><domain>" + domain + "</domain><queue>" + queue + "</queue></pc></whizFlow>";
								break;
							case "/pc/logs":
								// domain logs
								domain = request.QueryString["domain"];
								responseString = "<whizFlow><pc><logs>" + Whiz.WhizFlow.Engine.Monitoring.Utilities.PerformanceCounters.BE.PerformanceCounters.GetLogs(Environment.MachineName, _serviceName, domain).ToString() + "</logs><service>" + _serviceName + "</service><domain>" + domain + "</domain></pc></whizFlow>";
								break;
							case "/wmi/getobject":
								// gets object in the wmi repository
								entity = Int32.Parse(request.QueryString["entity"]);
								name = request.QueryString["name"];
								Dictionary<String, String> wmiObject = Whiz.WhizFlow.Engine.Monitoring.Utilities.WMI.BE.WMI.GetObject((Monitoring.Utilities.WMI.Entities)entity, _serviceName, Environment.MachineName, name);
								String objectXml = "";
								foreach (KeyValuePair<String, String> kvp in wmiObject)
								{
									objectXml += "<" + kvp.Key + "><![CDATA[" + kvp.Value + "]]></" + kvp.Key + ">";
								}
								responseString = "<whizFlow><wmi><object type='" + entity.ToString() + "' name='" + name + "'>" + objectXml + "</object></wmi></whizFlow>";
								break;
							case "/wmi/getobjects":
								// gets objects in the wmi repository
								entity = Int32.Parse(request.QueryString["entity"]);
								Dictionary<Tuple<String, String, String>, Dictionary<String, String>> wmiObjects = Whiz.WhizFlow.Engine.Monitoring.Utilities.WMI.BE.WMI.GetObjects((Monitoring.Utilities.WMI.Entities)entity, _serviceName, Environment.MachineName);
								String objectsXml = "<objects>";
								foreach (KeyValuePair<Tuple<String, String, String>, Dictionary<String, String>> kvpObjs in wmiObjects)
								{
									objectsXml += "<object type='" + entity.ToString() + "'>";
									foreach (KeyValuePair<String, String> kvp in kvpObjs.Value)
									{
										objectsXml += "<" + kvp.Key + "><![CDATA[" + kvp.Value + "]]></" + kvp.Key + ">";
									}
									objectsXml += "</object>";
								}
								objectsXml += "</objects>";
								responseString = "<whizFlow><wmi>" + objectsXml + "</wmi></whizFlow>";
								break;
							case "/domains/list":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.ListAppDomains;
									e.Parameters = null;
									HttpCommand(this, e);
									Dictionary<String, AppDomainManager.WhizFlowDomain> domains = (Dictionary<String, AppDomainManager.WhizFlowDomain>)e.Response;
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse><domains>");
									foreach (String d in domains.Keys)
									{
										sb.Append("<domain>" + d + "</domain>");
									}
									sb.Append("</domains></commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domains/start":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.StartAppDomains;
									e.Parameters = null;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									if (e.Response != null)
									{
										Dictionary<String, AppDomainManager.WhizFlowDomain> domains = (Dictionary<String, AppDomainManager.WhizFlowDomain>)e.Response;
										sb.Append("<whizFlow><commandResponse><domainsStarted>");
										foreach (String d in domains.Keys)
										{
											sb.Append("<domain>" + d + "</domain>");
										}
										sb.Append("</domainsStarted></commandResponse></whizFlow>");
									}
									else
									{
										sb.Append("<whizFlow><commandResponse>There were domains already running. Command ignored.");
										sb.Append("</commandResponse></whizFlow>");
									}
									responseString = sb.ToString();
								}
								break;
							case "/domains/stop":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.StopAppDomains;
									e.Parameters = null;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domain/queues":
								break;
							case "/domain/schedulers":
								break;
							case "/domain/workers":
								break;
							case "/domain/start":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.StartAppDomain;
									e.Parameters = request.QueryString["domain"];
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domain/stop":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.StopAppDomain;
									e.Parameters = request.QueryString["domain"];
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domain/add":
								if (request.HttpMethod == "POST")
								{
									// the body of the request is the configuration (xml) of the domain to load
									if (HttpCommand != null)
									{
										HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
										e.Command = HttpInterfaceCommand.AddAppDomain;
										String body = new StreamReader(request.InputStream).ReadToEnd();
										e.Parameters = body;
										HttpCommand(this, e);
										StringBuilder sb = new StringBuilder();
										sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
										responseString = sb.ToString();
									}
								}
								break;
							case "/domainconfiguration/activate":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Parameters = request.QueryString["domain"];
									e.Command = HttpInterfaceCommand.ActivateDomainConfiguration;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domainconfiguration/save":
								if (request.HttpMethod == "POST")
								{
									// the body of the request is the configuration (xml) of the domain to load
									if (HttpCommand != null)
									{
										HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
										e.Command = HttpInterfaceCommand.SaveDomainConfiguration;
										String body = new StreamReader(request.InputStream).ReadToEnd();
										e.Parameters = body;
										HttpCommand(this, e);
										StringBuilder sb = new StringBuilder();
										sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
										responseString = sb.ToString();
									}
								}
								break;
							case "/domainconfiguration/get":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Parameters = request.QueryString["domain"];
									e.Command = HttpInterfaceCommand.GetDomainConfiguration;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domainconfiguration/deactivate":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Parameters = request.QueryString["domain"];
									e.Command = HttpInterfaceCommand.DeactivateDomainConfiguration;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domainconfiguration/delete":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Parameters = request.QueryString["domain"];
									e.Command = HttpInterfaceCommand.DeleteDomainConfiguration;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domainconfiguration/list":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.ListDomainConfigurations;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domainscheduler/activate":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.ActivateDomainScheduler;
									e.Parameters = request.QueryString;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domainscheduler/deactivate":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.DeactivateDomainScheduler;
									e.Parameters = request.QueryString;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>ok</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domainscheduler/changerecurrence":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.ChangeDomainSchedulerRecurrence;
									e.Parameters = request.QueryString;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/domainscheduler/restorerecurrence":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.RestoreDomainSchedulerRecurrence;
									e.Parameters = request.QueryString;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/taskcontent/get":
								if (HttpCommand != null)
								{
									HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
									e.Command = HttpInterfaceCommand.GetTaskContent;
									e.Parameters = request.QueryString;
									HttpCommand(this, e);
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
								}
								break;
							case "/taskinformations/getbytaskcontentid":
								{
									if (HttpCommand != null)
									{
										HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
										e.Command = HttpInterfaceCommand.GetTaskInformations1;
										e.Parameters = request.QueryString;
										HttpCommand(this, e);
										StringBuilder sb = new StringBuilder();
										sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
										responseString = sb.ToString();
									}
									break;
								}
							case "/taskinformations/getprocessed":
								{
									if (HttpCommand != null)
									{
										HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
										e.Command = HttpInterfaceCommand.GetTaskInformations2;
										e.Parameters = request.QueryString;
										HttpCommand(this, e);
										StringBuilder sb = new StringBuilder();
										sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
										responseString = sb.ToString();
									}
									break;
								}
							case "/taskinformations/getqueueprocessed":
								{
									if (HttpCommand != null)
									{
										HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
										e.Command = HttpInterfaceCommand.GetTaskInformations3;
										e.Parameters = request.QueryString;
										HttpCommand(this, e);
										StringBuilder sb = new StringBuilder();
										sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
										responseString = sb.ToString();
									}
									break;
								}
							case "/taskinformations/getprocessedtimeframe":
								{
									if (HttpCommand != null)
									{
										HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
										e.Command = HttpInterfaceCommand.GetTaskInformations4;
										e.Parameters = request.QueryString;
										HttpCommand(this, e);
										StringBuilder sb = new StringBuilder();
										sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
										responseString = sb.ToString();
									}
									break;
								}
							case "/taskinformations/getqueueprocessedtimeframe":
								{
									if (HttpCommand != null)
									{
										HttpInterfaceEventArgs e = new HttpInterfaceEventArgs();
										e.Command = HttpInterfaceCommand.GetTaskInformations5;
										e.Parameters = request.QueryString;
										HttpCommand(this, e);
										StringBuilder sb = new StringBuilder();
										sb.Append("<whizFlow><commandResponse>" + (String)e.Response + "</commandResponse></whizFlow>");
										responseString = sb.ToString();
									}
									break;
								}
							case "/service/getname":
								{
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + _serviceName + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
									break;
								}
							case "/service/gethost":
								{
									StringBuilder sb = new StringBuilder();
									sb.Append("<whizFlow><commandResponse>" + Environment.MachineName + "</commandResponse></whizFlow>");
									responseString = sb.ToString();
									break;
								}
						}
					}
				}
				catch (Exception ex)
				{
					responseString = "<whizFlow><error>Something went wrong</error><detail><![CDATA[" + ex.Message + "-StackTrace:" + ex.StackTrace + "]]></detail></whizFlow>";
				}
				if (responseString == null) responseString = "WhizFlow Http Interface";
				Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
				response.ContentType = "text/xml";
				response.ContentLength64 = buffer.Length;
				System.IO.Stream outputStream = response.OutputStream;
				outputStream.Write(buffer, 0, buffer.Length);
				outputStream.Close();
				//_localHost.BeginGetContext(new AsyncCallback(RequestCallback), _localHost);
			}
			catch { }
		}
	}
}
