namespace Whiz.WhizFlow.Monitoring
{
	partial class TaskLogs
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
			this.lsvProcessingInformation = new System.Windows.Forms.ListView();
			this.colTaskContentId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colService = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDomain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colQueue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colSignature = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colProcessingTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colTaskId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblDomain = new System.Windows.Forms.Label();
			this.lblServiceName = new System.Windows.Forms.Label();
			this.lblQueue = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lsvProcessingInformation
			// 
			this.lsvProcessingInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lsvProcessingInformation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTaskContentId,
            this.colTime,
            this.colHost,
            this.colService,
            this.colDomain,
            this.colQueue,
            this.colSignature,
            this.colProcessingTime});
			this.lsvProcessingInformation.FullRowSelect = true;
			this.lsvProcessingInformation.GridLines = true;
			this.lsvProcessingInformation.Location = new System.Drawing.Point(12, 84);
			this.lsvProcessingInformation.MultiSelect = false;
			this.lsvProcessingInformation.Name = "lsvProcessingInformation";
			this.lsvProcessingInformation.Size = new System.Drawing.Size(1031, 405);
			this.lsvProcessingInformation.TabIndex = 1;
			this.lsvProcessingInformation.UseCompatibleStateImageBehavior = false;
			this.lsvProcessingInformation.View = System.Windows.Forms.View.Details;
			this.lsvProcessingInformation.DoubleClick += new System.EventHandler(this.lsvProcessingInformation_DoubleClick);
			this.lsvProcessingInformation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lsvProcessingInformation_KeyUp);
			// 
			// colTaskContentId
			// 
			this.colTaskContentId.Text = "Task Content Id";
			this.colTaskContentId.Width = 90;
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
			// colFeedId
			// 
			this.colTaskId.Text = "Task Id";
			// 
			// lblDomain
			// 
			this.lblDomain.AutoSize = true;
			this.lblDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDomain.Location = new System.Drawing.Point(12, 33);
			this.lblDomain.Name = "lblDomain";
			this.lblDomain.Size = new System.Drawing.Size(73, 24);
			this.lblDomain.TabIndex = 15;
			this.lblDomain.Text = "domain";
			// 
			// lblServiceName
			// 
			this.lblServiceName.AutoSize = true;
			this.lblServiceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblServiceName.Location = new System.Drawing.Point(12, 9);
			this.lblServiceName.Name = "lblServiceName";
			this.lblServiceName.Size = new System.Drawing.Size(70, 24);
			this.lblServiceName.TabIndex = 14;
			this.lblServiceName.Text = "service";
			// 
			// lblQueue
			// 
			this.lblQueue.AutoSize = true;
			this.lblQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblQueue.Location = new System.Drawing.Point(12, 57);
			this.lblQueue.Name = "lblQueue";
			this.lblQueue.Size = new System.Drawing.Size(65, 24);
			this.lblQueue.TabIndex = 16;
			this.lblQueue.Text = "queue";
			// 
			// TaskLogs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1055, 501);
			this.Controls.Add(this.lblQueue);
			this.Controls.Add(this.lblDomain);
			this.Controls.Add(this.lblServiceName);
			this.Controls.Add(this.lsvProcessingInformation);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(1071, 540);
			this.Name = "TaskLogs";
			this.ShowInTaskbar = false;
			this.Text = "Task Logs";
			this.Load += new System.EventHandler(this.TaskLogs_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lsvProcessingInformation;
		private System.Windows.Forms.ColumnHeader colTime;
		private System.Windows.Forms.ColumnHeader colQueue;
		private System.Windows.Forms.ColumnHeader colSignature;
		private System.Windows.Forms.ColumnHeader colProcessingTime;
		private System.Windows.Forms.ColumnHeader colTaskContentId;
		private System.Windows.Forms.ColumnHeader colService;
		private System.Windows.Forms.ColumnHeader colDomain;
		private System.Windows.Forms.ColumnHeader colTaskId;
		private System.Windows.Forms.ColumnHeader colHost;
		private System.Windows.Forms.Label lblDomain;
		private System.Windows.Forms.Label lblServiceName;
		private System.Windows.Forms.Label lblQueue;
	}
}