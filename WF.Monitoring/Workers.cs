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
	/// Form for the recap of all the workers running in a single WhizFlow domain
	/// </summary>
	public partial class Workers : PluginBaseForm
	{
		private String _whizFlow;
		private String _service;
		private String _domain;
		private List<String> _workers;
		private System.Timers.Timer _timer;
		/// <summary>
		/// Constructor
		/// </summary>
		public Workers()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="whizFlow"></param>
		/// <param name="service"></param>
		/// <param name="domain"></param>
		public Workers(String whizFlow, String service, String domain)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			var result = Query.WorkerHandlerThreadsGet(_whizFlow);
			_workers = new List<String>();
			foreach (var el in result.Where(t => t.WhizFlow == _service && t.Domain == _domain))
			{
				ListViewItem lvi = new ListViewItem();
				lvi.Text = el.WorkerName;
				lvi.SubItems.Add(el.Status);
				lvi.Name = el.WorkerName;
				lsvSchedulers.Items.Add(lvi);
			}
			_timer = new System.Timers.Timer(100000);
			_timer.Elapsed += _timer_Elapsed;
			_timer.Start();
		}
		/// <summary>
		/// Timer event handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			try
			{
				var result = Query.SchedulerHandlerThreadsGet(_whizFlow);
				foreach (var el in result.Where(t => t.WhizFlow == _service && t.Domain == _domain))
				{
					this.Invoke((MethodInvoker)delegate()
					{
						lsvSchedulers.Items[el.Scheduler].SubItems[1].Text = el.Mode;
						lsvSchedulers.Items[el.Scheduler].SubItems[2].Text = el.LastRunning.ToString();
						lsvSchedulers.Items[el.Scheduler].SubItems[3].Text = el.Status;
					});
				}
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
		private void Schedulers_FormClosing(object sender, FormClosingEventArgs e)
		{
			_timer.Stop();
			_timer.Elapsed -= _timer_Elapsed;
		}
	}
}
