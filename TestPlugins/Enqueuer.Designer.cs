namespace WhizFlowTestPlugins
{
	partial class Enqueuer
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
			this.btnEnqueue = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtItems = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblActualItem = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnEnqueue
			// 
			this.btnEnqueue.Location = new System.Drawing.Point(12, 12);
			this.btnEnqueue.Name = "btnEnqueue";
			this.btnEnqueue.Size = new System.Drawing.Size(75, 23);
			this.btnEnqueue.TabIndex = 0;
			this.btnEnqueue.Text = "Enqueue";
			this.btnEnqueue.UseVisualStyleBackColor = true;
			this.btnEnqueue.Click += new System.EventHandler(this.btnEnqueue_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Item Count:";
			// 
			// txtItems
			// 
			this.txtItems.Location = new System.Drawing.Point(79, 48);
			this.txtItems.Name = "txtItems";
			this.txtItems.Size = new System.Drawing.Size(100, 20);
			this.txtItems.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 86);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Actual Item:";
			// 
			// lblActualItem
			// 
			this.lblActualItem.AutoSize = true;
			this.lblActualItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblActualItem.Location = new System.Drawing.Point(76, 86);
			this.lblActualItem.Name = "lblActualItem";
			this.lblActualItem.Size = new System.Drawing.Size(0, 16);
			this.lblActualItem.TabIndex = 4;
			// 
			// Enqueuer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(192, 129);
			this.Controls.Add(this.lblActualItem);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtItems);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnEnqueue);
			this.Name = "Enqueuer";
			this.Text = "Enqueuer";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnEnqueue;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtItems;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblActualItem;
	}
}