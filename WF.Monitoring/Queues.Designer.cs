namespace Whiz.WhizFlow.Monitoring
{
	partial class Queues
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
			this.lsvQueues = new System.Windows.Forms.ListView();
			this.colQueue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colItems = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colThroughput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMaxThroughput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colProcessed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// lsvQueues
			// 
			this.lsvQueues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colQueue,
            this.colItems,
            this.colThroughput,
            this.colMaxThroughput,
            this.colProcessed});
			this.lsvQueues.FullRowSelect = true;
			this.lsvQueues.GridLines = true;
			this.lsvQueues.Location = new System.Drawing.Point(12, 12);
			this.lsvQueues.MultiSelect = false;
			this.lsvQueues.Name = "lsvQueues";
			this.lsvQueues.Size = new System.Drawing.Size(378, 390);
			this.lsvQueues.TabIndex = 0;
			this.lsvQueues.UseCompatibleStateImageBehavior = false;
			this.lsvQueues.View = System.Windows.Forms.View.Details;
			this.lsvQueues.DoubleClick += new System.EventHandler(this.lsvQueues_DoubleClick);
			// 
			// colQueue
			// 
			this.colQueue.Text = "Queue";
			this.colQueue.Width = 100;
			// 
			// colItems
			// 
			this.colItems.Text = "Items";
			// 
			// colThroughput
			// 
			this.colThroughput.Text = "Throughput";
			// 
			// colMaxThroughput
			// 
			this.colMaxThroughput.Text = "Max";
			// 
			// colProcessed
			// 
			this.colProcessed.Text = "Processed";
			// 
			// Queues
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(402, 414);
			this.Controls.Add(this.lsvQueues);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(418, 453);
			this.Name = "Queues";
			this.Text = "Queues";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Queues_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lsvQueues;
		private System.Windows.Forms.ColumnHeader colQueue;
		private System.Windows.Forms.ColumnHeader colItems;
		private System.Windows.Forms.ColumnHeader colThroughput;
		private System.Windows.Forms.ColumnHeader colMaxThroughput;
		private System.Windows.Forms.ColumnHeader colProcessed;
	}
}