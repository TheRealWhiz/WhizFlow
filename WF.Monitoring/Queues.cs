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
	/// Form for the recap of all the queue running in a single WhizFlow domain
	/// </summary>
	public partial class Queues : PluginBaseForm
	{
		private String _whizFlow;
		private String _service;
		private String _domain;
		private List<String> _queues;
		private System.Timers.Timer _timer;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">Service name</param>
		/// <param name="domain">Domain name</param>
		public Queues(String whizFlow, String service, String domain)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			QueueQueryOrchestrator.AddHost(whizFlow);
			QueueQueryOrchestrator.Update += QueueQueryOrchestrator_Update;
			var result = Query.QueueHandlerThreadsGet(_whizFlow);
			_queues = new List<String>();
			foreach (var el in result.Where(t => t.WhizFlow == _service && t.Domain == _domain))
			{
				ListViewItem lvi = new ListViewItem();
				lvi.Text = el.Queue;
				lvi.SubItems.Add(el.ItemsInQueue.ToString());
				lvi.SubItems.Add("0");
				lvi.SubItems.Add("0");
				lvi.SubItems.Add("0");
				lvi.Name = el.Queue;
				lsvQueues.Items.Add(lvi);
				_queues.Add(el.Queue);
			}
			_timer = new System.Timers.Timer(10000);
			_timer.Elapsed += _timer_Elapsed;
			//_timer.Start();
		}
		void QueueQueryOrchestrator_Update()
		{
			try
			{
				var result = QueueQueryOrchestrator.GetQueueHandlerThreads(_whizFlow);
				foreach (var el in result.Where(t => t.WhizFlow == _service && t.Domain == _domain))
				{
					this.Invoke((MethodInvoker)delegate()
					{
						lsvQueues.Items[el.Queue].SubItems[1].Text = el.ItemsInQueue.ToString();
					});
				}
				foreach (String queue in _queues)
				{
					var processed = Query.QueueProcessedTasksGet(_whizFlow, _domain, queue);
					this.Invoke((MethodInvoker)delegate()
					{
						lsvQueues.Items[queue].SubItems[4].Text = processed.ToString();
					});
					var throughput = Query.QueueProcessedTasksPerSecondGet(_whizFlow, _domain, queue);
					this.Invoke((MethodInvoker)delegate()
					{
						lsvQueues.Items[queue].SubItems[2].Text = throughput.ToString();
						if (float.Parse(lsvQueues.Items[queue].SubItems[3].Text) < throughput)
						{
							lsvQueues.Items[queue].SubItems[3].Text = throughput.ToString();
						}
					});
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			try
			{
				var result = Query.QueueHandlerThreadsGet(_whizFlow);
				foreach (var el in result.Where(t => t.WhizFlow == _service && t.Domain == _domain))
				{
					this.Invoke((MethodInvoker)delegate()
					{
						lsvQueues.Items[el.Queue].SubItems[1].Text = el.ItemsInQueue.ToString();
					});
				}
				foreach (String queue in _queues)
				{
					var processed = Query.QueueProcessedTasksGet(_whizFlow, _domain, queue);
					this.Invoke((MethodInvoker)delegate()
					{
						lsvQueues.Items[queue].SubItems[4].Text = processed.ToString();
					});
					var throughput = Query.QueueProcessedTasksPerSecondGet(_whizFlow, _domain, queue);
					this.Invoke((MethodInvoker)delegate()
					{
						lsvQueues.Items[queue].SubItems[2].Text = throughput.ToString();
						if (float.Parse(lsvQueues.Items[queue].SubItems[3].Text) < throughput)
						{
							lsvQueues.Items[queue].SubItems[3].Text = throughput.ToString();
						}
					});
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void Queues_FormClosing(object sender, FormClosingEventArgs e)
		{
			_timer.Stop();
			_timer.Elapsed -= _timer_Elapsed;
			QueueQueryOrchestrator.Update -= QueueQueryOrchestrator_Update;
		}
		private void lsvQueues_DoubleClick(object sender, EventArgs e)
		{
			if (lsvQueues.SelectedItems.Count > 0)
			{
				Queue q = new Queue(_whizFlow, _service, _domain, lsvQueues.SelectedItems[0].Text);
				OpenPluginForm(q);
			}
		}
	}
}
