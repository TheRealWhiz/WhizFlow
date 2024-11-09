using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Whiz.WhizFlow.Tools.WhizFlowSimulator
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[MTAThread]
		static void Main(String[] args)
		{
			String config = "";
			String serviceName = "";
			if (args.Length > 0)
			{
				config = args[0];
				serviceName = args[1];
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try
			{
				Application.Run(new WhizFlowSimulator(config, serviceName));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
