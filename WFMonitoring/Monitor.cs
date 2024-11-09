using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Deltatre.Pipeline.Engine.Monitoring.Utilities.Feeds.FE;
using Deltatre.Pipeline.Engine.Monitoring.Utilities.WMI.FE;
using Deltatre.Pipeline.Engine.Monitoring.Utilities.Logs.FE;
using Deltatre.Pipeline.Engine.Monitoring.Utilities.PerformanceCounters.FE;
using Deltatre.Pipeline.Engine.Monitoring.Utilities;
using Deltatre.Pipeline.Engine.Monitoring.Utilities.WMI;
using Deltatre.Pipeline.Engine.Monitoring.Utilities.Feeds;
using Deltatre.Pipeline.Engine.Monitoring.Utilities.Logs;

namespace Deltatre.Pipeline.Tools.Monitoring
{
	public partial class Monitor : Form
	{
		public Monitor()
		{
			InitializeComponent();
		}

		private void btnQuery_Click(object sender, EventArgs e)
		{
			try
			{
				String[] pTemp = WMI.GetPipelineInstances(txtMachineName.Text, "http://localhost/PipelineWebServices/");
				lsbPipelines.Items.Clear();
				foreach (String p in pTemp)
				{
					lsbPipelines.Items.Add(p);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void cmbObjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbObjects.SelectedIndex > -1)
			{
				Entities pType = (Entities)cmbObjects.SelectedIndex;
				Dictionary<String, Dictionary<String, String>> pTemp = WMI.GetObjects(pType, lsbPipelines.Items[lsbPipelines.SelectedIndex].ToString(), txtMachineName.Text, "http://localhost/PipelineWebServices/");
				lsbObjects.Items.Clear();
				lsvProperties.Items.Clear();
				foreach (KeyValuePair<String, Dictionary<String, String>> pItem in pTemp)
				{
					lsbObjects.Items.Add(pItem.Key);
				}
				lsbObjects.Tag = pTemp;
			}
		}

		private void lsbPipelines_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmbObjects.SelectedIndex = -1;
			lsbObjects.Items.Clear();
		}

		private void lsbObjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			lsvProperties.Items.Clear();
			if (lsbObjects.SelectedIndex > -1)
			{
				Dictionary<String, String> pT = ((Dictionary<String, Dictionary<String, String>>)lsbObjects.Tag)[lsbObjects.SelectedItem.ToString()];

				foreach (KeyValuePair<String, String> pItem in pT)
				{
					ListViewItem pLVI = new ListViewItem();
					pLVI.Text = pItem.Key;
					pLVI.SubItems.Add(pItem.Value);
					lsvProperties.Items.Add(pLVI);
				}
			}

		}

		private void button1_Click(object sender, EventArgs e)
		{
			//List<LogEntry> pL = Logs.GetLogs(-2147469529, Module.All, LogTypes.All, null, @"Data Source=si-syap-03\mssql2008;Initial Catalog=Pipeline;Integrated Security=True");
			List<LogEntry> pL = Logs.GetLogs(-2147469529, Module.All, LogTypes.All, null, "ORP_CommonEngine", "http://localhost/PipelineWebServices/");

			pL = Deltatre.Pipeline.Engine.Monitoring.Utilities.Logs.BE.Logs.GetLogs(100, Module.All, LogTypes.All, null, @"Data Source=si-syap-03\mssql2008;Initial Catalog=Pipeline;Integrated Security=True");
			//List<FeedInformation> pI = Feeds.GetFeedInformation(-2147469529, @"Data Source=si-syap-03\mssql2008;Initial Catalog=Pipeline;Integrated Security=True");
			List<FeedInformation> pI = Feeds.GetFeedInformation(-2147469529, "ORP_CommonEngine", "http://localhost/PipelineWebServices/");
			Console.Write("assaas");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			MessageBox.Show(Deltatre.Pipeline.Engine.Monitoring.Utilities.PerformanceCounters.BE.PerformanceCounters.GetQueueProcessedFeedsPerSecond("si-syap-02", "ORP_CommonEngine", "LOW").ToString());

			var p = Deltatre.Pipeline.Engine.Monitoring.Utilities.Feeds.BE.Feeds.GetQueueProcessed(10, "LOW", @"Data Source=si-syap-03\mssql2008;Initial Catalog=Pipeline;Integrated Security=True");
			//MessageBox.Show(PerformanceCounters.GetQueueProcessedFeedsPerSecond("si-syap-02", "ORP_CommonEngine", "SCHEDULE", "http://localhost/PipelineWebServices/").ToString());
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Dictionary<String, String> pT = WMI.GetObject(Entities.PipeHandlerThread, "ORP_CommonEngine", "si-syap-02", "LOW", "http://localhost/PipelineWebServices/");
			MessageBox.Show("done");
		}

	}
}
