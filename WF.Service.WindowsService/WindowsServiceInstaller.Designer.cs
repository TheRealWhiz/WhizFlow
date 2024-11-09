namespace Whiz.WhizFlow.Service
{
	partial class WindowsServiceInstaller
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.serviceManager = new System.ServiceProcess.ServiceInstaller();
			this.serviceManagerInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			// 
			// serviceManager
			// 
			this.serviceManager.ServiceName = "TBR";
			this.serviceManager.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// serviceManagerInstaller
			// 
			this.serviceManagerInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
			this.serviceManagerInstaller.Password = null;
			this.serviceManagerInstaller.Username = null;
			// 
			// WindowsServiceInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceManagerInstaller,
            this.serviceManager});

		}

		#endregion

		private System.ServiceProcess.ServiceInstaller serviceManager;
		private System.ServiceProcess.ServiceProcessInstaller serviceManagerInstaller;
	}
}
