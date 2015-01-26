namespace VisualOdometry.UI
{
	partial class MapForm
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
			this.m_Timer = new System.Windows.Forms.Timer(this.components);
			this.m_BottomPanel = new System.Windows.Forms.Panel();
			this.m_ZoomOutButton = new System.Windows.Forms.Button();
			this.m_ZoomInButton = new System.Windows.Forms.Button();
			this.m_AutoScaleCheckBox = new System.Windows.Forms.CheckBox();
			this.m_PictureBox = new System.Windows.Forms.PictureBox();
			this.m_BottomPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_PictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// m_BottomPanel
			// 
			this.m_BottomPanel.Controls.Add(this.m_ZoomOutButton);
			this.m_BottomPanel.Controls.Add(this.m_ZoomInButton);
			this.m_BottomPanel.Controls.Add(this.m_AutoScaleCheckBox);
			this.m_BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_BottomPanel.Location = new System.Drawing.Point(0, 373);
			this.m_BottomPanel.Name = "m_BottomPanel";
			this.m_BottomPanel.Size = new System.Drawing.Size(519, 35);
			this.m_BottomPanel.TabIndex = 1;
			// 
			// m_ZoomOutButton
			// 
			this.m_ZoomOutButton.Location = new System.Drawing.Point(149, 5);
			this.m_ZoomOutButton.Name = "m_ZoomOutButton";
			this.m_ZoomOutButton.Size = new System.Drawing.Size(27, 23);
			this.m_ZoomOutButton.TabIndex = 2;
			this.m_ZoomOutButton.Text = "-";
			this.m_ZoomOutButton.UseVisualStyleBackColor = true;
			this.m_ZoomOutButton.Click += new System.EventHandler(this.OnZoomOutButtonClicked);
			// 
			// m_ZoomInButton
			// 
			this.m_ZoomInButton.Location = new System.Drawing.Point(115, 5);
			this.m_ZoomInButton.Name = "m_ZoomInButton";
			this.m_ZoomInButton.Size = new System.Drawing.Size(28, 23);
			this.m_ZoomInButton.TabIndex = 1;
			this.m_ZoomInButton.Text = "+";
			this.m_ZoomInButton.UseVisualStyleBackColor = true;
			this.m_ZoomInButton.Click += new System.EventHandler(this.OnZoomInButtonClicked);
			// 
			// m_AutoScaleCheckBox
			// 
			this.m_AutoScaleCheckBox.AutoSize = true;
			this.m_AutoScaleCheckBox.Checked = true;
			this.m_AutoScaleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_AutoScaleCheckBox.Location = new System.Drawing.Point(13, 7);
			this.m_AutoScaleCheckBox.Name = "m_AutoScaleCheckBox";
			this.m_AutoScaleCheckBox.Size = new System.Drawing.Size(96, 21);
			this.m_AutoScaleCheckBox.TabIndex = 0;
			this.m_AutoScaleCheckBox.Text = "Auto scale";
			this.m_AutoScaleCheckBox.UseVisualStyleBackColor = true;
			// 
			// m_PictureBox
			// 
			this.m_PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_PictureBox.Location = new System.Drawing.Point(0, 0);
			this.m_PictureBox.Name = "m_PictureBox";
			this.m_PictureBox.Size = new System.Drawing.Size(519, 373);
			this.m_PictureBox.TabIndex = 2;
			this.m_PictureBox.TabStop = false;
			this.m_PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
			this.m_PictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
			this.m_PictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
			// 
			// MapForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(519, 408);
			this.Controls.Add(this.m_PictureBox);
			this.Controls.Add(this.m_BottomPanel);
			this.Name = "MapForm";
			this.Text = "Robot Path";
			this.m_BottomPanel.ResumeLayout(false);
			this.m_BottomPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_PictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer m_Timer;
		private System.Windows.Forms.Panel m_BottomPanel;
		private System.Windows.Forms.CheckBox m_AutoScaleCheckBox;
		private System.Windows.Forms.PictureBox m_PictureBox;
		private System.Windows.Forms.Button m_ZoomOutButton;
		private System.Windows.Forms.Button m_ZoomInButton;
	}
}