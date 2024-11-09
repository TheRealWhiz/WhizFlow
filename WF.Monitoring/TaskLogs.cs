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
using Whiz.WhizFlow.Engine.Monitoring.Utilities.Tasks;

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// This form allows to see the task processing information happened
	/// </summary>
	public partial class TaskLogs : PluginBaseForm
	{
		private String _whizFlow;
		private String _service;
		private String _domain;
		private String _queue;
		private DateTime _dateFrom;
		private DateTime _dateTo;
		private Int32 _numberOfEntries;
		private String _mode;
		/// <summary>
		/// Constructor to see the last entries for a specific queue
		/// </summary>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">Service name</param>
		/// <param name="domain">Domain name</param>
		/// <param name="queue">Queue name</param>
		/// <param name="numberOfEntries">Entries to show</param>
		public TaskLogs(String whizFlow, String service, String domain, String queue, Int32 numberOfEntries)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			_queue = queue;
			_numberOfEntries = numberOfEntries;
			_mode = "processedqueue";
			lblServiceName.Text = service;
			lblQueue.Text = queue;
			lblDomain.Text = domain;
		}
		/// <summary>
		/// Constructor to see the entries in a specified timeframe in a queue
		/// </summary>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">Service name</param>
		/// <param name="domain">Domain name</param>
		/// <param name="queue">Queue name</param>
		/// <param name="from">Timeframe start</param>
		/// <param name="to">Timeframe end</param>
		public TaskLogs(String whizFlow, String service, String domain, String queue, DateTime from, DateTime to)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			_queue = queue;
			_dateFrom = from;
			_dateTo = to;
			_mode = "processedqueuetime";
			lblServiceName.Text = service;
			lblQueue.Text = queue;
			lblDomain.Text = domain;
		}
		/// <summary>
		/// Constructor to see the last entries for the whole system
		/// </summary>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">Service name</param>
		/// <param name="domain">Domain name</param>
		/// <param name="numberOfEntries">Entries to show</param>
		public TaskLogs(String whizFlow, String service, String domain, Int32 numberOfEntries)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			_numberOfEntries = numberOfEntries;
			_mode = "processed";
			lblServiceName.Text = service;
			lblQueue.Text = "All domain queues";
			lblDomain.Text = domain;
		}
		/// <summary>
		/// Constructor to see the entries in a specified timeframe for the whole system
		/// </summary>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">Service name</param>
		/// <param name="domain">Domain name</param>
		/// <param name="from">Timeframe start</param>
		/// <param name="to">Timeframe end</param>
		public TaskLogs(String whizFlow, String service, String domain, DateTime from, DateTime to)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			_dateFrom = from;
			_dateTo = to;
			_mode = "processedtime";
			lblServiceName.Text = service;
			lblQueue.Text = "All domain queues";
			lblDomain.Text = domain;
		}
		private void LoadProcessedQueue()
		{
			Task.Run(() =>
			{
				List<TaskInformation> tis = Query.TaskInformationsGetProcessed(_whizFlow, _domain, _queue, _numberOfEntries, false, false, false);
				this.Invoke((MethodInvoker)delegate()
				{
					foreach (TaskInformation p in tis)
					{
						ListViewItem lvi = new ListViewItem();
						lvi.Text = p.TaskContentId.ToString();
						lvi.SubItems.Add(p.Time.ToString("o"));
						lvi.SubItems.Add(p.HostName);
						lvi.SubItems.Add(p.Service);
						lvi.SubItems.Add(p.Domain);
						lvi.SubItems.Add(p.Queue);
						lvi.SubItems.Add(p.Signature);
						lvi.SubItems.Add(p.ProcessingTime.ToString());
						lsvProcessingInformation.Items.Add(lvi);
					}
				});
			});
		}
		private void LoadProcessedQueueTime()
		{
			Task.Run(() =>
			{
				List<TaskInformation> tis = Query.TaskInformationsGetProcessed(_whizFlow, _domain, _queue, _dateFrom, _dateTo, false, false, false);
				this.Invoke((MethodInvoker)delegate()
				{
					foreach (TaskInformation p in tis)
					{
						ListViewItem lvi = new ListViewItem();
						lvi.Text = p.TaskContentId.ToString();
						lvi.SubItems.Add(p.Time.ToString("o"));
						lvi.SubItems.Add(p.HostName);
						lvi.SubItems.Add(p.Service);
						lvi.SubItems.Add(p.Domain);
						lvi.SubItems.Add(p.Queue);
						lvi.SubItems.Add(p.Signature);
						lvi.SubItems.Add(p.ProcessingTime.ToString());
						lsvProcessingInformation.Items.Add(lvi);
					}
				});
			});
		}
		private void LoadProcessed()
		{
			Task.Run(() =>
			{
				List<TaskInformation> tis = Query.TaskInformationsGetProcessed(_whizFlow, _domain, _numberOfEntries, false, false, false);
				this.Invoke((MethodInvoker)delegate()
				{
					foreach (TaskInformation p in tis)
					{
						ListViewItem lvi = new ListViewItem();
						lvi.Text = p.TaskContentId.ToString();
						lvi.SubItems.Add(p.Time.ToString("o"));
						lvi.SubItems.Add(p.HostName);
						lvi.SubItems.Add(p.Service);
						lvi.SubItems.Add(p.Domain);
						lvi.SubItems.Add(p.Queue);
						lvi.SubItems.Add(p.Signature);
						lvi.SubItems.Add(p.ProcessingTime.ToString());
						lsvProcessingInformation.Items.Add(lvi);
					}
				});
			});
		}
		private void LoadProcessedTime()
		{
			Task.Run(() =>
			{
				List<TaskInformation> tis = Query.TaskInformationsGetProcessed(_whizFlow, _domain, _dateFrom, _dateTo, false, false, false);
				this.Invoke((MethodInvoker)delegate()
				{
					foreach (TaskInformation p in tis)
					{
						ListViewItem lvi = new ListViewItem();
						lvi.Text = p.TaskContentId.ToString();
						lvi.SubItems.Add(p.Time.ToString("o"));
						lvi.SubItems.Add(p.HostName);
						lvi.SubItems.Add(p.Service);
						lvi.SubItems.Add(p.Domain);
						lvi.SubItems.Add(p.Queue);
						lvi.SubItems.Add(p.Signature);
						lvi.SubItems.Add(p.ProcessingTime.ToString());
						lsvProcessingInformation.Items.Add(lvi);
					}
				});
			});
		}
		private void lsvProcessingInformation_DoubleClick(object sender, EventArgs e)
		{
			if (lsvProcessingInformation.SelectedItems.Count > 0)
			{
				TaskContentViewer t = new TaskContentViewer(Int32.Parse(lsvProcessingInformation.SelectedItems[0].SubItems[0].Text), _whizFlow, _service, _domain);
				OpenPluginForm(t);
			}
		}
		private void lsvProcessingInformation_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
			{
				lsvProcessingInformation.Items.Clear();
				try
				{
					switch (_mode)
					{
						case "processedqueue":
							LoadProcessedQueue();
							break;
						case "processedqueuetime":
							LoadProcessedQueueTime();
							break;
						case "processed":
							LoadProcessed();
							break;
						case "processedtime":
							LoadProcessedTime();
							break;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					this.Close();
				}
			}
			else if (e.KeyCode == Keys.Q)
			{
				if (lsvProcessingInformation.SelectedItems.Count > 0)
				{
					TaskLogs t = new TaskLogs(_whizFlow, _service, _domain, lsvProcessingInformation.SelectedItems[0].SubItems[5].Text, 100);
					OpenPluginForm(t);
				}
			}
			else if (e.KeyCode == Keys.F)
			{
				if (lsvProcessingInformation.SelectedItems.Count > 0)
				{
					TaskContentViewer t = new TaskContentViewer(Int32.Parse(lsvProcessingInformation.SelectedItems[0].SubItems[0].Text), _whizFlow, _service, _domain);
					OpenPluginForm(t);
				}
			}
		}
		private void TaskLogs_Load(object sender, EventArgs e)
		{
			try
			{
				switch (_mode)
				{
					case "processedqueue":
						LoadProcessedQueue();
						break;
					case "processedqueuetime":
						LoadProcessedQueueTime();
						break;
					case "processed":
						LoadProcessed();
						break;
					case "processedtime":
						LoadProcessedTime();
						break;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Close();
			}
		}

	}
}
