using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Whiz.Monitoring.Application.Common;

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// This is the main menu for all the monitoring functions for a single WhizFlow Host
	/// </summary>
	public partial class WhizFlowMonitor : PluginBaseForm
	{
		private String _host;
		private String _hostName;
		private System.Timers.Timer _timer;
		private String _serviceName;
		/// <summary>
		/// Constructor of the main menu.
		/// </summary>
		/// <param name="host">WhizFlow host in the form http://{address}:{port}</param>
		public WhizFlowMonitor(String host)
		{
			InitializeComponent();
			_host = host;
			_serviceName = Query.ServiceNameGet(host);
			_hostName = Query.HostNameGet(host);
			this.Text = "Monitoring : " + host;
			lblServer.Text = _hostName;
			lblServiceName.Text = _serviceName;
		}
		/// <summary>
		/// Timer event. Refresh the domains list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			try
			{
				var resultConfigs = Query.DomainConfigurationsGetList(_host);
				List<String> result = Query.DomainsGet(_host);
				this.Invoke((MethodInvoker)delegate()
				{
					foreach (String domain in result)
					{
						if (!lsvDomains.Items.ContainsKey(domain))
						{
							ListViewItem i = new ListViewItem();
							i.Text = domain;
							i.SubItems.Add("Running");
							i.Name = domain;
							lsvDomains.Items.Add(i);
						}
						else
						{
							lsvDomains.Items[domain].SubItems[1].Text = "Running";
						}
					}
					foreach (var p in resultConfigs)
					{
						if (!lsvDomains.Items.ContainsKey(p.Domain) && p.Service == _serviceName && p.Hostname == _hostName)
						{
							ListViewItem i = new ListViewItem();
							i.Text = p.Domain;
							i.SubItems.Add("Stopped");
							i.Name = p.Domain;
							lsvDomains.Items.Add(i);
						}
						else if (lsvDomains.Items.ContainsKey(p.Domain) && p.Service == _serviceName && p.Hostname == _hostName && !result.Contains(p.Domain))
						{
							lsvDomains.Items[p.Domain].SubItems[1].Text = "Stopped";
						}
					}
					List<String> toRemove = new List<String>();
					foreach (ListViewItem l in lsvDomains.Items)
					{
						if ((!result.Contains(l.Name) && resultConfigs.Where(t => t.Domain == l.Name && t.Service == _serviceName && t.Hostname == _hostName).Count() == 0))
						{
							toRemove.Add(l.Name);
						}
					}
					foreach (String d in toRemove)
					{
						lsvDomains.Items.Remove(lsvDomains.Items[d]);
					}
				});

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		/// <summary>
		/// Form closing event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WhizFlowMonitor_FormClosing(object sender, FormClosingEventArgs e)
		{
			_timer.Elapsed -= _timer_Elapsed;
		}
		/// <summary>
		/// Opens the queues window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnQueues_Click(object sender, EventArgs e)
		{
			if (lsvDomains.SelectedItems.Count == 0) return;
			Queues q = new Queues(_host, _serviceName, lsvDomains.SelectedItems[0].Text);
			OpenPluginForm(q);
		}
		/// <summary>
		/// Opens the schedulers window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSchedulers_Click(object sender, EventArgs e)
		{
			if (lsvDomains.SelectedItems.Count == 0) return;
			Schedulers q = new Schedulers(_host, _serviceName, lsvDomains.SelectedItems[0].Text);
			OpenPluginForm(q);
		}
		/// <summary>
		/// Opens the logs window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLogs_Click(object sender, EventArgs e)
		{
			if (lsvDomains.SelectedItems.Count == 0) return;
			Logs q = new Logs(_host, _serviceName, lsvDomains.SelectedItems[0].Text);
			OpenPluginForm(q);
		}
		/// <summary>
		/// Opens the task logs window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnTaskLogs_Click(object sender, EventArgs e)
		{
			if (lsvDomains.SelectedItems.Count == 0) return;
			TaskLogs q = new TaskLogs(_host, _serviceName, lsvDomains.SelectedItems[0].Text, 100);
			OpenPluginForm(q);
		}
		/// <summary>
		/// Opens the configurations window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnConfigs_Click(object sender, EventArgs e)
		{
			Configurations c = new Configurations(_host);
			OpenPluginForm(c);
		}
		/// <summary>
		/// Starts the selected domain
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnStart_Click(object sender, EventArgs e)
		{
			if (lsvDomains.SelectedItems.Count == 0) return;
			Query.DomainStart(_host, lsvDomains.SelectedItems[0].Text);
		}
		/// <summary>
		/// Stops the selected domain
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnStop_Click(object sender, EventArgs e)
		{
			if (lsvDomains.SelectedItems.Count == 0) return;
			Query.DomainStop(_host, lsvDomains.SelectedItems[0].Text);
		}
		/// <summary>
		/// Load event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WhizFlowMonitor_Load(object sender, EventArgs e)
		{
			Task.Run(() => { _timer_Elapsed(this, null); });
			_timer = new System.Timers.Timer(10000);
			_timer.Elapsed += _timer_Elapsed;
			_timer.Start();
		}
		/// <summary>
		/// Opens the task view interface
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnTaskView_Click(object sender, EventArgs e)
		{
			if (lsvDomains.SelectedItems.Count == 0) return;
			TaskContentViewer tcv = new TaskContentViewer(_host, _serviceName, lsvDomains.SelectedItems[0].Text);
			OpenPluginForm(tcv);
		}
		/// <summary>
		/// Opens the workers monitor
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnWorkers_Click(object sender, EventArgs e)
		{
			if (lsvDomains.SelectedItems.Count == 0) return;
			Workers q = new Workers(_host, _serviceName, lsvDomains.SelectedItems[0].Text);
			OpenPluginForm(q);
		}
	}
}