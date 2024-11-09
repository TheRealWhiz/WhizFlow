using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Management;
using Whiz.WhizFlow.Common;
using Whiz.WhizFlow.Engine.Modules;
using Whiz.Framework.Configuration;
using Whiz.Framework.Configuration.Xml;

namespace Whiz.WhizFlow.Service
{
	/// <summary>
	/// WhizFlow windows service class
	/// </summary>
	public partial class WindowsService : ServiceBase
	{
		/// <summary>
		/// The handler of all the app domains in WhizFlow
		/// </summary>
		private Whiz.WhizFlow.Engine.AppDomainManager _whizFlowManager;
		/// <summary>
		/// The display name of the windows service
		/// </summary>
		private String _serviceName;
		/// <summary>
		/// Constructor
		/// </summary>
		public WindowsService()
		{
			InitializeComponent();
		}
		/// <summary>
		/// The start of the service
		/// </summary>
		/// <param name="args"></param>
		protected override void OnStart(string[] args)
		{
			String configFile;
			// Retrieve the service instance name
			_serviceName = "";
			try
			{
				Int32 pid = Process.GetCurrentProcess().Id;
				ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DisplayName FROM Win32_Service WHERE ProcessId = " + pid.ToString());
				foreach (ManagementObject obj in searcher.Get())
				{
					_serviceName = obj["DisplayName"].ToString();
					break;
				}
			}
			catch (Exception ex)
			{
				EventLog whizFlowLog = new EventLog();
				whizFlowLog.Source = "WhizFlow";
				System.Diagnostics.EventLog.WriteEntry(_serviceName, ex.Message + Environment.NewLine + ex.StackTrace, EventLogEntryType.Error);
				this.Stop();
				return;
			}

			try
			{
				configFile = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Whiz").OpenSubKey("WhizFlow").OpenSubKey(_serviceName).GetValue("ConfigurationPath").ToString();
			}
			catch (Exception ex)
			{
				// we write the error to eventlog
				EventLog whizFlowLog = new EventLog();
				whizFlowLog.Source = "WhizFlow";
				System.Diagnostics.EventLog.WriteEntry(_serviceName, ex.Message + Environment.NewLine + ex.StackTrace, EventLogEntryType.Error);
				this.Stop();
				return;
			}
			// loads the main config
			GenericConfiguration config;
			try
			{
				config = new GenericConfiguration();
				config.LoadFromXmlFile(configFile, true);
			}
			catch (Exception ex)
			{
				// we write the error to eventlog
				EventLog whizFlowLog = new EventLog();
				whizFlowLog.Source = "WhizFlow";
				System.Diagnostics.EventLog.WriteEntry("WhizFlow", ex.Message + Environment.NewLine + ex.StackTrace, EventLogEntryType.Error);
				this.Stop();
				return;
			}

			// starting Log Process
			try
			{
				Whiz.WhizFlow.Log.Init(config.Get("config/db").Value, new System.Diagnostics.PerformanceCounter(Whiz.WhizFlow.Engine.Monitoring.PerformanceCounters.Utilities.FormatPerformanceCounterCategoryName(_serviceName), Whiz.WhizFlow.Engine.Monitoring.PerformanceCounters.Utilities.LOGS_COUNTER_NAME, "DefaultDomain_Logs", false), _serviceName, "Main");
			}
			catch (Exception ex)
			{
				// we write the error to eventlog
				EventLog whizFlowLog = new EventLog();
				whizFlowLog.Source = "WhizFlow";
				System.Diagnostics.EventLog.WriteEntry("WhizFlow", ex.Message + Environment.NewLine + ex.StackTrace, EventLogEntryType.Error);
				this.Stop();
				return;
			}

			try
			{
				_whizFlowManager = new Engine.AppDomainManager(config, _serviceName);
				_whizFlowManager.CreateAppDomains();
			}
			catch (Exception ex)
			{
				EventLog whizFlowLog = new EventLog();
				whizFlowLog.Source = "WhizFlow";
				System.Diagnostics.EventLog.WriteEntry("WhizFlow", ex.Message + Environment.NewLine + ex.StackTrace, EventLogEntryType.Error);
				this.Stop();
				return;
			}
		}
		/// <summary>
		/// The stop of the service
		/// </summary>
		protected override void OnStop()
		{
			try
			{
				// stops all the appdomains
				_whizFlowManager.StopAppDomains();
				// destroy the Log Process
				Whiz.WhizFlow.Log.Dispose();
			}
			catch (Exception ex)
			{
				// we write the error to eventlog
				EventLog whizFlowLog = new EventLog();
				whizFlowLog.Source = "WhizFlow";
				System.Diagnostics.EventLog.WriteEntry(_serviceName, ex.Message + Environment.NewLine + ex.StackTrace, EventLogEntryType.Error);
				this.Stop();
				return;
			}
		}

	}
}
