namespace OpticalFlow
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
			this.m_TopPanel = new System.Windows.Forms.Panel();
			this.m_MinDistanceTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.m_QaulityLevelTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.m_BlockSizeTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.m_ApplyButton = new System.Windows.Forms.Button();
			this.m_MaxFeatureCountTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.m_BottomPanel = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.m_NotTrackedFeaturesCount = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.m_TrackedFeaturesCountTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.m_FoundFeaturesCountTextBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.m_CenterPanel = new System.Windows.Forms.Panel();
			this.m_ImagesSplitter = new System.Windows.Forms.SplitContainer();
			this.m_ImageBoxWithHeading = new Vision.WinForm.ImageBoxWithHeading();
			this.m_FlowImageBoxWithHeading = new Vision.WinForm.ImageBoxWithHeading();
			this.m_TopPanel.SuspendLayout();
			this.m_BottomPanel.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.m_CenterPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ImagesSplitter)).BeginInit();
			this.m_ImagesSplitter.Panel1.SuspendLayout();
			this.m_ImagesSplitter.Panel2.SuspendLayout();
			this.m_ImagesSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_TopPanel
			// 
			this.m_TopPanel.Controls.Add(this.m_MinDistanceTextBox);
			this.m_TopPanel.Controls.Add(this.label4);
			this.m_TopPanel.Controls.Add(this.m_QaulityLevelTextBox);
			this.m_TopPanel.Controls.Add(this.label3);
			this.m_TopPanel.Controls.Add(this.m_BlockSizeTextBox);
			this.m_TopPanel.Controls.Add(this.label2);
			this.m_TopPanel.Controls.Add(this.m_ApplyButton);
			this.m_TopPanel.Controls.Add(this.m_MaxFeatureCountTextBox);
			this.m_TopPanel.Controls.Add(this.label1);
			this.m_TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_TopPanel.Location = new System.Drawing.Point(0, 0);
			this.m_TopPanel.Name = "m_TopPanel";
			this.m_TopPanel.Size = new System.Drawing.Size(1402, 83);
			this.m_TopPanel.TabIndex = 0;
			// 
			// m_MinDistanceTextBox
			// 
			this.m_MinDistanceTextBox.Location = new System.Drawing.Point(403, 38);
			this.m_MinDistanceTextBox.Name = "m_MinDistanceTextBox";
			this.m_MinDistanceTextBox.Size = new System.Drawing.Size(68, 22);
			this.m_MinDistanceTextBox.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(262, 41);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(93, 17);
			this.label4.TabIndex = 7;
			this.label4.Text = "Min Distance:";
			// 
			// m_QaulityLevelTextBox
			// 
			this.m_QaulityLevelTextBox.Location = new System.Drawing.Point(154, 38);
			this.m_QaulityLevelTextBox.Name = "m_QaulityLevelTextBox";
			this.m_QaulityLevelTextBox.Size = new System.Drawing.Size(68, 22);
			this.m_QaulityLevelTextBox.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(94, 17);
			this.label3.TabIndex = 5;
			this.label3.Text = "Quality Level:";
			// 
			// m_BlockSizeTextBox
			// 
			this.m_BlockSizeTextBox.Location = new System.Drawing.Point(403, 10);
			this.m_BlockSizeTextBox.Name = "m_BlockSizeTextBox";
			this.m_BlockSizeTextBox.Size = new System.Drawing.Size(68, 22);
			this.m_BlockSizeTextBox.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(262, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "Block Size:";
			// 
			// m_ApplyButton
			// 
			this.m_ApplyButton.Location = new System.Drawing.Point(510, 38);
			this.m_ApplyButton.Name = "m_ApplyButton";
			this.m_ApplyButton.Size = new System.Drawing.Size(75, 23);
			this.m_ApplyButton.TabIndex = 2;
			this.m_ApplyButton.Text = "Apply";
			this.m_ApplyButton.UseVisualStyleBackColor = true;
			this.m_ApplyButton.Click += new System.EventHandler(this.OnApplyButtonClicked);
			// 
			// m_MaxFeatureCountTextBox
			// 
			this.m_MaxFeatureCountTextBox.Location = new System.Drawing.Point(154, 10);
			this.m_MaxFeatureCountTextBox.Name = "m_MaxFeatureCountTextBox";
			this.m_MaxFeatureCountTextBox.Size = new System.Drawing.Size(68, 22);
			this.m_MaxFeatureCountTextBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(131, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Max Feature Count:";
			// 
			// m_BottomPanel
			// 
			this.m_BottomPanel.Controls.Add(this.groupBox1);
			this.m_BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_BottomPanel.Location = new System.Drawing.Point(0, 578);
			this.m_BottomPanel.Name = "m_BottomPanel";
			this.m_BottomPanel.Size = new System.Drawing.Size(1402, 88);
			this.m_BottomPanel.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.m_NotTrackedFeaturesCount);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.m_TrackedFeaturesCountTextBox);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.m_FoundFeaturesCountTextBox);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1402, 88);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Features";
			// 
			// m_NotTrackedFeaturesCount
			// 
			this.m_NotTrackedFeaturesCount.Location = new System.Drawing.Point(271, 53);
			this.m_NotTrackedFeaturesCount.Name = "m_NotTrackedFeaturesCount";
			this.m_NotTrackedFeaturesCount.ReadOnly = true;
			this.m_NotTrackedFeaturesCount.Size = new System.Drawing.Size(68, 22);
			this.m_NotTrackedFeaturesCount.TabIndex = 9;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(175, 56);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(90, 17);
			this.label7.TabIndex = 8;
			this.label7.Text = "Not Tracked:";
			// 
			// m_TrackedFeaturesCountTextBox
			// 
			this.m_TrackedFeaturesCountTextBox.Location = new System.Drawing.Point(83, 53);
			this.m_TrackedFeaturesCountTextBox.Name = "m_TrackedFeaturesCountTextBox";
			this.m_TrackedFeaturesCountTextBox.ReadOnly = true;
			this.m_TrackedFeaturesCountTextBox.Size = new System.Drawing.Size(68, 22);
			this.m_TrackedFeaturesCountTextBox.TabIndex = 7;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(13, 56);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 17);
			this.label6.TabIndex = 6;
			this.label6.Text = "Tracked:";
			// 
			// m_FoundFeaturesCountTextBox
			// 
			this.m_FoundFeaturesCountTextBox.Location = new System.Drawing.Point(83, 23);
			this.m_FoundFeaturesCountTextBox.Name = "m_FoundFeaturesCountTextBox";
			this.m_FoundFeaturesCountTextBox.ReadOnly = true;
			this.m_FoundFeaturesCountTextBox.Size = new System.Drawing.Size(68, 22);
			this.m_FoundFeaturesCountTextBox.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(13, 26);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(52, 17);
			this.label5.TabIndex = 4;
			this.label5.Text = "Found:";
			// 
			// m_CenterPanel
			// 
			this.m_CenterPanel.Controls.Add(this.m_ImagesSplitter);
			this.m_CenterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_CenterPanel.Location = new System.Drawing.Point(0, 83);
			this.m_CenterPanel.Name = "m_CenterPanel";
			this.m_CenterPanel.Size = new System.Drawing.Size(1402, 495);
			this.m_CenterPanel.TabIndex = 2;
			// 
			// m_ImagesSplitter
			// 
			this.m_ImagesSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ImagesSplitter.Location = new System.Drawing.Point(0, 0);
			this.m_ImagesSplitter.Name = "m_ImagesSplitter";
			// 
			// m_ImagesSplitter.Panel1
			// 
			this.m_ImagesSplitter.Panel1.Controls.Add(this.m_ImageBoxWithHeading);
			// 
			// m_ImagesSplitter.Panel2
			// 
			this.m_ImagesSplitter.Panel2.Controls.Add(this.m_FlowImageBoxWithHeading);
			this.m_ImagesSplitter.Size = new System.Drawing.Size(1402, 495);
			this.m_ImagesSplitter.SplitterDistance = 677;
			this.m_ImagesSplitter.TabIndex = 1;
			// 
			// m_ImageBoxWithHeading
			// 
			this.m_ImageBoxWithHeading.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ImageBoxWithHeading.Heading = "Features to Track";
			this.m_ImageBoxWithHeading.Location = new System.Drawing.Point(0, 0);
			this.m_ImageBoxWithHeading.Name = "m_ImageBoxWithHeading";
			this.m_ImageBoxWithHeading.Size = new System.Drawing.Size(677, 495);
			this.m_ImageBoxWithHeading.TabIndex = 1;
			// 
			// m_FlowImageBoxWithHeading
			// 
			this.m_FlowImageBoxWithHeading.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_FlowImageBoxWithHeading.Heading = "Flow";
			this.m_FlowImageBoxWithHeading.Location = new System.Drawing.Point(0, 0);
			this.m_FlowImageBoxWithHeading.Name = "m_FlowImageBoxWithHeading";
			this.m_FlowImageBoxWithHeading.Size = new System.Drawing.Size(721, 495);
			this.m_FlowImageBoxWithHeading.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1402, 666);
			this.Controls.Add(this.m_CenterPanel);
			this.Controls.Add(this.m_BottomPanel);
			this.Controls.Add(this.m_TopPanel);
			this.Name = "MainForm";
			this.Text = "Optical Flow";
			this.m_TopPanel.ResumeLayout(false);
			this.m_TopPanel.PerformLayout();
			this.m_BottomPanel.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.m_CenterPanel.ResumeLayout(false);
			this.m_ImagesSplitter.Panel1.ResumeLayout(false);
			this.m_ImagesSplitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ImagesSplitter)).EndInit();
			this.m_ImagesSplitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_TopPanel;
		private System.Windows.Forms.Button m_ApplyButton;
		private System.Windows.Forms.TextBox m_MaxFeatureCountTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel m_BottomPanel;
		private System.Windows.Forms.Panel m_CenterPanel;
		private System.Windows.Forms.TextBox m_MinDistanceTextBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox m_QaulityLevelTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox m_BlockSizeTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox m_NotTrackedFeaturesCount;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox m_TrackedFeaturesCountTextBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox m_FoundFeaturesCountTextBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.SplitContainer m_ImagesSplitter;
		private Vision.WinForm.ImageBoxWithHeading m_ImageBoxWithHeading;
		private Vision.WinForm.ImageBoxWithHeading m_FlowImageBoxWithHeading;
	}
}

