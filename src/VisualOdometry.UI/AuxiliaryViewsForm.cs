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
using Emgu.CV;
using Emgu.CV.Structure;

namespace VisualOdometry.UI
{
	public partial class AuxiliaryViewsForm : Form
	{
		private MainForm m_MainForm;
		private VisualOdometer m_VisualOdometer;
		private HomographyMatrix m_GroundProjectionTransformation;
		private Image<Bgr, Byte> m_GroundProjectionImage;

		public AuxiliaryViewsForm(MainForm mainForm, VisualOdometer visualOdometer, HomographyMatrix groundProjectionTransformation)
		{
			InitializeComponent();
			this.ShowInTaskbar = false;
			m_MainForm = mainForm;
			m_VisualOdometer = visualOdometer;
			m_GroundProjectionTransformation = groundProjectionTransformation;
		}

		internal void Update(bool drawFeatures)
		{
			if (!this.Created)
			{
				return;
			}

			if (m_FeaturesMaskRadioButton.Checked)
			{
				m_ImageBox.Image = m_VisualOdometer.OpticalFlow.MaskImage.Clone();
			}
			if (m_GroundProjectionRadioButton.Checked)
			{
				if (m_GroundProjectionImage == null)
				{
					m_GroundProjectionImage = m_VisualOdometer.CurrentImage.Clone();
				}
				CvInvoke.cvWarpPerspective(
					m_VisualOdometer.CurrentImage.Ptr,
					m_GroundProjectionImage.Ptr,
					m_GroundProjectionTransformation.Ptr,
					(int)Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR | (int)Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS,
					new MCvScalar());

				if (drawFeatures)
				{
					DrawFeatures();
				}

				m_ImageBox.Image = m_GroundProjectionImage;
			}
		}

		private void DrawFeatures()
		{
			List<TrackedFeature> trackedFeatures = m_VisualOdometer.TrackedFeatures;
			System.Drawing.PointF[] featurePointPair = new System.Drawing.PointF[2];
			for (int i = 0; i < trackedFeatures.Count; i++)
			{
				TrackedFeature trackedFeature = trackedFeatures[i];
				if (trackedFeature.Count < 2)
				{
					continue;
				}

				// previous and current feature points need to be in the ground region
				if (!(trackedFeature[-1].Y > m_VisualOdometer.GroundRegionTop && trackedFeature[0].Y > m_VisualOdometer.GroundRegionTop))
				{
					continue;
				}

				featurePointPair[0] = trackedFeature[-1]; // previous feature location
				featurePointPair[1] = trackedFeature[0];  // current featue location

				m_GroundProjectionTransformation.ProjectPoints(featurePointPair);
				m_MainForm.DrawPreviousFeatureLocation(featurePointPair[0], trackedFeature.IsFull, m_GroundProjectionImage);

				Angle headingChange = m_VisualOdometer.RotationAnalyzer.HeadingChange;

				if (m_RemoveRotationEffectCheckBox.Checked)
				{
					PointF rotationCorrectedEndpoint = m_VisualOdometer.TranslationAnalyzer.RemoveRotationEffect(
						headingChange, featurePointPair[1]);
					m_MainForm.DrawCurrentFeatureLocation(rotationCorrectedEndpoint, trackedFeature.IsFull, m_GroundProjectionImage);
				}
				else
				{
					m_MainForm.DrawCurrentFeatureLocation(featurePointPair[1], trackedFeature.IsFull, m_GroundProjectionImage);
				}

				//// Remove rotation effect on current feature location. The center of the rotation is (0,0) on the ground plane
				//Point rotationCorrectedEndPoint = new Point(
				//    c * featurePointPair[1].X - s * featurePointPair[1].Y,
				//    s * featurePointPair[1].X + c * featurePointPair[1].Y);

				//Point translationIncrement = new Point(
				//    rotationCorrectedEndPoint.X - featurePointPair[0].X,
				//    rotationCorrectedEndPoint.Y - featurePointPair[0].Y);

				//m_TranslationIncrements.Add(translationIncrement);
				//sumX += translationIncrement.X;
				//sumY += translationIncrement.Y;
			}
		}
	}
}
