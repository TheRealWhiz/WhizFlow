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

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// Queue status viewer
	/// </summary>
	public partial class Queue : PluginBaseForm
	{
		private String _whizFlow;
		private String _service;
		private String _domain;
		private String _queue;
		private Query.QueueHandlerThread _queueData;
		private System.Timers.Timer _timer = new System.Timers.Timer();
		/// <summary>
		///	Gets or sets the status of the auto refresh
		/// </summary>
		public Boolean RefreshDisabled { get; set; }
		/// <summary>
		/// Gets information about the error state of a queue
		/// </summary>
		public Boolean Error { get; private set; }
		/// <summary>
		/// Gets information about items in the queue at this time
		/// </summary>
		public Int32 QueueItems { get; private set; }
		/// <summary>
		/// Init the Queue viewer and all the relative subsystems
		/// </summary>
		public void Init()
		{
			QueueQueryOrchestrator.AddHost(_whizFlow);
			QueueQueryOrchestrator.Update += QueueQueryOrchestrator_Update;
			prbQueue.Maximum = 1;
			RefreshDisabled = false;
		}
		private void QueueQueryOrchestrator_Update()
		{
			_queueData = QueueQueryOrchestrator.GetQueueHandlerThreads(_whizFlow).First(q => q.Queue == _queue && q.Domain == _domain && q.WhizFlow == _service);
		}
		/// <summary>
		/// Refresh the information of the queue monitored
		/// </summary>
		public void RefreshMonitor()
		{
			try
			{
				this.Invoke((MethodInvoker)delegate
					{
						lblError.Visible = false;
					});
				//Query.QueueHandlerThread q= Query.QueueHandlerThreadGet
				if (_queueData == null)
				{
					this.Invoke((MethodInvoker)delegate
					{
						lblError.Visible = true;
						lblError.Text = "Unable to retrieve informations";
						RefreshDisabled = true;
						Error = true;
					});
				}
				/*else if (pQueue.Count == 0)
				{
					lblError.Visible = true;
					lblError.Text = "Unable to retrieve " + _queue + " queue";
					RefreshDisabled = true;
					Error = true;
				}*/
				else
				{
					float processedPerSecond = Query.QueueProcessedTasksPerSecondGet(_whizFlow, _domain, _queue);
					float processed = Query.QueueProcessedTasksGet(_whizFlow, _domain, _queue);
					this.Invoke((MethodInvoker)delegate
					{
						lblInQueue.Text = _queueData.ItemsInQueue.ToString() + " in queue";
						lblProcessed.Text = ((Int32)processed).ToString() + " processed";
						if (prbQueue.Maximum < (Int32)(processedPerSecond * 10)) prbQueue.Maximum = (Int32)((processedPerSecond + 1) * 10);
						prbQueue.Value = (Int32)(processedPerSecond * 10);
						lblActual.Text = "now: " + processedPerSecond.ToString() + " , peak: " + (Int32.Parse(prbQueue.Maximum.ToString()) / 10).ToString();
						QueueItems = _queueData.ItemsInQueue;
						if (QueueItems > 1500)
						{
							lblColorIndicator.BackColor = Color.Red;
							lblInQueue.BackColor = Color.Red;
							lblProcessed.BackColor = Color.Red;
							lblActual.BackColor = Color.Red;
						}
						else if (QueueItems > 1000)
						{
							lblColorIndicator.BackColor = Color.Orange;
							lblInQueue.BackColor = Color.Orange;
							lblProcessed.BackColor = Color.Orange;
							lblActual.BackColor = Color.Orange;
						}
						else if (QueueItems > 500)
						{
							lblColorIndicator.BackColor = Color.Yellow;
							lblInQueue.BackColor = Color.Yellow;
							lblProcessed.BackColor = Color.Yellow;
							lblActual.BackColor = Color.Yellow;
						}
						else if (QueueItems > 150)
						{
							lblColorIndicator.BackColor = Color.YellowGreen;
							lblInQueue.BackColor = Color.YellowGreen;
							lblProcessed.BackColor = Color.YellowGreen;
							lblActual.BackColor = Color.YellowGreen;
						}
						else
						{
							lblColorIndicator.BackColor = Color.Lime;
							lblInQueue.BackColor = Color.Lime;
							lblProcessed.BackColor = Color.Lime;
							lblActual.BackColor = Color.Lime;
						}
					});
				}
				//if (this.WindowState == FormWindowState.Minimized && (Error || QueueItems >= 500)) this.WindowState = FormWindowState.Normal;
			}
			catch (Exception ex)
			{
				this.Invoke((MethodInvoker)delegate
				{
					lblError.Visible = true;
					lblError.Text = ex.Message;
					RefreshDisabled = true;
					Error = true;
				});
			}
			finally { if (!Error) _timer.Start(); }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="whizFlow">WhizFlow host in the form http://{address}:{port}</param>
		/// <param name="service">Service name</param>
		/// <param name="domain">Domain that hosts the queue to monitor</param>
		/// <param name="queue">Queue to monitor</param>
		public Queue(String whizFlow, String service, String domain, String queue)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			_queue = queue;
		}
		private void QueueViewer_Load(object sender, EventArgs e)
		{
			Init();
			this.Text = _service + ":" + _domain + ":" + _queue;
			toolTip.SetToolTip(this.lblActual, _service + ":" + _domain + ":" + _queue);
			toolTip.SetToolTip(this.lblColorIndicator, _service + ":" + _domain + ":" + _queue);
			toolTip.SetToolTip(this.lblInQueue, _service + ":" + _domain + ":" + _queue);
			toolTip.SetToolTip(this.lblProcessed, _service + ":" + _domain + ":" + _queue);
			toolTip.SetToolTip(this.lblError, _service + ":" + _domain + ":" + _queue);
			toolTip.SetToolTip(this.prbQueue, _service + ":" + _domain + ":" + _queue);
			toolTip.SetToolTip(this, _service + ":" + _domain + ":" + _queue);
			_timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
			_timer.Interval = 2000;
			_timer.Start();
		}
		private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			_timer.Stop();
			if (!RefreshDisabled)
			{
				Task.Factory.StartNew(() =>
				{ RefreshMonitor(); });
			}
		}
		private void QueueViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				_timer.Stop();
				_timer.Elapsed -= new System.Timers.ElapsedEventHandler(_timer_Elapsed);
			}
			catch
			{
			}
		}
		private void lblError_DoubleClick(object sender, EventArgs e)
		{
			Error = false;
			RefreshDisabled = false;
			Task.Factory.StartNew(() =>
			{ RefreshMonitor(); });
		}
	}
}
