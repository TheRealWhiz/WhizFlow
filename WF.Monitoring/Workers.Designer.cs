namespace Whiz.WhizFlow.Monitoring
{
	partial class Workers
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
			this.lsvSchedulers = new System.Windows.Forms.ListView();
			this.colWorker = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// lsvSchedulers
			// 
			this.lsvSchedulers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lsvSchedulers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colWorker,
            this.colStatus});
			this.lsvSchedulers.FullRowSelect = true;
			this.lsvSchedulers.GridLines = true;
			this.lsvSchedulers.HideSelection = false;
			this.lsvSchedulers.Location = new System.Drawing.Point(13, 12);
			this.lsvSchedulers.MultiSelect = false;
			this.lsvSchedulers.Name = "lsvSchedulers";
			this.lsvSchedulers.Size = new System.Drawing.Size(407, 464);
			this.lsvSchedulers.TabIndex = 5;
			this.lsvSchedulers.UseCompatibleStateImageBehavior = false;
			this.lsvSchedulers.View = System.Windows.Forms.View.Details;
			// 
			// colWorker
			// 
			this.colWorker.Text = "Worker";
			this.colWorker.Width = 284;
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			this.colStatus.Width = 95;
			// 
			// Workers
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(432, 488);
			this.Controls.Add(this.lsvSchedulers);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(448, 527);
			this.Name = "Workers";
			this.Text = "Workers";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Schedulers_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ListView lsvSchedulers;
		private System.Windows.Forms.ColumnHeader colWorker;
		private System.Windows.Forms.ColumnHeader colStatus;
	}
}