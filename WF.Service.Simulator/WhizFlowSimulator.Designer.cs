namespace Whiz.WhizFlow.Tools.WhizFlowSimulator
{
	partial class WhizFlowSimulator
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
			this.txtMainConfig = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.txtServiceName = new System.Windows.Forms.TextBox();
			this.btnPCCreate = new System.Windows.Forms.Button();
			this.btnPCDelete = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtMainConfig
			// 
			this.txtMainConfig.Location = new System.Drawing.Point(109, 10);
			this.txtMainConfig.Name = "txtMainConfig";
			this.txtMainConfig.Size = new System.Drawing.Size(163, 20);
			this.txtMainConfig.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Configuration";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(16, 91);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 6;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(197, 91);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 7;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(13, 39);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(82, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "WhizFlow name";
			// 
			// txtServiceName
			// 
			this.txtServiceName.Location = new System.Drawing.Point(109, 36);
			this.txtServiceName.Name = "txtServiceName";
			this.txtServiceName.Size = new System.Drawing.Size(163, 20);
			this.txtServiceName.TabIndex = 10;
			// 
			// btnPCCreate
			// 
			this.btnPCCreate.Location = new System.Drawing.Point(109, 62);
			this.btnPCCreate.Name = "btnPCCreate";
			this.btnPCCreate.Size = new System.Drawing.Size(75, 23);
			this.btnPCCreate.TabIndex = 12;
			this.btnPCCreate.Text = "Create PC";
			this.btnPCCreate.UseVisualStyleBackColor = true;
			this.btnPCCreate.Click += new System.EventHandler(this.btnPCCreate_Click);
			// 
			// btnPCDelete
			// 
			this.btnPCDelete.Location = new System.Drawing.Point(197, 62);
			this.btnPCDelete.Name = "btnPCDelete";
			this.btnPCDelete.Size = new System.Drawing.Size(75, 23);
			this.btnPCDelete.TabIndex = 13;
			this.btnPCDelete.Text = "Delete PC";
			this.btnPCDelete.UseVisualStyleBackColor = true;
			this.btnPCDelete.Click += new System.EventHandler(this.btnPCDelete_Click);
			// 
			// WhizFlowSimulator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 125);
			this.Controls.Add(this.btnPCDelete);
			this.Controls.Add(this.btnPCCreate);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtServiceName);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtMainConfig);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(300, 164);
			this.MinimumSize = new System.Drawing.Size(300, 164);
			this.Name = "WhizFlowSimulator";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WhizFlow Simulator";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMainConfig;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtServiceName;
		private System.Windows.Forms.Button btnPCCreate;
		private System.Windows.Forms.Button btnPCDelete;
	}
}

