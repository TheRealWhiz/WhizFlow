namespace Deltatre.Pipeline.Tools.Monitoring
{
	partial class Monitor
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
			this.btnQuery = new System.Windows.Forms.Button();
			this.lsbPipelines = new System.Windows.Forms.ListBox();
			this.txtMachineName = new System.Windows.Forms.TextBox();
			this.lblMachineName = new System.Windows.Forms.Label();
			this.cmbObjects = new System.Windows.Forms.ComboBox();
			this.lblObjectType = new System.Windows.Forms.Label();
			this.lsbObjects = new System.Windows.Forms.ListBox();
			this.lsvProperties = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnQuery
			// 
			this.btnQuery.Location = new System.Drawing.Point(172, 12);
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Size = new System.Drawing.Size(80, 20);
			this.btnQuery.TabIndex = 0;
			this.btnQuery.Text = "Query";
			this.btnQuery.UseVisualStyleBackColor = true;
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			// 
			// lsbPipelines
			// 
			this.lsbPipelines.FormattingEnabled = true;
			this.lsbPipelines.Location = new System.Drawing.Point(12, 38);
			this.lsbPipelines.Name = "lsbPipelines";
			this.lsbPipelines.Size = new System.Drawing.Size(151, 433);
			this.lsbPipelines.TabIndex = 1;
			this.lsbPipelines.SelectedIndexChanged += new System.EventHandler(this.lsbPipelines_SelectedIndexChanged);
			// 
			// txtMachineName
			// 
			this.txtMachineName.Location = new System.Drawing.Point(66, 12);
			this.txtMachineName.Name = "txtMachineName";
			this.txtMachineName.Size = new System.Drawing.Size(100, 20);
			this.txtMachineName.TabIndex = 2;
			// 
			// lblMachineName
			// 
			this.lblMachineName.AutoSize = true;
			this.lblMachineName.Location = new System.Drawing.Point(12, 15);
			this.lblMachineName.Name = "lblMachineName";
			this.lblMachineName.Size = new System.Drawing.Size(48, 13);
			this.lblMachineName.TabIndex = 3;
			this.lblMachineName.Text = "Machine";
			// 
			// cmbObjects
			// 
			this.cmbObjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbObjects.FormattingEnabled = true;
			this.cmbObjects.Items.AddRange(new object[] {
            "MSMQHandler",
            "FeedIdentifier",
            "PipeHandler",
            "PipeHandlerThread",
            "SchedulerHandler",
            "SchedulerHandlerThread"});
			this.cmbObjects.Location = new System.Drawing.Point(256, 38);
			this.cmbObjects.Name = "cmbObjects";
			this.cmbObjects.Size = new System.Drawing.Size(121, 21);
			this.cmbObjects.TabIndex = 4;
			this.cmbObjects.SelectedIndexChanged += new System.EventHandler(this.cmbObjects_SelectedIndexChanged);
			// 
			// lblObjectType
			// 
			this.lblObjectType.AutoSize = true;
			this.lblObjectType.Location = new System.Drawing.Point(169, 41);
			this.lblObjectType.Name = "lblObjectType";
			this.lblObjectType.Size = new System.Drawing.Size(43, 13);
			this.lblObjectType.TabIndex = 5;
			this.lblObjectType.Text = "Objects";
			// 
			// lsbObjects
			// 
			this.lsbObjects.FormattingEnabled = true;
			this.lsbObjects.Location = new System.Drawing.Point(172, 65);
			this.lsbObjects.Name = "lsbObjects";
			this.lsbObjects.Size = new System.Drawing.Size(205, 407);
			this.lsbObjects.TabIndex = 6;
			this.lsbObjects.SelectedIndexChanged += new System.EventHandler(this.lsbObjects_SelectedIndexChanged);
			// 
			// lsvProperties
			// 
			this.lsvProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.lsvProperties.FullRowSelect = true;
			this.lsvProperties.GridLines = true;
			this.lsvProperties.Location = new System.Drawing.Point(383, 38);
			this.lsvProperties.Name = "lsvProperties";
			this.lsvProperties.Size = new System.Drawing.Size(306, 434);
			this.lsvProperties.TabIndex = 8;
			this.lsvProperties.UseCompatibleStateImageBehavior = false;
			this.lsvProperties.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 84;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Value";
			this.columnHeader2.Width = 194;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(609, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 20);
			this.button1.TabIndex = 9;
			this.button1.Text = "Query";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(513, 8);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(80, 20);
			this.button2.TabIndex = 10;
			this.button2.Text = "Query";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(406, 8);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(80, 20);
			this.button3.TabIndex = 11;
			this.button3.Text = "Query";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// Monitor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(701, 478);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.lsvProperties);
			this.Controls.Add(this.lsbObjects);
			this.Controls.Add(this.lblObjectType);
			this.Controls.Add(this.cmbObjects);
			this.Controls.Add(this.lblMachineName);
			this.Controls.Add(this.txtMachineName);
			this.Controls.Add(this.lsbPipelines);
			this.Controls.Add(this.btnQuery);
			this.Name = "Monitor";
			this.Text = "Pipeline Monitor Application";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnQuery;
		private System.Windows.Forms.ListBox lsbPipelines;
		private System.Windows.Forms.TextBox txtMachineName;
		private System.Windows.Forms.Label lblMachineName;
		private System.Windows.Forms.ComboBox cmbObjects;
		private System.Windows.Forms.Label lblObjectType;
		private System.Windows.Forms.ListBox lsbObjects;
		private System.Windows.Forms.ListView lsvProperties;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
	}
}

