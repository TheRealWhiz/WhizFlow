using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration.Install;

namespace Whiz.WhizFlow.Service
{
	[RunInstaller(true)]
	public partial class WindowsServiceInstaller : Installer
	{
		public WindowsServiceInstaller()
		{
			InitializeComponent();
			this.BeforeInstall += new InstallEventHandler(ProjectInstaller_BeforeInstall);
			this.BeforeUninstall += new InstallEventHandler(ProjectInstaller_BeforeUninstall);
		}

		void ProjectInstaller_BeforeInstall(object sender, InstallEventArgs e)
		{
			if (!String.IsNullOrEmpty(this.Context.Parameters["User"]))
			{
				this.serviceManagerInstaller.Account = ServiceAccount.User;
				this.serviceManagerInstaller.Username = this.Context.Parameters["User"];
				if (!String.IsNullOrEmpty(this.Context.Parameters["Pass"]))
				{
					this.serviceManagerInstaller.Password = this.Context.Parameters["Pass"];
				}
			}

			if (!String.IsNullOrEmpty(this.Context.Parameters["ServiceName"]))
			{
				this.serviceManager.ServiceName = this.Context.Parameters["ServiceName"];
				this.serviceManager.Description = "WhizFlow Service for the " + this.Context.Parameters["ServiceName"] + " instance";
				this.serviceManager.DisplayName = this.Context.Parameters["ServiceName"];

				/* creating registry structure */
				if (!(Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").GetSubKeyNames().Contains("Whiz")))
				{
					Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("Whiz");
				}
				if (!(Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Whiz").GetSubKeyNames().Contains("WhizFlow")))
				{
					Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE", true).OpenSubKey("Whiz", true).CreateSubKey("WhizFlow");
				}
				if (!(Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Whiz").OpenSubKey("WhizFlow").GetSubKeyNames().Contains(this.serviceManager.ServiceName)))
				{
					Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE", true).OpenSubKey("Whiz", true).OpenSubKey("WhizFlow", true).CreateSubKey(this.serviceManager.ServiceName);
					Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE", true).OpenSubKey("Whiz", true).OpenSubKey("WhizFlow", true).OpenSubKey(this.serviceManager.ServiceName, true).SetValue("ConfigurationPath", String.IsNullOrEmpty(this.Context.Parameters["Config"]) ? "" : this.Context.Parameters["Config"], Microsoft.Win32.RegistryValueKind.String);
				}
			}
			else
			{
				throw (new Exception("A service name must be provided when installing this service"));
			}
			Engine.Monitoring.PerformanceCounters.Utilities.CreateWhizFlowPerformanceCounters(this.Context.Parameters["ServiceName"]);
		}

		void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e)
		{
			if (!String.IsNullOrEmpty(this.Context.Parameters["ServiceName"]))
			{
				this.serviceManager.ServiceName = this.Context.Parameters["ServiceName"];
			}
			else
			{
				throw (new Exception("A service name must be provided when uninstalling this service"));
			}
			Engine.Monitoring.PerformanceCounters.Utilities.DeleteWhizFlowPerformanceCounters(this.Context.Parameters["ServiceName"]);
		}
	}
}
