﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WhizFlowTestPlugins
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try
			{
				Application.Run(new Enqueuer());
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
