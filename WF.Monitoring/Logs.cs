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
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Logs;

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// Logs viewer form
	/// </summary>
	public partial class Logs : PluginBaseForm
	{
		private String _whizFlow;
		private String _service;
		private String _domain;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="whizFlow">The whizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">The service name</param>
		/// <param name="domain">The domain of the service to query</param>
		public Logs(String whizFlow, String service, String domain)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			radLastEntries.Checked = true;
		}
		/// <summary>
		/// Constructor with taskContentId filter preselected
		/// </summary>
		/// <param name="whizFlow">The whizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">The service name</param>
		/// <param name="domain">The domain of the service to query</param>
		/// <param name="taskContentId">Task content Id filter</param>
		/// <param name="allDomains">Extract for all domains</param>
		/// <param name="allHosts">Extract for all hosts</param>
		/// <param name="allServices">Extract for all services</param>
		public Logs(String whizFlow, String service, String domain, Int32 taskContentId, Boolean allHosts, Boolean allServices, Boolean allDomains)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			txtFilterTaskContentId.Text = taskContentId.ToString();
			radTaskContentId.Checked = true;
			grpTaskContentId.Enabled = true;
			chkAllDomains.Checked = allDomains;
			chkAllServices.Checked = allServices;
			chkAllHosts.Checked = allHosts;
		}
		private void Logs_Load(object sender, EventArgs e)
		{
			cmbLogType.Items.Add("Information");
			cmbLogType.Items.Add("Warning");
			cmbLogType.Items.Add("Error");
			cmbLogType.Items.Add("OperationLog");
			cmbLogType.Items.Add("ModuleSpecificLog");
			cmbLogType.Items.Add("All");
			cmbModule.Items.Add("ManagerBase");
			cmbModule.Items.Add("ManagerPlugin");
			cmbModule.Items.Add("ControllerBase");
			cmbModule.Items.Add("ControllerPlugin");
			cmbModule.Items.Add("QueuesHandler");
			cmbModule.Items.Add("QueueHandlerThread");
			cmbModule.Items.Add("QueueProcessor");
			cmbModule.Items.Add("SchedulersHandler");
			cmbModule.Items.Add("SchedulerHandlerThread");
			cmbModule.Items.Add("SchedulerProcessor");
			cmbModule.Items.Add("SchedulerControllerBase");
			cmbModule.Items.Add("SchedulerControllerPlugin");
			cmbModule.Items.Add("AppDomainManager");
			cmbModule.Items.Add("WhizFlowManager");
			cmbModule.Items.Add("WorkersHandler");
			cmbModule.Items.Add("WorkerHandlerThread");
			cmbModule.Items.Add("WorkerProcessor");
			cmbModule.Items.Add("WorkerControllerPlugin");
			cmbModule.Items.Add("All");
			cmbLogType.SelectedIndex = cmbLogType.Items.Count - 1;
			cmbModule.SelectedIndex = cmbModule.Items.Count - 1;
		}
		private void btnFromNow_Click(object sender, EventArgs e)
		{
			txtFrom.Text = DateTime.Now.ToString("o");
		}
		private void btnToNow_Click(object sender, EventArgs e)
		{
			txtTo.Text = DateTime.Now.ToString("o");
		}
		private void PopulateGrid(List<LogEntry> logs)
		{
			lsvLogs.Items.Clear();
			foreach (LogEntry l in logs)
			{
				ListViewItem i = new ListViewItem();
				i.Text = l.Time.ToString("o");
				i.SubItems.Add(l.HostName);
				i.SubItems.Add(l.Service);
				i.SubItems.Add(l.Domain);
				i.SubItems.Add(l.Module.ToString());
				i.SubItems.Add(l.LogType.ToString());
				i.SubItems.Add(l.TaskContentId.ToString());
				i.SubItems.Add(l.Object);
				i.SubItems.Add(l.Message);
				i.SubItems.Add(l.AdditionalInformation);
				lsvLogs.Items.Add(i);
			}
		}
		private void btnGetLogs_Click(object sender, EventArgs e)
		{
			Boolean blnAllDomains;
			Boolean blnAllHosts;
			Boolean blnAllServices;
			Int32 entries = 0;
			Int32 taskContentId = 0;
			DateTime from = DateTime.MinValue;
			DateTime to = DateTime.MaxValue;
			Modules module;
			LogTypes logType;
			Boolean filter_timeframe;
			Boolean filter_taskId;
			if (radLastEntries.Checked)
			{
				if (!Int32.TryParse(txtEntries.Text, out entries)) return;
				filter_timeframe = false;
				filter_taskId = false;
			}
			else if (radTaskContentId.Checked)
			{
				if (!Int32.TryParse(txtFilterTaskContentId.Text, out taskContentId)) return;
				filter_timeframe = false;
				filter_taskId = true;
			}
			else
			{
				if (!(DateTime.TryParse(txtFrom.Text, out from) && DateTime.TryParse(txtTo.Text, out to))) return;
				filter_timeframe = true;
				filter_taskId = false;
			}
			blnAllDomains = chkAllDomains.Checked;
			blnAllHosts = chkAllHosts.Checked;
			blnAllServices = chkAllServices.Checked;
			module = (Modules)cmbModule.SelectedIndex;
			logType = (LogTypes)cmbLogType.SelectedIndex;
			Task.Run(() =>
				{
					try
					{
						List<LogEntry> l;
						if (filter_timeframe)
						{
							l = Query.LogsGet2(_whizFlow, _domain, from, to, module, logType, blnAllHosts, blnAllServices, blnAllDomains);
						}
						else if (filter_taskId)
						{
							l = Query.LogsGet3(_whizFlow, _domain, taskContentId, module, logType, blnAllHosts, blnAllServices, blnAllDomains);
						}
						else
						{
							l = Query.LogsGet1(_whizFlow, _domain, entries, module, logType, blnAllHosts, blnAllServices, blnAllDomains);
						}
						this.Invoke((MethodInvoker)delegate { PopulateGrid(l); });
					}
					catch
					{ }
				});
		}
		private void btnGetTaskInfo_Click(object sender, EventArgs e)
		{
			Int32 id;
			if (Int32.TryParse(txtTaskContentId.Text, out id))
			{
				TaskContentViewer t = new TaskContentViewer(id, _whizFlow, _service, _domain);
				OpenPluginForm(t);
			}
		}
		private void lsvLogs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lsvLogs.SelectedItems.Count > 0)
			{
				txtMessage.Text = lsvLogs.SelectedItems[0].SubItems[8].Text;
				txtAdditionalInformation.Text = lsvLogs.SelectedItems[0].SubItems[9].Text;
				txtTaskContentId.Text = lsvLogs.SelectedItems[0].SubItems[6].Text;
				if (txtTaskContentId.Text != "") { btnGetTaskInfo.Enabled = true; }
			}
			else
			{
				txtTaskContentId.Text = "";
				txtAdditionalInformation.Text = "";
				txtMessage.Text = "";
				btnGetTaskInfo.Enabled = false;
			}
		}
		private void lsvLogs_DoubleClick(object sender, EventArgs e)
		{
			if (lsvLogs.SelectedItems.Count > 0)
			{
				Int32 p;
				if (Int32.TryParse(txtTaskContentId.Text, out p))
				{
					TaskContentViewer t = new TaskContentViewer(p, _whizFlow, _service, _domain);
					OpenPluginForm(t);
				}
			}
		}
		private void radLastEntries_CheckedChanged(object sender, EventArgs e)
		{
			if (radLastEntries.Checked == true)
			{
				radTimeframe.Checked = false;
				radTaskContentId.Checked = false;
				grpTimeFrame.Enabled = false;
				grpLast.Enabled = true;
				grpTaskContentId.Enabled = false;
			}
		}
		private void radTimeframe_CheckedChanged(object sender, EventArgs e)
		{
			if (radTimeframe.Checked == true)
			{
				radLastEntries.Checked = false;
				radTaskContentId.Checked = false;
				grpTimeFrame.Enabled = true;
				grpLast.Enabled = false;
				grpTaskContentId.Enabled = false;
			}
		}
		private void radTaskContentId_CheckedChanged(object sender, EventArgs e)
		{
			if (radTaskContentId.Checked == true)
			{
				radLastEntries.Checked = false;
				radTimeframe.Checked = false;
				grpTimeFrame.Enabled = false;
				grpLast.Enabled = false;
				grpTaskContentId.Enabled = true;
			}
		}
	}
}
