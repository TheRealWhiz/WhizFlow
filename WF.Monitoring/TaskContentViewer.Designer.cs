namespace Whiz.WhizFlow.Monitoring
{
	partial class TaskContentViewer
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
			this.txtContent = new System.Windows.Forms.TextBox();
			this.lsvProcessingInformation = new System.Windows.Forms.ListView();
			this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colService = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDomain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colQueue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colSignature = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colProcessingTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblServiceName = new System.Windows.Forms.Label();
			this.lblDomain = new System.Windows.Forms.Label();
			this.txtId = new System.Windows.Forms.TextBox();
			this.lblId = new System.Windows.Forms.Label();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnLogs = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtContent
			// 
			this.txtContent.Location = new System.Drawing.Point(12, 79);
			this.txtContent.Multiline = true;
			this.txtContent.Name = "txtContent";
			this.txtContent.ReadOnly = true;
			this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtContent.Size = new System.Drawing.Size(836, 517);
			this.txtContent.TabIndex = 0;
			// 
			// lsvProcessingInformation
			// 
			this.lsvProcessingInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lsvProcessingInformation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colHost,
            this.colService,
            this.colDomain,
            this.colQueue,
            this.colSignature,
            this.colProcessingTime});
			this.lsvProcessingInformation.FullRowSelect = true;
			this.lsvProcessingInformation.GridLines = true;
			this.lsvProcessingInformation.Location = new System.Drawing.Point(12, 602);
			this.lsvProcessingInformation.MultiSelect = false;
			this.lsvProcessingInformation.Name = "lsvProcessingInformation";
			this.lsvProcessingInformation.Size = new System.Drawing.Size(836, 147);
			this.lsvProcessingInformation.TabIndex = 2;
			this.lsvProcessingInformation.UseCompatibleStateImageBehavior = false;
			this.lsvProcessingInformation.View = System.Windows.Forms.View.Details;
			this.lsvProcessingInformation.DoubleClick += new System.EventHandler(this.lsvProcessingInformation_DoubleClick);
			this.lsvProcessingInformation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lsvProcessingInformation_KeyUp);
			// 
			// colTime
			// 
			this.colTime.Text = "Time";
			this.colTime.Width = 140;
			// 
			// colHost
			// 
			this.colHost.Text = "Host";
			this.colHost.Width = 130;
			// 
			// colService
			// 
			this.colService.Text = "Service";
			this.colService.Width = 130;
			// 
			// colDomain
			// 
			this.colDomain.Text = "Domain";
			this.colDomain.Width = 130;
			// 
			// colQueue
			// 
			this.colQueue.Text = "Queue";
			this.colQueue.Width = 130;
			// 
			// colSignature
			// 
			this.colSignature.Text = "Signature";
			this.colSignature.Width = 130;
			// 
			// colProcessingTime
			// 
			this.colProcessingTime.Text = "Processing Time";
			this.colProcessingTime.Width = 120;
			// 
			// lblServiceName
			// 
			this.lblServiceName.AutoSize = true;
			this.lblServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblServiceName.Location = new System.Drawing.Point(12, 9);
			this.lblServiceName.Name = "lblServiceName";
			this.lblServiceName.Size = new System.Drawing.Size(70, 24);
			this.lblServiceName.TabIndex = 11;
			this.lblServiceName.Text = "service";
			// 
			// lblDomain
			// 
			this.lblDomain.AutoSize = true;
			this.lblDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDomain.Location = new System.Drawing.Point(8, 42);
			this.lblDomain.Name = "lblDomain";
			this.lblDomain.Size = new System.Drawing.Size(73, 24);
			this.lblDomain.TabIndex = 13;
			this.lblDomain.Text = "domain";
			// 
			// txtId
			// 
			this.txtId.Location = new System.Drawing.Point(667, 11);
			this.txtId.Name = "txtId";
			this.txtId.Size = new System.Drawing.Size(100, 20);
			this.txtId.TabIndex = 14;
			// 
			// lblId
			// 
			this.lblId.AutoSize = true;
			this.lblId.Location = new System.Drawing.Point(642, 14);
			this.lblId.Name = "lblId";
			this.lblId.Size = new System.Drawing.Size(19, 13);
			this.lblId.TabIndex = 15;
			this.lblId.Text = "Id:";
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(773, 9);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(75, 23);
			this.btnLoad.TabIndex = 16;
			this.btnLoad.Text = "Load";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnLogs
			// 
			this.btnLogs.Location = new System.Drawing.Point(773, 37);
			this.btnLogs.Name = "btnLogs";
			this.btnLogs.Size = new System.Drawing.Size(75, 23);
			this.btnLogs.TabIndex = 17;
			this.btnLogs.Text = "Logs";
			this.btnLogs.UseVisualStyleBackColor = true;
			this.btnLogs.Click += new System.EventHandler(this.btnLogs_Click);
			// 
			// TaskContentViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(860, 761);
			this.Controls.Add(this.btnLogs);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.lblId);
			this.Controls.Add(this.txtId);
			this.Controls.Add(this.lblDomain);
			this.Controls.Add(this.lblServiceName);
			this.Controls.Add(this.lsvProcessingInformation);
			this.Controls.Add(this.txtContent);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(876, 800);
			this.Name = "TaskContentViewer";
			this.ShowInTaskbar = false;
			this.Text = "Task Viewer";
			this.Load += new System.EventHandler(this.TaskContentViewer_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtContent;
		private System.Windows.Forms.ListView lsvProcessingInformation;
		private System.Windows.Forms.ColumnHeader colTime;
		private System.Windows.Forms.ColumnHeader colHost;
		private System.Windows.Forms.ColumnHeader colService;
		private System.Windows.Forms.ColumnHeader colDomain;
		private System.Windows.Forms.ColumnHeader colQueue;
		private System.Windows.Forms.ColumnHeader colSignature;
		private System.Windows.Forms.ColumnHeader colProcessingTime;
		private System.Windows.Forms.Label lblServiceName;
		private System.Windows.Forms.Label lblDomain;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.Label lblId;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnLogs;
	}
}