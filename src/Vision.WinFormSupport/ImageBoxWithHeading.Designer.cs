namespace Vision.WinForm
{
	partial class ImageBoxWithHeading
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_HeadingPanel = new System.Windows.Forms.Panel();
			this.m_HeadingLabel = new System.Windows.Forms.Label();
			this.m_ImageBox = new Emgu.CV.UI.ImageBox();
			this.m_HeadingPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ImageBox)).BeginInit();
			this.SuspendLayout();
			// 
			// m_HeadingPanel
			// 
			this.m_HeadingPanel.Controls.Add(this.m_HeadingLabel);
			this.m_HeadingPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_HeadingPanel.Location = new System.Drawing.Point(0, 0);
			this.m_HeadingPanel.Name = "m_HeadingPanel";
			this.m_HeadingPanel.Size = new System.Drawing.Size(235, 23);
			this.m_HeadingPanel.TabIndex = 1;
			// 
			// m_HeadingLabel
			// 
			this.m_HeadingLabel.AutoSize = true;
			this.m_HeadingLabel.Location = new System.Drawing.Point(3, 0);
			this.m_HeadingLabel.Name = "m_HeadingLabel";
			this.m_HeadingLabel.Size = new System.Drawing.Size(46, 17);
			this.m_HeadingLabel.TabIndex = 1;
			this.m_HeadingLabel.Text = "label1";
			// 
			// m_ImageBox
			// 
			this.m_ImageBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_ImageBox.Cursor = System.Windows.Forms.Cursors.Cross;
			this.m_ImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ImageBox.Location = new System.Drawing.Point(0, 23);
			this.m_ImageBox.Margin = new System.Windows.Forms.Padding(4);
			this.m_ImageBox.Name = "m_ImageBox";
			this.m_ImageBox.Size = new System.Drawing.Size(235, 160);
			this.m_ImageBox.TabIndex = 4;
			this.m_ImageBox.TabStop = false;
			// 
			// ImageBoxWithHeading
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_ImageBox);
			this.Controls.Add(this.m_HeadingPanel);
			this.Name = "ImageBoxWithHeading";
			this.Size = new System.Drawing.Size(235, 183);
			this.m_HeadingPanel.ResumeLayout(false);
			this.m_HeadingPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ImageBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_HeadingPanel;
		private System.Windows.Forms.Label m_HeadingLabel;
		private Emgu.CV.UI.ImageBox m_ImageBox;

	}
}
