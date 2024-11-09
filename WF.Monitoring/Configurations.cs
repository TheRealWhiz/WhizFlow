using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Whiz.Monitoring.Application.Common;
using Whiz.WhizFlow.Common.Objects;
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Tasks;
using System.Threading.Tasks;

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// WhizFlow Configurations viewer. This class shows only the configurations hosted on the WhizFlow Database (dynamic domains). Static domains cannot be handled by this class.
	/// </summary>
	public partial class Configurations : PluginBaseForm
	{
		private String _whizFlow;
		private String _host;
		private String _service;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		public Configurations(String whizFlow)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
		}
		private void TaskContentViewer_Load(object sender, EventArgs e)
		{
			this.Text = "Configurations of " + _whizFlow;
		}
		private void LoadList()
		{
			Task.Run(() =>
				{
					try
					{
						_host = Query.HostNameGet(_whizFlow);
						_service = Query.ServiceNameGet(_whizFlow);
						List<Query.WhizFlowConfiguration> configs = Query.DomainConfigurationsGetList(_whizFlow);
						this.Invoke((MethodInvoker)delegate
						{
							lsvConfigurations.Items.Clear();
							foreach (var c in configs)
							{
								ListViewItem i = new ListViewItem();
								i.Text = c.Id.ToString();
								i.SubItems.Add(c.Hostname);
								i.SubItems.Add(c.Service);
								i.SubItems.Add(c.Domain);
								i.SubItems.Add(c.Active.ToString());
								i.Tag = c;
								if (c.Hostname != _host || c.Service != _service) i.BackColor = Color.LightGray;
								lsvConfigurations.Items.Add(i);
							}
						});
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						this.Close();
					}
				});
		}
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lsvConfigurations.SelectedItems.Count == 0) return;
			Query.WhizFlowConfiguration c = (Query.WhizFlowConfiguration)lsvConfigurations.SelectedItems[0].Tag;
			if (c.Hostname != _host || c.Service != _service) return;
			String domain = ((Query.WhizFlowConfiguration)lsvConfigurations.SelectedItems[0].Tag).Domain;
			Task.Run(() =>
			{
				try
				{
					Query.DomainConfigurationDelete(_whizFlow, domain);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					this.Close();
				}
				LoadList();
			});
		}
		private void btnLoad_Click(object sender, EventArgs e)
		{
			LoadList();
		}
		private void lsvConfigurations_SelectedIndexChanged(object sender, EventArgs e)
		{
			txtConfiguration.Text = "";
			if (lsvConfigurations.SelectedItems.Count > 0)
			{
				txtConfiguration.Text = ((Query.WhizFlowConfiguration)lsvConfigurations.SelectedItems[0].Tag).Configuration;
			}
		}
		private void btnSave_Click(object sender, EventArgs e)
		{
			Query.DomainConfigurationSave(_whizFlow, txtConfiguration.Text);
		}
		private void btnActivate_Click(object sender, EventArgs e)
		{
			if (lsvConfigurations.SelectedItems.Count > 0)
			{
				Query.DomainConfigurationActivate(_whizFlow, lsvConfigurations.SelectedItems[0].SubItems[3].Text);
			}
		}
		private void btnDeactivate_Click(object sender, EventArgs e)
		{
			if (lsvConfigurations.SelectedItems.Count > 0)
			{
				Query.DomainConfigurationDeactivate(_whizFlow, lsvConfigurations.SelectedItems[0].SubItems[3].Text);
			}
		}
	}
}
