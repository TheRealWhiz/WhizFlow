namespace Whiz.WhizFlow.Monitoring
{
	partial class WhizFlowMonitor
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lsvDomains = new System.Windows.Forms.ListView();
			this.colDomain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnQueues = new System.Windows.Forms.Button();
			this.btnSchedulers = new System.Windows.Forms.Button();
			this.btnLogs = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnTaskLogs = new System.Windows.Forms.Button();
			this.btnConfig = new System.Windows.Forms.Button();
			this.lblServer = new System.Windows.Forms.Label();
			this.lblServiceName = new System.Windows.Forms.Label();
			this.btnTaskView = new System.Windows.Forms.Button();
			this.btnWorkers = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lsvDomains
			// 
			this.lsvDomains.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lsvDomains.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDomain,
            this.colStatus});
			this.lsvDomains.FullRowSelect = true;
			this.lsvDomains.GridLines = true;
			this.lsvDomains.HideSelection = false;
			this.lsvDomains.Location = new System.Drawing.Point(12, 103);
			this.lsvDomains.MultiSelect = false;
			this.lsvDomains.Name = "lsvDomains";
			this.lsvDomains.Size = new System.Drawing.Size(318, 127);
			this.lsvDomains.TabIndex = 0;
			this.lsvDomains.UseCompatibleStateImageBehavior = false;
			this.lsvDomains.View = System.Windows.Forms.View.Details;
			// 
			// colDomain
			// 
			this.colDomain.Text = "Domain";
			this.colDomain.Width = 230;
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			// 
			// btnQueues
			// 
			this.btnQueues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnQueues.Location = new System.Drawing.Point(12, 265);
			this.btnQueues.Name = "btnQueues";
			this.btnQueues.Size = new System.Drawing.Size(101, 23);
			this.btnQueues.TabIndex = 1;
			this.btnQueues.Text = "Queues";
			this.btnQueues.UseVisualStyleBackColor = true;
			this.btnQueues.Click += new System.EventHandler(this.btnQueues_Click);
			// 
			// btnSchedulers
			// 
			this.btnSchedulers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSchedulers.Location = new System.Drawing.Point(119, 265);
			this.btnSchedulers.Name = "btnSchedulers";
			this.btnSchedulers.Size = new System.Drawing.Size(105, 23);
			this.btnSchedulers.TabIndex = 2;
			this.btnSchedulers.Text = "Schedulers";
			this.btnSchedulers.UseVisualStyleBackColor = true;
			this.btnSchedulers.Click += new System.EventHandler(this.btnSchedulers_Click);
			// 
			// btnLogs
			// 
			this.btnLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLogs.Location = new System.Drawing.Point(13, 294);
			this.btnLogs.Name = "btnLogs";
			this.btnLogs.Size = new System.Drawing.Size(100, 23);
			this.btnLogs.TabIndex = 3;
			this.btnLogs.Text = "Logs";
			this.btnLogs.UseVisualStyleBackColor = true;
			this.btnLogs.Click += new System.EventHandler(this.btnLogs_Click);
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnStart.Location = new System.Drawing.Point(13, 236);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(155, 23);
			this.btnStart.TabIndex = 4;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnStop.Location = new System.Drawing.Point(174, 236);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(156, 23);
			this.btnStop.TabIndex = 5;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnTaskLogs
			// 
			this.btnTaskLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnTaskLogs.Location = new System.Drawing.Point(119, 294);
			this.btnTaskLogs.Name = "btnTaskLogs";
			this.btnTaskLogs.Size = new System.Drawing.Size(105, 23);
			this.btnTaskLogs.TabIndex = 7;
			this.btnTaskLogs.Text = "Task Logs";
			this.btnTaskLogs.UseVisualStyleBackColor = true;
			this.btnTaskLogs.Click += new System.EventHandler(this.btnTaskLogs_Click);
			// 
			// btnConfig
			// 
			this.btnConfig.Location = new System.Drawing.Point(12, 71);
			this.btnConfig.Name = "btnConfig";
			this.btnConfig.Size = new System.Drawing.Size(318, 23);
			this.btnConfig.TabIndex = 8;
			this.btnConfig.Text = "Configs";
			this.btnConfig.UseVisualStyleBackColor = true;
			this.btnConfig.Click += new System.EventHandler(this.btnConfigs_Click);
			// 
			// lblServer
			// 
			this.lblServer.AutoSize = true;
			this.lblServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblServer.Location = new System.Drawing.Point(9, 9);
			this.lblServer.Name = "lblServer";
			this.lblServer.Size = new System.Drawing.Size(45, 24);
			this.lblServer.TabIndex = 9;
			this.lblServer.Text = "host";
			// 
			// lblServiceName
			// 
			this.lblServiceName.AutoSize = true;
			this.lblServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblServiceName.Location = new System.Drawing.Point(9, 44);
			this.lblServiceName.Name = "lblServiceName";
			this.lblServiceName.Size = new System.Drawing.Size(70, 24);
			this.lblServiceName.TabIndex = 10;
			this.lblServiceName.Text = "service";
			// 
			// btnTaskView
			// 
			this.btnTaskView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnTaskView.Location = new System.Drawing.Point(230, 294);
			this.btnTaskView.Name = "btnTaskView";
			this.btnTaskView.Size = new System.Drawing.Size(100, 23);
			this.btnTaskView.TabIndex = 11;
			this.btnTaskView.Text = "Task View";
			this.btnTaskView.UseVisualStyleBackColor = true;
			this.btnTaskView.Click += new System.EventHandler(this.btnTaskView_Click);
			// 
			// btnWorkers
			// 
			this.btnWorkers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnWorkers.Location = new System.Drawing.Point(230, 265);
			this.btnWorkers.Name = "btnWorkers";
			this.btnWorkers.Size = new System.Drawing.Size(100, 23);
			this.btnWorkers.TabIndex = 12;
			this.btnWorkers.Text = "Workers";
			this.btnWorkers.UseVisualStyleBackColor = true;
			this.btnWorkers.Click += new System.EventHandler(this.btnWorkers_Click);
			// 
			// WhizFlowMonitor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(342, 324);
			this.Controls.Add(this.btnWorkers);
			this.Controls.Add(this.btnTaskView);
			this.Controls.Add(this.lblServiceName);
			this.Controls.Add(this.lblServer);
			this.Controls.Add(this.btnConfig);
			this.Controls.Add(this.btnTaskLogs);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnLogs);
			this.Controls.Add(this.btnSchedulers);
			this.Controls.Add(this.btnQueues);
			this.Controls.Add(this.lsvDomains);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(358, 363);
			this.MaximumSize = new System.Drawing.Size(358, 600);
			this.MinimumSize = new System.Drawing.Size(358, 312);
			this.Name = "WhizFlowMonitor";
			this.Text = "WhizFlowMonitor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WhizFlowMonitor_FormClosing);
			this.Load += new System.EventHandler(this.WhizFlowMonitor_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lsvDomains;
		private System.Windows.Forms.ColumnHeader colDomain;
		private System.Windows.Forms.Button btnQueues;
		private System.Windows.Forms.Button btnSchedulers;
		private System.Windows.Forms.Button btnLogs;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnTaskLogs;
		private System.Windows.Forms.Button btnConfig;
		private System.Windows.Forms.ColumnHeader colStatus;
		private System.Windows.Forms.Label lblServer;
		private System.Windows.Forms.Label lblServiceName;
		private System.Windows.Forms.Button btnTaskView;
		private System.Windows.Forms.Button btnWorkers;
	}
}