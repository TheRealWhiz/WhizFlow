namespace Whiz.WhizFlow.Monitoring
{
	partial class Logs
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
			this.grpTimeFrame = new System.Windows.Forms.GroupBox();
			this.txtFrom = new System.Windows.Forms.TextBox();
			this.lblFrom = new System.Windows.Forms.Label();
			this.txtTo = new System.Windows.Forms.TextBox();
			this.lblTo = new System.Windows.Forms.Label();
			this.btnFromNow = new System.Windows.Forms.Button();
			this.btnToNow = new System.Windows.Forms.Button();
			this.grpLast = new System.Windows.Forms.GroupBox();
			this.txtEntries = new System.Windows.Forms.TextBox();
			this.txtTaskContentId = new System.Windows.Forms.TextBox();
			this.btnGetTaskInfo = new System.Windows.Forms.Button();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.txtAdditionalInformation = new System.Windows.Forms.TextBox();
			this.lblLogType = new System.Windows.Forms.Label();
			this.lblModule = new System.Windows.Forms.Label();
			this.cmbLogType = new System.Windows.Forms.ComboBox();
			this.cmbModule = new System.Windows.Forms.ComboBox();
			this.lsvLogs = new System.Windows.Forms.ListView();
			this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colService = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDomain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colModule = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colLogType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colTaskContentId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colObject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colAdditionalInformation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.radLastEntries = new System.Windows.Forms.RadioButton();
			this.radTimeframe = new System.Windows.Forms.RadioButton();
			this.chkAllHosts = new System.Windows.Forms.CheckBox();
			this.chkAllServices = new System.Windows.Forms.CheckBox();
			this.chkAllDomains = new System.Windows.Forms.CheckBox();
			this.btnGetLogs = new System.Windows.Forms.Button();
			this.grpTaskContentId = new System.Windows.Forms.GroupBox();
			this.txtFilterTaskContentId = new System.Windows.Forms.TextBox();
			this.radTaskContentId = new System.Windows.Forms.RadioButton();
			this.grpTimeFrame.SuspendLayout();
			this.grpLast.SuspendLayout();
			this.grpTaskContentId.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpTimeFrame
			// 
			this.grpTimeFrame.Controls.Add(this.txtFrom);
			this.grpTimeFrame.Controls.Add(this.lblFrom);
			this.grpTimeFrame.Controls.Add(this.txtTo);
			this.grpTimeFrame.Controls.Add(this.lblTo);
			this.grpTimeFrame.Controls.Add(this.btnFromNow);
			this.grpTimeFrame.Controls.Add(this.btnToNow);
			this.grpTimeFrame.Location = new System.Drawing.Point(414, 457);
			this.grpTimeFrame.Name = "grpTimeFrame";
			this.grpTimeFrame.Size = new System.Drawing.Size(229, 67);
			this.grpTimeFrame.TabIndex = 39;
			this.grpTimeFrame.TabStop = false;
			this.grpTimeFrame.Text = "Logs in time frame";
			// 
			// txtFrom
			// 
			this.txtFrom.Location = new System.Drawing.Point(45, 16);
			this.txtFrom.Name = "txtFrom";
			this.txtFrom.Size = new System.Drawing.Size(145, 20);
			this.txtFrom.TabIndex = 12;
			// 
			// lblFrom
			// 
			this.lblFrom.AutoSize = true;
			this.lblFrom.Location = new System.Drawing.Point(6, 19);
			this.lblFrom.Name = "lblFrom";
			this.lblFrom.Size = new System.Drawing.Size(30, 13);
			this.lblFrom.TabIndex = 13;
			this.lblFrom.Text = "From";
			// 
			// txtTo
			// 
			this.txtTo.Location = new System.Drawing.Point(45, 38);
			this.txtTo.Name = "txtTo";
			this.txtTo.Size = new System.Drawing.Size(145, 20);
			this.txtTo.TabIndex = 14;
			// 
			// lblTo
			// 
			this.lblTo.AutoSize = true;
			this.lblTo.Location = new System.Drawing.Point(6, 41);
			this.lblTo.Name = "lblTo";
			this.lblTo.Size = new System.Drawing.Size(20, 13);
			this.lblTo.TabIndex = 15;
			this.lblTo.Text = "To";
			// 
			// btnFromNow
			// 
			this.btnFromNow.Location = new System.Drawing.Point(196, 17);
			this.btnFromNow.Name = "btnFromNow";
			this.btnFromNow.Size = new System.Drawing.Size(22, 20);
			this.btnFromNow.TabIndex = 18;
			this.btnFromNow.Text = "N";
			this.btnFromNow.UseVisualStyleBackColor = true;
			this.btnFromNow.Click += new System.EventHandler(this.btnFromNow_Click);
			// 
			// btnToNow
			// 
			this.btnToNow.Location = new System.Drawing.Point(196, 38);
			this.btnToNow.Name = "btnToNow";
			this.btnToNow.Size = new System.Drawing.Size(22, 20);
			this.btnToNow.TabIndex = 17;
			this.btnToNow.Text = "N";
			this.btnToNow.UseVisualStyleBackColor = true;
			this.btnToNow.Click += new System.EventHandler(this.btnToNow_Click);
			// 
			// grpLast
			// 
			this.grpLast.Controls.Add(this.txtEntries);
			this.grpLast.Location = new System.Drawing.Point(295, 457);
			this.grpLast.Name = "grpLast";
			this.grpLast.Size = new System.Drawing.Size(113, 47);
			this.grpLast.TabIndex = 38;
			this.grpLast.TabStop = false;
			this.grpLast.Text = "Last logs";
			// 
			// txtEntries
			// 
			this.txtEntries.Location = new System.Drawing.Point(6, 17);
			this.txtEntries.Name = "txtEntries";
			this.txtEntries.Size = new System.Drawing.Size(100, 20);
			this.txtEntries.TabIndex = 0;
			// 
			// txtTaskContentId
			// 
			this.txtTaskContentId.Location = new System.Drawing.Point(772, 381);
			this.txtTaskContentId.Name = "txtTaskContentId";
			this.txtTaskContentId.ReadOnly = true;
			this.txtTaskContentId.Size = new System.Drawing.Size(100, 20);
			this.txtTaskContentId.TabIndex = 37;
			// 
			// btnGetTaskInfo
			// 
			this.btnGetTaskInfo.Enabled = false;
			this.btnGetTaskInfo.Location = new System.Drawing.Point(772, 407);
			this.btnGetTaskInfo.Name = "btnGetTaskInfo";
			this.btnGetTaskInfo.Size = new System.Drawing.Size(100, 20);
			this.btnGetTaskInfo.TabIndex = 36;
			this.btnGetTaskInfo.Text = "Get Task Info";
			this.btnGetTaskInfo.UseVisualStyleBackColor = true;
			this.btnGetTaskInfo.Click += new System.EventHandler(this.btnGetTaskInfo_Click);
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(12, 381);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtMessage.Size = new System.Drawing.Size(262, 71);
			this.txtMessage.TabIndex = 35;
			// 
			// txtAdditionalInformation
			// 
			this.txtAdditionalInformation.Location = new System.Drawing.Point(280, 381);
			this.txtAdditionalInformation.Multiline = true;
			this.txtAdditionalInformation.Name = "txtAdditionalInformation";
			this.txtAdditionalInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtAdditionalInformation.Size = new System.Drawing.Size(486, 71);
			this.txtAdditionalInformation.TabIndex = 34;
			// 
			// lblLogType
			// 
			this.lblLogType.AutoSize = true;
			this.lblLogType.Location = new System.Drawing.Point(12, 485);
			this.lblLogType.Name = "lblLogType";
			this.lblLogType.Size = new System.Drawing.Size(52, 13);
			this.lblLogType.TabIndex = 32;
			this.lblLogType.Text = "Log Type";
			// 
			// lblModule
			// 
			this.lblModule.AutoSize = true;
			this.lblModule.Location = new System.Drawing.Point(12, 462);
			this.lblModule.Name = "lblModule";
			this.lblModule.Size = new System.Drawing.Size(42, 13);
			this.lblModule.TabIndex = 31;
			this.lblModule.Text = "Module";
			// 
			// cmbLogType
			// 
			this.cmbLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLogType.FormattingEnabled = true;
			this.cmbLogType.Location = new System.Drawing.Point(82, 480);
			this.cmbLogType.Name = "cmbLogType";
			this.cmbLogType.Size = new System.Drawing.Size(121, 21);
			this.cmbLogType.TabIndex = 29;
			// 
			// cmbModule
			// 
			this.cmbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbModule.FormattingEnabled = true;
			this.cmbModule.Location = new System.Drawing.Point(82, 458);
			this.cmbModule.Name = "cmbModule";
			this.cmbModule.Size = new System.Drawing.Size(121, 21);
			this.cmbModule.TabIndex = 28;
			// 
			// lsvLogs
			// 
			this.lsvLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colHost,
            this.colService,
            this.colDomain,
            this.colModule,
            this.colLogType,
            this.colTaskContentId,
            this.colObject,
            this.colMessage,
            this.colAdditionalInformation});
			this.lsvLogs.FullRowSelect = true;
			this.lsvLogs.GridLines = true;
			this.lsvLogs.Location = new System.Drawing.Point(12, 12);
			this.lsvLogs.Name = "lsvLogs";
			this.lsvLogs.Size = new System.Drawing.Size(860, 363);
			this.lsvLogs.TabIndex = 27;
			this.lsvLogs.UseCompatibleStateImageBehavior = false;
			this.lsvLogs.View = System.Windows.Forms.View.Details;
			this.lsvLogs.SelectedIndexChanged += new System.EventHandler(this.lsvLogs_SelectedIndexChanged);
			this.lsvLogs.DoubleClick += new System.EventHandler(this.lsvLogs_DoubleClick);
			// 
			// colTime
			// 
			this.colTime.Text = "Time";
			this.colTime.Width = 73;
			// 
			// colHost
			// 
			this.colHost.Text = "Host";
			this.colHost.Width = 64;
			// 
			// colService
			// 
			this.colService.Text = "Service";
			// 
			// colDomain
			// 
			this.colDomain.Text = "Domain";
			// 
			// colModule
			// 
			this.colModule.Text = "Module";
			this.colModule.Width = 87;
			// 
			// colLogType
			// 
			this.colLogType.Text = "Log Type";
			this.colLogType.Width = 92;
			// 
			// colTaskContentId
			// 
			this.colTaskContentId.Text = "Task";
			this.colTaskContentId.Width = 67;
			// 
			// colObject
			// 
			this.colObject.Text = "Object";
			this.colObject.Width = 95;
			// 
			// colMessage
			// 
			this.colMessage.Text = "Message";
			this.colMessage.Width = 186;
			// 
			// colAdditionalInformation
			// 
			this.colAdditionalInformation.Text = "Additional Information";
			this.colAdditionalInformation.Width = 169;
			// 
			// radLastEntries
			// 
			this.radLastEntries.AutoSize = true;
			this.radLastEntries.Location = new System.Drawing.Point(209, 460);
			this.radLastEntries.Name = "radLastEntries";
			this.radLastEntries.Size = new System.Drawing.Size(80, 17);
			this.radLastEntries.TabIndex = 40;
			this.radLastEntries.TabStop = true;
			this.radLastEntries.Text = "Last Entries";
			this.radLastEntries.UseVisualStyleBackColor = true;
			this.radLastEntries.CheckedChanged += new System.EventHandler(this.radLastEntries_CheckedChanged);
			// 
			// radTimeframe
			// 
			this.radTimeframe.AutoSize = true;
			this.radTimeframe.Location = new System.Drawing.Point(209, 483);
			this.radTimeframe.Name = "radTimeframe";
			this.radTimeframe.Size = new System.Drawing.Size(74, 17);
			this.radTimeframe.TabIndex = 41;
			this.radTimeframe.TabStop = true;
			this.radTimeframe.Text = "Timeframe";
			this.radTimeframe.UseVisualStyleBackColor = true;
			this.radTimeframe.CheckedChanged += new System.EventHandler(this.radTimeframe_CheckedChanged);
			// 
			// chkAllHosts
			// 
			this.chkAllHosts.AutoSize = true;
			this.chkAllHosts.Location = new System.Drawing.Point(15, 507);
			this.chkAllHosts.Name = "chkAllHosts";
			this.chkAllHosts.Size = new System.Drawing.Size(67, 17);
			this.chkAllHosts.TabIndex = 42;
			this.chkAllHosts.Text = "All Hosts";
			this.chkAllHosts.UseVisualStyleBackColor = true;
			// 
			// chkAllServices
			// 
			this.chkAllServices.AutoSize = true;
			this.chkAllServices.Location = new System.Drawing.Point(15, 530);
			this.chkAllServices.Name = "chkAllServices";
			this.chkAllServices.Size = new System.Drawing.Size(81, 17);
			this.chkAllServices.TabIndex = 43;
			this.chkAllServices.Text = "All Services";
			this.chkAllServices.UseVisualStyleBackColor = true;
			// 
			// chkAllDomains
			// 
			this.chkAllDomains.AutoSize = true;
			this.chkAllDomains.Location = new System.Drawing.Point(103, 507);
			this.chkAllDomains.Name = "chkAllDomains";
			this.chkAllDomains.Size = new System.Drawing.Size(81, 17);
			this.chkAllDomains.TabIndex = 44;
			this.chkAllDomains.Text = "All Domains";
			this.chkAllDomains.UseVisualStyleBackColor = true;
			// 
			// btnGetLogs
			// 
			this.btnGetLogs.Location = new System.Drawing.Point(772, 457);
			this.btnGetLogs.Name = "btnGetLogs";
			this.btnGetLogs.Size = new System.Drawing.Size(99, 20);
			this.btnGetLogs.TabIndex = 45;
			this.btnGetLogs.Text = "Get";
			this.btnGetLogs.UseVisualStyleBackColor = true;
			this.btnGetLogs.Click += new System.EventHandler(this.btnGetLogs_Click);
			// 
			// grpTaskContentId
			// 
			this.grpTaskContentId.Controls.Add(this.txtFilterTaskContentId);
			this.grpTaskContentId.Location = new System.Drawing.Point(653, 457);
			this.grpTaskContentId.Name = "grpTaskContentId";
			this.grpTaskContentId.Size = new System.Drawing.Size(113, 47);
			this.grpTaskContentId.TabIndex = 46;
			this.grpTaskContentId.TabStop = false;
			this.grpTaskContentId.Text = "Task related Logs";
			// 
			// txtFilterTaskContentId
			// 
			this.txtFilterTaskContentId.Location = new System.Drawing.Point(6, 17);
			this.txtFilterTaskContentId.Name = "txtFilterTaskContentId";
			this.txtFilterTaskContentId.Size = new System.Drawing.Size(100, 20);
			this.txtFilterTaskContentId.TabIndex = 0;
			// 
			// radTaskContentId
			// 
			this.radTaskContentId.AutoSize = true;
			this.radTaskContentId.Location = new System.Drawing.Point(209, 506);
			this.radTaskContentId.Name = "radTaskContentId";
			this.radTaskContentId.Size = new System.Drawing.Size(61, 17);
			this.radTaskContentId.TabIndex = 47;
			this.radTaskContentId.TabStop = true;
			this.radTaskContentId.Text = "Task Id";
			this.radTaskContentId.UseVisualStyleBackColor = true;
			this.radTaskContentId.CheckedChanged += new System.EventHandler(this.radTaskContentId_CheckedChanged);
			// 
			// Logs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(883, 561);
			this.Controls.Add(this.radTaskContentId);
			this.Controls.Add(this.grpTaskContentId);
			this.Controls.Add(this.btnGetLogs);
			this.Controls.Add(this.grpTimeFrame);
			this.Controls.Add(this.chkAllDomains);
			this.Controls.Add(this.chkAllServices);
			this.Controls.Add(this.chkAllHosts);
			this.Controls.Add(this.radTimeframe);
			this.Controls.Add(this.radLastEntries);
			this.Controls.Add(this.grpLast);
			this.Controls.Add(this.txtTaskContentId);
			this.Controls.Add(this.btnGetTaskInfo);
			this.Controls.Add(this.txtMessage);
			this.Controls.Add(this.txtAdditionalInformation);
			this.Controls.Add(this.lblLogType);
			this.Controls.Add(this.lblModule);
			this.Controls.Add(this.cmbLogType);
			this.Controls.Add(this.cmbModule);
			this.Controls.Add(this.lsvLogs);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(899, 600);
			this.Name = "Logs";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Logs";
			this.Load += new System.EventHandler(this.Logs_Load);
			this.grpTimeFrame.ResumeLayout(false);
			this.grpTimeFrame.PerformLayout();
			this.grpLast.ResumeLayout(false);
			this.grpLast.PerformLayout();
			this.grpTaskContentId.ResumeLayout(false);
			this.grpTaskContentId.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpTimeFrame;
		private System.Windows.Forms.TextBox txtFrom;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.TextBox txtTo;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Button btnFromNow;
		private System.Windows.Forms.Button btnToNow;
		private System.Windows.Forms.GroupBox grpLast;
		private System.Windows.Forms.TextBox txtEntries;
		private System.Windows.Forms.TextBox txtTaskContentId;
		private System.Windows.Forms.Button btnGetTaskInfo;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.TextBox txtAdditionalInformation;
		private System.Windows.Forms.Label lblLogType;
		private System.Windows.Forms.Label lblModule;
		private System.Windows.Forms.ComboBox cmbLogType;
		private System.Windows.Forms.ComboBox cmbModule;
		private System.Windows.Forms.ListView lsvLogs;
		private System.Windows.Forms.ColumnHeader colTime;
		private System.Windows.Forms.ColumnHeader colHost;
		private System.Windows.Forms.ColumnHeader colModule;
		private System.Windows.Forms.ColumnHeader colLogType;
		private System.Windows.Forms.ColumnHeader colTaskContentId;
		private System.Windows.Forms.ColumnHeader colObject;
		private System.Windows.Forms.ColumnHeader colMessage;
		private System.Windows.Forms.ColumnHeader colAdditionalInformation;
		private System.Windows.Forms.RadioButton radLastEntries;
		private System.Windows.Forms.RadioButton radTimeframe;
		private System.Windows.Forms.CheckBox chkAllHosts;
		private System.Windows.Forms.CheckBox chkAllServices;
		private System.Windows.Forms.CheckBox chkAllDomains;
		private System.Windows.Forms.Button btnGetLogs;
		private System.Windows.Forms.ColumnHeader colService;
		private System.Windows.Forms.ColumnHeader colDomain;
		private System.Windows.Forms.GroupBox grpTaskContentId;
		private System.Windows.Forms.TextBox txtFilterTaskContentId;
		private System.Windows.Forms.RadioButton radTaskContentId;
	}
}