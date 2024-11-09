namespace Whiz.WhizFlow.Monitoring
{
	partial class Menu
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
			this.btnOpenMonitor = new System.Windows.Forms.Button();
			this.txtWFHost = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOpenMonitor
			// 
			this.btnOpenMonitor.Location = new System.Drawing.Point(407, 12);
			this.btnOpenMonitor.Name = "btnOpenMonitor";
			this.btnOpenMonitor.Size = new System.Drawing.Size(75, 23);
			this.btnOpenMonitor.TabIndex = 1;
			this.btnOpenMonitor.Text = "Open";
			this.btnOpenMonitor.UseVisualStyleBackColor = true;
			this.btnOpenMonitor.Click += new System.EventHandler(this.btnOpenMonitor_Click);
			// 
			// txtWFHost
			// 
			this.txtWFHost.Location = new System.Drawing.Point(96, 14);
			this.txtWFHost.Name = "txtWFHost";
			this.txtWFHost.Size = new System.Drawing.Size(305, 20);
			this.txtWFHost.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "WhizFlow Host";
			// 
			// Menu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(494, 50);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtWFHost);
			this.Controls.Add(this.btnOpenMonitor);
			this.FormLocation = new System.Drawing.Point(15, 15);
			this.FormSize = new System.Drawing.Size(510, 89);
			this.Name = "Menu";
			this.Text = "Menu";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOpenMonitor;
		private System.Windows.Forms.TextBox txtWFHost;
		private System.Windows.Forms.Label label1;
	}
}