namespace Whiz.WhizFlow.Monitoring
{
	partial class Configurations
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
			this.txtConfiguration = new System.Windows.Forms.TextBox();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.lsvConfigurations = new System.Windows.Forms.ListView();
			this.colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colService = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDomain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnActivate = new System.Windows.Forms.Button();
			this.btnDeactivate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtConfiguration
			// 
			this.txtConfiguration.Location = new System.Drawing.Point(12, 227);
			this.txtConfiguration.Multiline = true;
			this.txtConfiguration.Name = "txtConfiguration";
			this.txtConfiguration.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtConfiguration.Size = new System.Drawing.Size(836, 399);
			this.txtConfiguration.TabIndex = 0;
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(773, 12);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(75, 23);
			this.btnLoad.TabIndex = 3;
			this.btnLoad.Text = "Load";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(773, 632);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 4;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(773, 41);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 5;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// lsvConfigurations
			// 
			this.lsvConfigurations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colHost,
            this.colService,
            this.colDomain,
            this.colActive});
			this.lsvConfigurations.FullRowSelect = true;
			this.lsvConfigurations.GridLines = true;
			this.lsvConfigurations.Location = new System.Drawing.Point(12, 12);
			this.lsvConfigurations.MultiSelect = false;
			this.lsvConfigurations.Name = "lsvConfigurations";
			this.lsvConfigurations.Size = new System.Drawing.Size(755, 209);
			this.lsvConfigurations.TabIndex = 6;
			this.lsvConfigurations.UseCompatibleStateImageBehavior = false;
			this.lsvConfigurations.View = System.Windows.Forms.View.Details;
			this.lsvConfigurations.SelectedIndexChanged += new System.EventHandler(this.lsvConfigurations_SelectedIndexChanged);
			// 
			// colId
			// 
			this.colId.Text = "Id";
			// 
			// colHost
			// 
			this.colHost.Text = "Host";
			this.colHost.Width = 160;
			// 
			// colService
			// 
			this.colService.Text = "Service";
			this.colService.Width = 160;
			// 
			// colDomain
			// 
			this.colDomain.Text = "Domain";
			this.colDomain.Width = 280;
			// 
			// colActive
			// 
			this.colActive.Text = "Active";
			// 
			// btnActivate
			// 
			this.btnActivate.Location = new System.Drawing.Point(773, 70);
			this.btnActivate.Name = "btnActivate";
			this.btnActivate.Size = new System.Drawing.Size(75, 23);
			this.btnActivate.TabIndex = 7;
			this.btnActivate.Text = "Activate";
			this.btnActivate.UseVisualStyleBackColor = true;
			this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
			// 
			// btnDeactivate
			// 
			this.btnDeactivate.Location = new System.Drawing.Point(773, 99);
			this.btnDeactivate.Name = "btnDeactivate";
			this.btnDeactivate.Size = new System.Drawing.Size(75, 23);
			this.btnDeactivate.TabIndex = 8;
			this.btnDeactivate.Text = "Deactivate";
			this.btnDeactivate.UseVisualStyleBackColor = true;
			this.btnDeactivate.Click += new System.EventHandler(this.btnDeactivate_Click);
			// 
			// Configurations
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(860, 667);
			this.Controls.Add(this.btnDeactivate);
			this.Controls.Add(this.btnActivate);
			this.Controls.Add(this.lsvConfigurations);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.txtConfiguration);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(876, 706);
			this.Name = "Configurations";
			this.ShowInTaskbar = false;
			this.Load += new System.EventHandler(this.TaskContentViewer_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtConfiguration;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.ListView lsvConfigurations;
		private System.Windows.Forms.ColumnHeader colId;
		private System.Windows.Forms.ColumnHeader colHost;
		private System.Windows.Forms.ColumnHeader colService;
		private System.Windows.Forms.ColumnHeader colDomain;
		private System.Windows.Forms.ColumnHeader colActive;
		private System.Windows.Forms.Button btnActivate;
		private System.Windows.Forms.Button btnDeactivate;
	}
}