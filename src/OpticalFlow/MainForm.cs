/******************************************************************************

    Visual, monocular odometry for robots using a regular web cam.
    Copyright (C) 2010  Rainer Hessmer, PhD
    
    Based on the paper by Jason Campbell et al. "A Robust Visual Odometry
    and Precipice Detection System Using Consumer-grade Monocular Vision"
    http://www.cs.cmu.edu/~personalrover/PER/ResearchersPapers/CampbellSukthankarNourbakhshPahwa_VisualOdometryCR.pdf

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpticalFlow.Properties;

namespace OpticalFlow
{
	public partial class MainForm : Form
	{
		private MainModel m_Model;

		public MainForm(MainModel mainModel)
		{
			InitializeComponent();
			m_ImagesSplitter.SplitterDistance = m_ImagesSplitter.Width / 2;
			m_Model = mainModel;

			UpdateFromModel();
			Application.Idle += ProcessFrame;
			m_Model.Changed += new EventHandler(OnModelChanged);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Size formSize = Settings.Default.Size;
			if (formSize.Height != 0)
			{
				this.Size = formSize;
				this.Location = Settings.Default.Location;
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			// If the MainForm is not minimized, save the current Size and 
			// location to the settings file.  Otherwise, the existing values 
			// in the settings file will not change...
			if (this.WindowState != FormWindowState.Minimized)
			{
				Settings.Default.Size = this.Size;
				Settings.Default.Location = this.Location;
			}
		}

		private void OnModelChanged(object sender, EventArgs e)
		{
			UpdateFromModel();
		}

		private void UpdateFromModel()
		{
			m_MaxFeatureCountTextBox.Text = m_Model.MaxFeatureCount.ToString();
			m_BlockSizeTextBox.Text = m_Model.BlockSize.ToString();
			m_QaulityLevelTextBox.Text = m_Model.QualityLevel.ToString();
			m_MinDistanceTextBox.Text = m_Model.MinDistance.ToString();
		}

		private void ProcessFrame(object sender, EventArgs e)
		{
			m_Model.ProcessFrame();
			m_ImageBoxWithHeading.ImageBox.Image = m_Model.CurrentImage;
			m_FoundFeaturesCountTextBox.Text = m_Model.CurrentFoundFeaturesCount.ToString();
			m_TrackedFeaturesCountTextBox.Text = m_Model.TrackedFeaturesCount.ToString();
			m_NotTrackedFeaturesCount.Text = m_Model.NotTrackedFeaturesCount.ToString();

			m_FlowImageBoxWithHeading.ImageBox.Image = m_Model.FlowImage;
		}

		private void OnApplyButtonClicked(object sender, EventArgs e)
		{
			int maxFeatureCount;
			if (Int32.TryParse(m_MaxFeatureCountTextBox.Text, out maxFeatureCount))
			{
				m_Model.MaxFeatureCount = maxFeatureCount;
			}

			int blockSize;
			if (Int32.TryParse(m_BlockSizeTextBox.Text, out blockSize))
			{
				m_Model.BlockSize = blockSize;
			}

			double qualityLevel;
			if (Double.TryParse(m_QaulityLevelTextBox.Text, out qualityLevel))
			{
				m_Model.QualityLevel = qualityLevel;
			}

			double minDistance;
			if (Double.TryParse(m_MinDistanceTextBox.Text, out minDistance))
			{
				m_Model.MinDistance = minDistance;
			}
		}
	}
}
