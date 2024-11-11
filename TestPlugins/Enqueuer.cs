using Whiz.WhizFlow;
using Whiz.WhizFlow.Common.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhizFlowTestPlugins
{
	public partial class Enqueuer : Form
	{
		public Enqueuer()
		{
			InitializeComponent();
		}

		private void btnEnqueue_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				String connection = "";
				DAL.SetupDedicatedQueue("CodaDedicata1", connection);
				for (Int32 n = 0; n < Int32.Parse(txtItems.Text); n++)
				{
					WhizFlowTaskContent tc = new WhizFlowTaskContent();
					tc.Content = DateTime.Now.ToString() + " " + n.ToString() + Guid.NewGuid().ToString();
					tc.TimeStamp = DateTime.Now;
					try
					{
						tc.Id = DAL.SaveTaskContent(tc, connection);
						WhizFlowTask t = new WhizFlowTask(tc);
						t.Signature = "1";
						DAL.SaveTask(t, "Coda1", connection);
						DAL.SaveTask(t, "Coda2", connection);
						DAL.SaveTask(t, "Coda3", connection);
						DAL.SaveTask(t, "Coda4", connection);
						DAL.SaveTaskDedicated(t, "CodaDedicata1", connection);
						this.Invoke((MethodInvoker)delegate
						{
							lblActualItem.Text = n.ToString();
						});
					}
					catch //(Exception ex)
					{ }
				}
			});
		}
	}
}
