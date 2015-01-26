namespace BirdsEyeView.UI
{
	partial class MainForm
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
			this.m_ImagesSplitter = new System.Windows.Forms.SplitContainer();
			this.m_TopPanel = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.m_BottomPanel = new System.Windows.Forms.Panel();
			this.m_CurrentImageImageBox = new Vision.WinForm.ImageBoxWithHeading();
			this.m_BirdsEyeViewImageBox = new Vision.WinForm.ImageBoxWithHeading();
			this.label1 = new System.Windows.Forms.Label();
			this.m_ApplyButton = new System.Windows.Forms.Button();
			this.m_ZTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.m_ImagesSplitter)).BeginInit();
			this.m_ImagesSplitter.Panel1.SuspendLayout();
			this.m_ImagesSplitter.Panel2.SuspendLayout();
			this.m_ImagesSplitter.SuspendLayout();
			this.m_TopPanel.SuspendLayout();
			this.m_BottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ImagesSplitter
			// 
			this.m_ImagesSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ImagesSplitter.Location = new System.Drawing.Point(0, 83);
			this.m_ImagesSplitter.Name = "m_ImagesSplitter";
			// 
			// m_ImagesSplitter.Panel1
			// 
			this.m_ImagesSplitter.Panel1.Controls.Add(this.m_CurrentImageImageBox);
			// 
			// m_ImagesSplitter.Panel2
			// 
			this.m_ImagesSplitter.Panel2.Controls.Add(this.m_BirdsEyeViewImageBox);
			this.m_ImagesSplitter.Size = new System.Drawing.Size(1328, 488);
			this.m_ImagesSplitter.SplitterDistance = 640;
			this.m_ImagesSplitter.TabIndex = 6;
			// 
			// m_TopPanel
			// 
			this.m_TopPanel.Controls.Add(this.m_ZTextBox);
			this.m_TopPanel.Controls.Add(this.m_ApplyButton);
			this.m_TopPanel.Controls.Add(this.label1);
			this.m_TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_TopPanel.Location = new System.Drawing.Point(0, 0);
			this.m_TopPanel.Name = "m_TopPanel";
			this.m_TopPanel.Size = new System.Drawing.Size(1328, 83);
			this.m_TopPanel.TabIndex = 4;
			// 
			// groupBox1
			// 
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1328, 88);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Features";
			// 
			// m_BottomPanel
			// 
			this.m_BottomPanel.Controls.Add(this.groupBox1);
			this.m_BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_BottomPanel.Location = new System.Drawing.Point(0, 571);
			this.m_BottomPanel.Name = "m_BottomPanel";
			this.m_BottomPanel.Size = new System.Drawing.Size(1328, 88);
			this.m_BottomPanel.TabIndex = 5;
			// 
			// m_CurrentImageImageBox
			// 
			this.m_CurrentImageImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_CurrentImageImageBox.Heading = "Current Image";
			this.m_CurrentImageImageBox.Location = new System.Drawing.Point(0, 0);
			this.m_CurrentImageImageBox.Name = "m_CurrentImageImageBox";
			this.m_CurrentImageImageBox.Size = new System.Drawing.Size(640, 488);
			this.m_CurrentImageImageBox.TabIndex = 0;
			// 
			// m_BirdsEyeViewImageBox
			// 
			this.m_BirdsEyeViewImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_BirdsEyeViewImageBox.Heading = "Bird\'s Eye View";
			this.m_BirdsEyeViewImageBox.Location = new System.Drawing.Point(0, 0);
			this.m_BirdsEyeViewImageBox.Name = "m_BirdsEyeViewImageBox";
			this.m_BirdsEyeViewImageBox.Size = new System.Drawing.Size(684, 488);
			this.m_BirdsEyeViewImageBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(21, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Z:";
			// 
			// m_ApplyButton
			// 
			this.m_ApplyButton.Location = new System.Drawing.Point(198, 10);
			this.m_ApplyButton.Name = "m_ApplyButton";
			this.m_ApplyButton.Size = new System.Drawing.Size(75, 23);
			this.m_ApplyButton.TabIndex = 2;
			this.m_ApplyButton.Text = "Apply";
			this.m_ApplyButton.UseVisualStyleBackColor = true;
			this.m_ApplyButton.Click += new System.EventHandler(this.OnApplyButtonClick);
			// 
			// m_ZTextBox
			// 
			this.m_ZTextBox.Location = new System.Drawing.Point(55, 10);
			this.m_ZTextBox.Name = "m_ZTextBox";
			this.m_ZTextBox.Size = new System.Drawing.Size(100, 22);
			this.m_ZTextBox.TabIndex = 3;
			this.m_ZTextBox.Text = "1";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1328, 659);
			this.Controls.Add(this.m_ImagesSplitter);
			this.Controls.Add(this.m_BottomPanel);
			this.Controls.Add(this.m_TopPanel);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.m_ImagesSplitter.Panel1.ResumeLayout(false);
			this.m_ImagesSplitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ImagesSplitter)).EndInit();
			this.m_ImagesSplitter.ResumeLayout(false);
			this.m_TopPanel.ResumeLayout(false);
			this.m_TopPanel.PerformLayout();
			this.m_BottomPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer m_ImagesSplitter;
		private System.Windows.Forms.Panel m_TopPanel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel m_BottomPanel;
		private Vision.WinForm.ImageBoxWithHeading m_CurrentImageImageBox;
		private Vision.WinForm.ImageBoxWithHeading m_BirdsEyeViewImageBox;
		private System.Windows.Forms.Button m_ApplyButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_ZTextBox;
	}
}

