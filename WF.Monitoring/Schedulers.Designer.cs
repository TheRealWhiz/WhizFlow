namespace Whiz.WhizFlow.Monitoring
{
	partial class Schedulers
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
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnChangeRecurrence = new System.Windows.Forms.Button();
			this.btnResetRecurrence = new System.Windows.Forms.Button();
			this.txtRecurrence = new System.Windows.Forms.TextBox();
			this.lsvSchedulers = new System.Windows.Forms.ListView();
			this.colScheduler = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colLastRunning = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(13, 453);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(94, 453);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 1;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnChangeRecurrence
			// 
			this.btnChangeRecurrence.Location = new System.Drawing.Point(175, 453);
			this.btnChangeRecurrence.Name = "btnChangeRecurrence";
			this.btnChangeRecurrence.Size = new System.Drawing.Size(75, 23);
			this.btnChangeRecurrence.TabIndex = 2;
			this.btnChangeRecurrence.Text = "Change";
			this.btnChangeRecurrence.UseVisualStyleBackColor = true;
			this.btnChangeRecurrence.Click += new System.EventHandler(this.btnChangeRecurrence_Click);
			// 
			// btnResetRecurrence
			// 
			this.btnResetRecurrence.Location = new System.Drawing.Point(345, 453);
			this.btnResetRecurrence.Name = "btnResetRecurrence";
			this.btnResetRecurrence.Size = new System.Drawing.Size(75, 23);
			this.btnResetRecurrence.TabIndex = 3;
			this.btnResetRecurrence.Text = "Reset";
			this.btnResetRecurrence.UseVisualStyleBackColor = true;
			this.btnResetRecurrence.Click += new System.EventHandler(this.btnResetRecurrence_Click);
			// 
			// txtRecurrence
			// 
			this.txtRecurrence.Location = new System.Drawing.Point(256, 455);
			this.txtRecurrence.Name = "txtRecurrence";
			this.txtRecurrence.Size = new System.Drawing.Size(83, 20);
			this.txtRecurrence.TabIndex = 4;
			// 
			// lsvSchedulers
			// 
			this.lsvSchedulers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colScheduler,
            this.colMode,
            this.colLastRunning,
            this.colStatus});
			this.lsvSchedulers.FullRowSelect = true;
			this.lsvSchedulers.GridLines = true;
			this.lsvSchedulers.Location = new System.Drawing.Point(13, 12);
			this.lsvSchedulers.MultiSelect = false;
			this.lsvSchedulers.Name = "lsvSchedulers";
			this.lsvSchedulers.Size = new System.Drawing.Size(407, 437);
			this.lsvSchedulers.TabIndex = 5;
			this.lsvSchedulers.UseCompatibleStateImageBehavior = false;
			this.lsvSchedulers.View = System.Windows.Forms.View.Details;
			// 
			// colScheduler
			// 
			this.colScheduler.Text = "Scheduler";
			this.colScheduler.Width = 89;
			// 
			// colMode
			// 
			this.colMode.Text = "Mode";
			this.colMode.Width = 103;
			// 
			// colLastRunning
			// 
			this.colLastRunning.Text = "Last Running";
			this.colLastRunning.Width = 122;
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			// 
			// Schedulers
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(432, 488);
			this.Controls.Add(this.lsvSchedulers);
			this.Controls.Add(this.txtRecurrence);
			this.Controls.Add(this.btnResetRecurrence);
			this.Controls.Add(this.btnChangeRecurrence);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(448, 527);
			this.Name = "Schedulers";
			this.Text = "Schedulers";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Schedulers_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnChangeRecurrence;
		private System.Windows.Forms.Button btnResetRecurrence;
		private System.Windows.Forms.TextBox txtRecurrence;
		private System.Windows.Forms.ListView lsvSchedulers;
		private System.Windows.Forms.ColumnHeader colScheduler;
		private System.Windows.Forms.ColumnHeader colMode;
		private System.Windows.Forms.ColumnHeader colLastRunning;
		private System.Windows.Forms.ColumnHeader colStatus;
	}
}