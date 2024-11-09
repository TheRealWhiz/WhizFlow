using Whiz.Framework.Configuration;
using Whiz.Framework.Configuration.Xml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Whiz.WhizFlow.Engine
{
	/// <summary>
	/// WhizFlow App Domain Manager
	/// </summary>
	public class AppDomainManager
	{
		private String _serviceName;
		private GenericConfiguration _configuration;
		private String _prefixes;
		private Whiz.WhizFlow.Engine.HttpInterface _http;
		/// <summary>
		/// The representation of a WhizFlow appdomain
		/// </summary>
		internal class WhizFlowDomain
		{
			internal AppDomain ApplicationDomain { get; set; }
			internal WhizFlow WhizFlowReference { get; set; }
			internal GenericConfiguration Configuration { get; set; }
			internal String DatabaseConnectionString { get; set; }
		}
		/// <summary>
		/// All the appdomains handled by the instance of WhizFlow
		/// </summary>
		private Dictionary<String, WhizFlowDomain> _appDomains;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="serviceName"></param>
		public AppDomainManager(GenericConfiguration configuration, String serviceName)
		{
			_appDomains = new Dictionary<String, WhizFlowDomain>();
			_configuration = configuration;
			_serviceName = serviceName;
			_prefixes = _configuration.Get("config/http").Value;
			// setups WMI
			WMISetup();
			_http = new Engine.HttpInterface(serviceName);
			_http.Setup(_prefixes);
			_http.Start();
			_http.HttpCommand += _http_HttpCommand;
			_http.HttpDomainCommand += _http_HttpDomainCommand;
		}
		/// <summary>
		/// Registers the assemblies for WMI
		/// </summary>
		private void WMISetup()
		{
			System.Configuration.Install.AssemblyInstaller asmInst = new System.Configuration.Install.AssemblyInstaller(Assembly.GetAssembly(typeof(Whiz.WhizFlow.Engine.Monitoring.QueueHandlerThread)), null);
			System.Collections.IDictionary mySavedState = new System.Collections.Hashtable();
			mySavedState.Clear();
			asmInst.UseNewContext = true;
			try
			{
				asmInst.Install(mySavedState);
				asmInst.Commit(mySavedState);
			}
			catch
			{
				asmInst.Rollback(mySavedState);
			}
			System.Configuration.Install.AssemblyInstaller asmInst2 = new System.Configuration.Install.AssemblyInstaller(Assembly.GetAssembly(typeof(Whiz.WhizFlow.Engine.Monitoring.Events.WhizFlowError)), null);
			System.Collections.IDictionary mySavedState2 = new System.Collections.Hashtable();
			mySavedState2.Clear();
			asmInst2.UseNewContext = true;
			try
			{
				asmInst2.Install(mySavedState2);
				asmInst2.Commit(mySavedState2);
			}
			catch
			{
				asmInst2.Rollback(mySavedState2);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _http_HttpCommand(object sender, HttpInterfaceEventArgs e)
		{
			switch (e.Command)
			{
				case HttpInterfaceCommand.ListAppDomains:
					e.Response = _appDomains;
					break;
				case HttpInterfaceCommand.StartAppDomains:
					if (_appDomains.Count == 0)
					{
						CreateAppDomains();
						e.Response = _appDomains;
					}
					else e.Response = null;
					break;
				case HttpInterfaceCommand.StopAppDomains:
					if (_appDomains.Count != 0)
					{
						StopAppDomains();
					}
					break;
				case HttpInterfaceCommand.AddAppDomain:
					try
					{
						GenericConfiguration g = new GenericConfiguration();
						g.LoadFromXml(XElement.Parse((String)e.Parameters), true);
						CreateAppDomainRuntime(g.GetList("domain").ToList()[0], (String)e.Parameters);
						e.Response = _appDomains;
					}
					catch (Exception ex)
					{
						e.Response = ex;
					}
					break;
				case HttpInterfaceCommand.StopAppDomain:
					StopAppDomain((String)e.Parameters, true);
					e.Response = _appDomains;
					break;
				case HttpInterfaceCommand.GetDomainConfiguration:
					try
					{
						var c = DAL.ReadWhizFlowConfiguration(Environment.MachineName, _serviceName, (String)e.Parameters, _configuration.Get("config/db").Value);
						StringBuilder s = new StringBuilder();
						s.Append("<configurations>");
						s.Append("<configuration>");
						s.Append("<host>");
						s.Append(c.Hostname);
						s.Append("</host>");
						s.Append("<service>");
						s.Append(c.Service);
						s.Append("</service>");
						s.Append("<domain>");
						s.Append(c.Domain);
						s.Append("</domain>");
						s.Append("<id>");
						s.Append(c.Id.ToString());
						s.Append("</id>");
						s.Append("<active>");
						s.Append(c.Active);
						s.Append("</active>");
						s.Append("<xml>");
						s.Append(c.Configuration);
						s.Append("</xml>");
						s.Append("</configuration>");
						s.Append("</configurations>");
						e.Response = s.ToString();
					}
					catch (Exception ex)
					{
						e.Response = ex.Message;
					}
					break;
				case HttpInterfaceCommand.SaveDomainConfiguration:
					GenericConfiguration t = new GenericConfiguration();
					WhizFlowConfiguration p = new WhizFlowConfiguration();
					t.LoadFromXml(XElement.Parse((String)e.Parameters), true);
					p.Active = true;
					p.Configuration = (String)e.Parameters;
					p.Hostname = Environment.MachineName;
					p.Service = _serviceName;
					p.Domain = t.Get("domain/name").Value;
					DAL.SaveWhizFlowConfiguration(p, _configuration.Get("config/db").Value);
					break;
				case HttpInterfaceCommand.ListDomainConfigurations:
					try
					{
						var list = DAL.ReadAllWhizFlowConfigurations(_configuration.Get("config/db").Value);
						StringBuilder s = new StringBuilder();
						s.Append("<configurations>");
						foreach (var c in list)
						{
							s.Append("<configuration>");
							s.Append("<host>");
							s.Append(c.Hostname);
							s.Append("</host>");
							s.Append("<service>");
							s.Append(c.Service);
							s.Append("</service>");
							s.Append("<domain>");
							s.Append(c.Domain);
							s.Append("</domain>");
							s.Append("<id>");
							s.Append(c.Id.ToString());
							s.Append("</id>");
							s.Append("<active>");
							s.Append(c.Active);
							s.Append("</active>");
							s.Append("<xml>");
							s.Append(c.Configuration);
							s.Append("</xml>");
							s.Append("</configuration>");
						}
						s.Append("</configurations>");
						e.Response = s.ToString();
					}
					catch (Exception ex)
					{
						e.Response = ex.Message;
					}
					break;
				case HttpInterfaceCommand.DeleteDomainConfiguration:
					try
					{
						DAL.DeleteWhizFlowConfiguration(Environment.MachineName, _serviceName, (String)e.Parameters, _configuration.Get("config/db").Value);
						e.Response = "Ok";
					}
					catch (Exception ex)
					{
						e.Response = ex.Message;
					}
					break;
				case HttpInterfaceCommand.ActivateDomainConfiguration:
					WhizFlowConfiguration wfConfigurationActivation = new WhizFlowConfiguration();
					wfConfigurationActivation.Domain = (String)e.Parameters;
					wfConfigurationActivation.Active = true;
					wfConfigurationActivation.Configuration = null;
					wfConfigurationActivation.Service = _serviceName;
					wfConfigurationActivation.Hostname = Environment.MachineName;
					DAL.SaveWhizFlowConfiguration(wfConfigurationActivation, _configuration.Get("config/db").Value);
					break;
				case HttpInterfaceCommand.DeactivateDomainConfiguration:
					WhizFlowConfiguration wfConfigurationDeactivation = new WhizFlowConfiguration();
					wfConfigurationDeactivation.Domain = (String)e.Parameters;
					wfConfigurationDeactivation.Active = false;
					wfConfigurationDeactivation.Configuration = null;
					wfConfigurationDeactivation.Service = _serviceName;
					wfConfigurationDeactivation.Hostname = Environment.MachineName;
					DAL.SaveWhizFlowConfiguration(wfConfigurationDeactivation, _configuration.Get("config/db").Value);
					break;
				case HttpInterfaceCommand.StartAppDomain:
					try
					{
						GenericConfiguration g = new GenericConfiguration();
						g.LoadFromXml(XElement.Parse(DAL.ReadWhizFlowConfigurations(Environment.MachineName, _serviceName, _configuration.Get("config/db").Value).Where(pt => pt.Domain == (String)e.Parameters).First().Configuration), true);
						CreateAppDomain(g.GetList("domain").ToList()[0]);
						e.Response = _appDomains;
					}
					catch (Exception ex)
					{
						e.Response = ex;
					}
					break;
				case HttpInterfaceCommand.ActivateDomainScheduler:
					try
					{
						e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.SchedulersManagement(HttpInterfaceCommand.ActivateDomainScheduler, (NameValueCollection)e.Parameters);
					}
					catch (Exception ex)
					{
						e.Response = ex;
					}
					break;
				case HttpInterfaceCommand.DeactivateDomainScheduler:
					try
					{
						e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.SchedulersManagement(HttpInterfaceCommand.DeactivateDomainScheduler, (NameValueCollection)e.Parameters);
					}
					catch (Exception ex)
					{
						e.Response = ex;
					}
					break;
				case HttpInterfaceCommand.ChangeDomainSchedulerRecurrence:
					try
					{
						e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.SchedulersManagement(HttpInterfaceCommand.ChangeDomainSchedulerRecurrence, (NameValueCollection)e.Parameters);
					}
					catch (Exception ex)
					{
						e.Response = ex;
					}
					break;
				case HttpInterfaceCommand.RestoreDomainSchedulerRecurrence:
					try
					{
						e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.SchedulersManagement(HttpInterfaceCommand.RestoreDomainSchedulerRecurrence, (NameValueCollection)e.Parameters);
					}
					catch (Exception ex)
					{
						e.Response = ex;
					}
					break;
				case HttpInterfaceCommand.GetLogs1:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetLogs1((NameValueCollection)e.Parameters);
					break;
				case HttpInterfaceCommand.GetLogs2:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetLogs2((NameValueCollection)e.Parameters);
					break;
				case HttpInterfaceCommand.GetLogs3:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetLogs3((NameValueCollection)e.Parameters);
					break;
				case HttpInterfaceCommand.GetTaskContent:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetTaskContent((NameValueCollection)e.Parameters);
					break;
				case HttpInterfaceCommand.GetTaskInformations1:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetTaskInformations1((NameValueCollection)e.Parameters);
					break;
				case HttpInterfaceCommand.GetTaskInformations2:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetTaskInformations2((NameValueCollection)e.Parameters);
					break;
				case HttpInterfaceCommand.GetTaskInformations3:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetTaskInformations3((NameValueCollection)e.Parameters);
					break;
				case HttpInterfaceCommand.GetTaskInformations4:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetTaskInformations4((NameValueCollection)e.Parameters);
					break;
				case HttpInterfaceCommand.GetTaskInformations5:
					e.Response = _appDomains[((NameValueCollection)e.Parameters)["domain"]].WhizFlowReference.GetTaskInformations5((NameValueCollection)e.Parameters);
					break;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _http_HttpDomainCommand(object sender, HttpInterfaceDomainCommandEventArgs e)
		{
			try
			{
				e.Response = _appDomains[e.Domain].WhizFlowReference.HttpCommand(e.Command, e.QueryString, e.Payload, e.Method);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>
		/// Creates the file-preconfigured WhizFlow domains
		/// </summary>
		public void CreateAppDomains()
		{
			Log.WriteLogAsync(Log.Module.AppDomainManager, Log.LogTypes.Information, "AppDomainManager", "WhizFlow " + _serviceName + " Starting AppDomain creation", "", _configuration.Get("config/db").Value);
			List<GenericConfiguration> domainsConfig = _configuration.GetList("config/domains/domain").ToList();
			foreach (GenericConfiguration domainConfig in domainsConfig)
			{
				CreateAppDomain(domainConfig);
			}
			// Reads db configurations
			foreach (WhizFlowConfiguration p in DAL.ReadWhizFlowConfigurations(Environment.MachineName, _serviceName, _configuration.Get("config/db").Value))
			{
				if (p.Active)
				{
					GenericConfiguration g = new GenericConfiguration();
					g.LoadFromXml(XElement.Parse(p.Configuration), true);
					CreateAppDomain(g.GetList("domain").ToList()[0]);
				}
			}
		}
		/// <summary>
		/// Create and start an appdomain
		/// </summary>
		/// <param name="domainConfig"></param>
		public void CreateAppDomain(GenericConfiguration domainConfig)
		{
			Log.WriteLogAsync(Log.Module.AppDomainManager, Log.LogTypes.Information, "AppDomainManager", "WhizFlow " + _serviceName + " creating AppDomain " + domainConfig.Get("name").Value, "", _configuration.Get("config/db").Value);
			if (_appDomains.ContainsKey(domainConfig.Get("name").Value))
			{
				Log.WriteLogAsync(Log.Module.AppDomainManager, Log.LogTypes.Warning, "AppDomainManager", "WhizFlow " + _serviceName + " cannot create AppDomain " + domainConfig.Get("name").Value + " because a domain with the same name already exists", "", _configuration.Get("config/db").Value);
				return;
			}
			WhizFlowDomain p = new WhizFlowDomain();
			AppDomain a;
			p.WhizFlowReference = Loader.Call(domainConfig.Get("name").Value, _serviceName, domainConfig, out a);
			p.ApplicationDomain = a;
			p.Configuration = domainConfig;
			p.DatabaseConnectionString = domainConfig.Get("db").Value;
			_appDomains.Add(domainConfig.Get("name").Value, p);
		}
		/// <summary>
		/// Create and start an appdomain saving its configuration on db
		/// </summary>
		/// <param name="domainConfig"></param>
		/// <param name="configuration"></param>
		public void CreateAppDomainRuntime(GenericConfiguration domainConfig, String configuration)
		{
			CreateAppDomain(domainConfig);
			WhizFlowConfiguration p = new WhizFlowConfiguration();
			p.Active = true;
			p.Configuration = configuration;
			p.Hostname = Environment.MachineName;
			p.Service = _serviceName;
			p.Domain = domainConfig.Get("name").Value;

			DAL.SaveWhizFlowConfiguration(p, _configuration.Get("config/db").Value);
		}
		/// <summary>
		/// Terminates all the app domains inside WhizFlow
		/// </summary>
		public void StopAppDomains()
		{
			foreach (WhizFlowDomain p in _appDomains.Values)
			{
				p.WhizFlowReference.Stop();
				p.WhizFlowReference.Dispose();
				p.WhizFlowReference = null;
				AppDomain.Unload(p.ApplicationDomain);
			}
			_appDomains = new Dictionary<String, WhizFlowDomain>();
		}
		/// <summary>
		/// Stop and eventually unload a single appdomain
		/// </summary>
		/// <param name="unload">Specify the unload of the appdomain</param>
		/// <param name="domainName">The domain to stop</param>
		public void StopAppDomain(String domainName, Boolean unload)
		{
			if (_appDomains.ContainsKey(domainName))
			{
				_appDomains[domainName].WhizFlowReference.Stop();
				_appDomains[domainName].WhizFlowReference.Dispose();
				if (unload)
				{
					_appDomains[domainName].WhizFlowReference = null;
					AppDomain.Unload(_appDomains[domainName].ApplicationDomain);
					_appDomains.Remove(domainName);
				}
			}
		}
		/// <summary>
		/// Starts the specified WhizFlow domain
		/// </summary>
		/// <param name="domainName"></param>
		public void StartAppDomain(String domainName)
		{
			_appDomains[domainName].WhizFlowReference.Start();
		}
		/// <summary>
		/// Shutdown the http interface
		/// </summary>
		public void ShutdownHttp()
		{
			_http.Stop();
		}
		/// <summary>
		/// Internal class for appdomain handling
		/// </summary>
		private class Loader : MarshalByRefObject
		{
			private WhizFlow CallInternal(Object[] parameters)
			{
				Assembly a = Assembly.LoadFile(Assembly.GetExecutingAssembly().Location);
				WhizFlow result = (WhizFlow)a.CreateInstance("Whiz.WhizFlow.Engine.WhizFlow", false, BindingFlags.CreateInstance, null, parameters, null, null);
				return result;
			}
			public static WhizFlow Call(String domainName, String serviceName, GenericConfiguration domainConfiguration, out AppDomain createdAppDom)
			{
				AppDomain dom = AppDomain.CreateDomain(domainName);
				Loader ld = (Loader)dom.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(Loader).FullName);
				Object[] parameters = new Object[3] { domainConfiguration, serviceName, domainName };
				WhizFlow result = ld.CallInternal(parameters);
				createdAppDom = dom;
				return result;
			}
		}
	}
}
