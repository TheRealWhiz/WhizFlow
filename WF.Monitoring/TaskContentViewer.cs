using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Whiz.Monitoring.Application.Common;
using Whiz.WhizFlow.Common.Objects;
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Tasks;

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// This form allow to view a TaskContent and all the processing information related.
	/// </summary>
	public partial class TaskContentViewer : PluginBaseForm
	{
		private String _whizFlow;
		private String _service;
		private String _domain;
		private Int32 _taskContentId;
		private Boolean _load;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="taskContentId">The TaskContent Id</param>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">Service name</param>
		/// <param name="domain">Domain name</param>
		public TaskContentViewer(Int32 taskContentId, String whizFlow, String service, String domain)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_domain = domain;
			_service = service;
			_taskContentId = taskContentId;
			lblDomain.Text = domain;
			lblServiceName.Text = service;
			_load = true;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">Service name</param>
		/// <param name="domain">Domain name</param>
		public TaskContentViewer(String whizFlow, String service, String domain)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_domain = domain;
			_service = service;
			lblDomain.Text = domain;
			lblServiceName.Text = service;
			_load = false;
		}
		private void TaskContentViewer_Load(object sender, EventArgs e)
		{
			if (_load)
			{
				this.Text = "TaskContentId:" + _taskContentId.ToString();
				txtId.Text = _taskContentId.ToString();
				LoadData();
			}
		}
		private void lsvProcessingInformation_DoubleClick(object sender, EventArgs e)
		{
			if (lsvProcessingInformation.SelectedItems.Count > 0)
			{
				if (lsvProcessingInformation.SelectedItems.Count > 0)
				{
					TaskLogs t = new TaskLogs(_whizFlow, _service, _domain, lsvProcessingInformation.SelectedItems[0].SubItems[4].Text, 100);
					OpenPluginForm(t);
				}
			}
		}
		private void LoadData()
		{
			Task.Run(() =>
			{
				try
				{
					WhizFlowTaskContent ptc = Query.TaskContentGet(_whizFlow, _domain, _taskContentId);
					List<TaskInformation> tis = Query.TaskInformationsGet(_whizFlow, _domain, _taskContentId);
					this.Invoke((MethodInvoker)delegate
					{
						lsvProcessingInformation.Items.Clear();
						txtContent.Text = "";
						txtContent.Text = "Timestamp : " + ptc.TimeStamp.ToString("o") + Environment.NewLine + "Content : " + Environment.NewLine + ptc.Content;
						foreach (TaskInformation ti in tis)
						{
							ListViewItem lvi = new ListViewItem();
							lvi.Text = ti.Time.ToString("o");
							lvi.SubItems.Add(ti.HostName);
							lvi.SubItems.Add(ti.Service);
							lvi.SubItems.Add(ti.Domain);
							lvi.SubItems.Add(ti.Queue);
							lvi.SubItems.Add(ti.Signature);
							lvi.SubItems.Add(ti.ProcessingTime.ToString());
							lsvProcessingInformation.Items.Add(lvi);
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
		private void lsvProcessingInformation_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
			{
				lsvProcessingInformation.Items.Clear();
				LoadData();
			}
			else if (e.KeyCode == Keys.Q)
			{
				if (lsvProcessingInformation.SelectedItems.Count > 0)
				{
					TaskLogs t = new TaskLogs(_whizFlow, _service, _domain, lsvProcessingInformation.SelectedItems[0].SubItems[4].Text, 100);
					OpenPluginForm(t);
				}
			}
			else if (e.KeyCode == Keys.L)
			{
				btnLogs_Click(this, new EventArgs());
			}
		}
		private void btnLoad_Click(object sender, EventArgs e)
		{
			try
			{
				_taskContentId = Int32.Parse(txtId.Text);
				this.Text = "TaskContentId:" + _taskContentId.ToString();
				LoadData();
			}
			catch { }
		}
		private void btnLogs_Click(object sender, EventArgs e)
		{
			try
			{
				_taskContentId = Int32.Parse(txtId.Text);
				Logs t = new Logs(_whizFlow, _service, _domain, _taskContentId, true, true, true);
				OpenPluginForm(t);
			}
			catch { }
		}
	}
}
