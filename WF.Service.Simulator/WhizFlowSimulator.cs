using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Whiz.WhizFlow.Engine.Modules;
using Whiz.WhizFlow.Common;
using Whiz.Framework.Configuration;
using Whiz.Framework.Configuration.Xml;
using Whiz.WhizFlow.Engine.Monitoring.PerformanceCounters;

namespace Whiz.WhizFlow.Tools.WhizFlowSimulator
{
	public partial class WhizFlowSimulator : Form
	{
		private Whiz.WhizFlow.Engine.AppDomainManager _whizFlowManager;
		public WhizFlowSimulator()
		{
			InitializeComponent();
		}
		public WhizFlowSimulator(String config, String serviceName)
		{
			InitializeComponent();
			txtMainConfig.Text = config;
			txtServiceName.Text = serviceName;
		}
		private void btnStart_Click(object sender, EventArgs e)
		{
			String strConfig = txtMainConfig.Text;
			// loads the main config
			GenericConfiguration config;
			try
			{
				config = new GenericConfiguration();
				config.LoadFromXmlFile(strConfig, true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			// starting Log Process
			try
			{
				Whiz.WhizFlow.Log.Init(config.Get("config/db").Value, new System.Diagnostics.PerformanceCounter(Whiz.WhizFlow.Engine.Monitoring.PerformanceCounters.Utilities.FormatPerformanceCounterCategoryName(txtServiceName.Text), Whiz.WhizFlow.Engine.Monitoring.PerformanceCounters.Utilities.LOGS_COUNTER_NAME, "DefaultDomain_Logs", false), txtServiceName.Text, "Main");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			// starting App domains
			try
			{
				_whizFlowManager = new Engine.AppDomainManager(config, txtServiceName.Text);
				_whizFlowManager.CreateAppDomains();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}
		private void btnStop_Click(object sender, EventArgs e)
		{
			try
			{
				// stops all the appdomains
				_whizFlowManager.StopAppDomains();
				_whizFlowManager.ShutdownHttp();
				// destroy the Log Process
				Whiz.WhizFlow.Log.Dispose();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;

			}
		}
		private void btnPCCreate_Click(object sender, EventArgs e)
		{
			try
			{
				Utilities.CreateWhizFlowPerformanceCounters(txtServiceName.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void btnPCDelete_Click(object sender, EventArgs e)
		{
			try
			{
				Utilities.DeleteWhizFlowPerformanceCounters(txtServiceName.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
