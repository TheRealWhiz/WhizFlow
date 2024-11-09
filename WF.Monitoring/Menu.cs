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
	/// Start point for the WhizFlow monitoring which allows to write a WF http endpoint and to start the monitoring panel
	/// </summary>
	public partial class Menu : PluginBaseForm
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Menu()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Opens the monitor for the specified endpoint
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOpenMonitor_Click(object sender, EventArgs e)
		{
			Uri uriResult;
			Boolean result = Uri.TryCreate(txtWFHost.Text, UriKind.Absolute, out uriResult)
					&& uriResult.Scheme == Uri.UriSchemeHttp;
			if (result)
			{
				WhizFlowMonitor p = new WhizFlowMonitor(txtWFHost.Text);
				OpenPluginForm(p);
			}
			else
			{
				MessageBox.Show("The specified endpoint is not a valid url (e.g: http://{address}:{port})", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}
}
