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
	/// Form for the recap of all the schedulers running in a single WhizFlow\ domain
	/// </summary>
	public partial class Schedulers : PluginBaseForm
	{
		private String _whizFlow;
		private String _service;
		private String _domain;
		private List<String> _schedulers;
		private System.Timers.Timer _timer;
		/// <summary>
		/// Contructor
		/// </summary>
		public Schedulers()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="whizFlow"></param>
		/// <param name="service"></param>
		/// <param name="domain"></param>
		public Schedulers(String whizFlow, String service, String domain)
		{
			InitializeComponent();
			_whizFlow = whizFlow;
			_service = service;
			_domain = domain;
			var result = Query.SchedulerHandlerThreadsGet(_whizFlow);
			_schedulers = new List<String>();
			foreach (var el in result.Where(t => t.WhizFlow == _service && t.Domain == _domain))
			{
				ListViewItem lvi = new ListViewItem();
				lvi.Text = el.Scheduler;
				lvi.SubItems.Add(el.Mode);
				lvi.SubItems.Add(el.LastRunning);
				lvi.SubItems.Add(el.Status);
				lvi.Name = el.Scheduler;
				lsvSchedulers.Items.Add(lvi);
			}
			_timer = new System.Timers.Timer(10000);
			_timer.Elapsed += _timer_Elapsed;
			_timer.Start();
		}
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
		private void Schedulers_FormClosing(object sender, FormClosingEventArgs e)
		{
			_timer.Stop();
			_timer.Elapsed -= _timer_Elapsed;
		}
		private void btnStart_Click(object sender, EventArgs e)
		{
			if (lsvSchedulers.SelectedItems.Count > 0)
			{
				Query.DomainSchedulerActivate(_whizFlow, _domain, lsvSchedulers.SelectedItems[0].Text);
			}
		}
		private void btnStop_Click(object sender, EventArgs e)
		{
			if (lsvSchedulers.SelectedItems.Count > 0)
			{
				Query.DomainSchedulerDeactivate(_whizFlow, _domain, lsvSchedulers.SelectedItems[0].Text);
			}
		}
		private void btnChangeRecurrence_Click(object sender, EventArgs e)
		{
			if (lsvSchedulers.SelectedItems.Count > 0)
			{
				Int32 ms;
				if (Int32.TryParse(txtRecurrence.Text, out ms))
				{
					Query.DomainSchedulerChangeRecurrence(_whizFlow, _domain, lsvSchedulers.SelectedItems[0].Text, ms);
				}
			}
		}
		private void btnResetRecurrence_Click(object sender, EventArgs e)
		{
			if (lsvSchedulers.SelectedItems.Count > 0)
			{
				Query.DomainSchedulerRestoreRecurrence(_whizFlow, _domain, lsvSchedulers.SelectedItems[0].Text);
			}
		}
	}
}
