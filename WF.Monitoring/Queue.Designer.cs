namespace Whiz.WhizFlow.Monitoring
{
	partial class Queue
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
			this.components = new System.ComponentModel.Container();
			this.lblProcessed = new System.Windows.Forms.Label();
			this.lblInQueue = new System.Windows.Forms.Label();
			this.prbQueue = new System.Windows.Forms.ProgressBar();
			this.lblColorIndicator = new System.Windows.Forms.Label();
			this.lblError = new System.Windows.Forms.Label();
			this.lblActual = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// lblProcessed
			// 
			this.lblProcessed.AutoSize = true;
			this.lblProcessed.BackColor = System.Drawing.Color.Lime;
			this.lblProcessed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProcessed.Location = new System.Drawing.Point(202, 10);
			this.lblProcessed.Name = "lblProcessed";
			this.lblProcessed.Size = new System.Drawing.Size(59, 13);
			this.lblProcessed.TabIndex = 17;
			this.lblProcessed.Text = " processed";
			// 
			// lblInQueue
			// 
			this.lblInQueue.AutoSize = true;
			this.lblInQueue.BackColor = System.Drawing.Color.Lime;
			this.lblInQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInQueue.Location = new System.Drawing.Point(108, 10);
			this.lblInQueue.Name = "lblInQueue";
			this.lblInQueue.Size = new System.Drawing.Size(51, 13);
			this.lblInQueue.TabIndex = 16;
			this.lblInQueue.Text = " in queue";
			// 
			// prbQueue
			// 
			this.prbQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.prbQueue.Location = new System.Drawing.Point(2, 3);
			this.prbQueue.Maximum = 50;
			this.prbQueue.Name = "prbQueue";
			this.prbQueue.Size = new System.Drawing.Size(100, 20);
			this.prbQueue.TabIndex = 15;
			// 
			// lblColorIndicator
			// 
			this.lblColorIndicator.BackColor = System.Drawing.Color.Lime;
			this.lblColorIndicator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblColorIndicator.Location = new System.Drawing.Point(-3, -2);
			this.lblColorIndicator.Name = "lblColorIndicator";
			this.lblColorIndicator.Size = new System.Drawing.Size(310, 56);
			this.lblColorIndicator.TabIndex = 19;
			// 
			// lblError
			// 
			this.lblError.BackColor = System.Drawing.Color.Red;
			this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblError.Location = new System.Drawing.Point(-5, -2);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(312, 54);
			this.lblError.TabIndex = 18;
			this.lblError.DoubleClick += new System.EventHandler(this.lblError_DoubleClick);
			// 
			// lblActual
			// 
			this.lblActual.AutoSize = true;
			this.lblActual.BackColor = System.Drawing.Color.Lime;
			this.lblActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblActual.Location = new System.Drawing.Point(2, 26);
			this.lblActual.Name = "lblActual";
			this.lblActual.Size = new System.Drawing.Size(0, 13);
			this.lblActual.TabIndex = 20;
			// 
			// Queue
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(302, 49);
			this.Controls.Add(this.lblError);
			this.Controls.Add(this.lblActual);
			this.Controls.Add(this.lblProcessed);
			this.Controls.Add(this.lblInQueue);
			this.Controls.Add(this.prbQueue);
			this.Controls.Add(this.lblColorIndicator);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(308, 74);
			this.Name = "Queue";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueueViewer_FormClosing);
			this.Load += new System.EventHandler(this.QueueViewer_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblProcessed;
		private System.Windows.Forms.Label lblInQueue;
		private System.Windows.Forms.ProgressBar prbQueue;
		private System.Windows.Forms.Label lblColorIndicator;
		private System.Windows.Forms.Label lblError;
		private System.Windows.Forms.Label lblActual;
		private System.Windows.Forms.ToolTip toolTip;

	}
}