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
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using OpticalFlow.Properties;
using Emgu.CV.CvEnum;

namespace OpticalFlow
{
	public class MainModel : IDisposable
	{
		private const int c_WinSize = 10;
		private Capture m_Capture;
		public Image<Bgr, Byte> CurrentImage { get; private set; }
		public Image<Gray, Byte> PreviousGrayImage { get; private set; }
		public Image<Gray, Byte> CurrentGrayImage { get; private set; }
		public Image<Bgr, Byte> FlowImage { get; private set; }

		private int m_MaxFeatureCount;
		private int m_BlockSize;
		private double m_QualityLevel;
		private double m_MinDistance;
		public PointF[] CurrentFoundFeatures { get; private set; }
		public PointF[] PreviousFoundFeatures { get; private set; }
		public PointF[] TrackedFeatures { get; private set; }
		private byte[] m_TrackingStatus;

		private MCvTermCriteria m_SubCornerTerminationCriteria;
		private MCvTermCriteria m_OpticalFlowTerminationCriteria;

		private Image<Gray, Byte> m_PreviousPyrBufferParam;
		private Image<Gray, Byte> m_CurrentPyrBufferParam;

		public event EventHandler Changed;

		public MainModel()
		{
			m_Capture = new Capture();
			m_MaxFeatureCount = Settings.Default.MaxFeatureCount;
			m_BlockSize = Settings.Default.BlockSize;
			m_QualityLevel = Settings.Default.QualityLevel;
			m_MinDistance = Settings.Default.MinDistance;

			m_SubCornerTerminationCriteria.max_iter = 20;
			m_SubCornerTerminationCriteria.epsilon = 0.1;
			m_SubCornerTerminationCriteria.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_EPS | Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;

			m_OpticalFlowTerminationCriteria.max_iter = 20;
			m_OpticalFlowTerminationCriteria.epsilon = 0.3;
			m_OpticalFlowTerminationCriteria.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_EPS | Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;
		}

		public int MaxFeatureCount
		{
			get { return m_MaxFeatureCount; }
			set
			{
				if (value != m_MaxFeatureCount)
				{
					m_MaxFeatureCount = value;
					RaiseChangedEvent();
				}
			}
		}

		public int BlockSize
		{
			get { return m_BlockSize; }
			set
			{
				if (value != m_BlockSize)
				{
					m_BlockSize = value;
					RaiseChangedEvent();
				}
			}
		}

		public double QualityLevel
		{
			get { return m_QualityLevel; }
			set
			{
				if (value != m_QualityLevel)
				{
					m_QualityLevel = value;
					RaiseChangedEvent();
				}
			}
		}

		public double MinDistance
		{
			get { return m_MinDistance; }
			set
			{
				if (value != m_MinDistance)
				{
					m_MinDistance = value;
					RaiseChangedEvent();
				}
			}
		}

		public int PreviousFoundFeaturesCount
		{
			get
			{
				if (this.PreviousFoundFeatures == null)
				{
					return 0;
				}
				return this.PreviousFoundFeatures.Length;
			}
		}

		public int CurrentFoundFeaturesCount
		{
			get
			{
				if (this.CurrentFoundFeatures == null)
				{
					return 0;
				}
				return this.CurrentFoundFeatures.Length;
			}
		}

		public int TrackedFeaturesCount
		{
			get
			{
				if (this.TrackedFeatures == null)
				{
					return 0;
				}
				return this.TrackedFeatures.Length;
			}
		}

		public int NotTrackedFeaturesCount { get; private set; }

		public void ProcessFrame()
		{
			this.CurrentImage = m_Capture.QueryFrame();
			this.FlowImage = this.CurrentImage.Clone();

			this.PreviousGrayImage = this.CurrentGrayImage;
			this.CurrentGrayImage = this.CurrentImage.Convert<Gray, Byte>();

			PointF[][] foundFeaturesInChannels = this.CurrentGrayImage.GoodFeaturesToTrack(this.MaxFeatureCount, this.QualityLevel, this.MinDistance, this.BlockSize);

			// Next we find refine the location of the found features
			this.CurrentGrayImage.FindCornerSubPix(foundFeaturesInChannels, new Size(c_WinSize, c_WinSize), new Size(-1, -1), m_SubCornerTerminationCriteria);
			this.PreviousFoundFeatures = this.CurrentFoundFeatures;
			this.CurrentFoundFeatures = foundFeaturesInChannels[0];

			DrawFoundFeaturesMarkers();

			if (this.PreviousGrayImage == null)
			{
				m_PreviousPyrBufferParam = new Image<Gray, byte>(this.CurrentImage.Width + 8, this.CurrentImage.Height / 3);
				m_CurrentPyrBufferParam = new Image<Gray, byte>(this.CurrentImage.Width + 8, this.CurrentImage.Height / 3);
			}
			else
			{
				CalculateOpticalFlow();
				DrawTrackedFeaturesMarkers();
				DrawFlowVectors();
			}
		}

		private void DrawFoundFeaturesMarkers()
		{
			foreach (PointF foundFeature in this.CurrentFoundFeatures)
			{
				CircleF circle = new CircleF(foundFeature, 3.0f);
				this.CurrentImage.Draw(circle, new Bgr(Color.Lime), 2);
			}
		}

		private void CalculateOpticalFlow()
		{
			PointF[] trackedFeatures;
			float[] trackedErrors;

			LKFLOW_TYPE flags = LKFLOW_TYPE.DEFAULT;
			if (this.TrackedFeatures != null)
			{
				// We have a prefilled pyramid
				m_PreviousPyrBufferParam = m_CurrentPyrBufferParam;
				flags = LKFLOW_TYPE.CV_LKFLOW_PYR_A_READY;
			}

			Emgu.CV.OpticalFlow.PyrLK(
				this.PreviousGrayImage,
				this.CurrentGrayImage,
				m_PreviousPyrBufferParam,
				m_CurrentPyrBufferParam,
				this.PreviousFoundFeatures,
				new Size(c_WinSize, c_WinSize),
				5, // level
				m_OpticalFlowTerminationCriteria,
				flags,
				out trackedFeatures,
				out m_TrackingStatus,
				out trackedErrors);

			this.TrackedFeatures = trackedFeatures;

			int notTrackedFeatures = 0;
			for (int i = 0; i < m_TrackingStatus.Length; i++)
			{
				if (m_TrackingStatus[i] == 0)
				{
					notTrackedFeatures++;
				}
			}
			this.NotTrackedFeaturesCount = notTrackedFeatures;
		}

		private void DrawTrackedFeaturesMarkers()
		{
			for (int i = 0; i < this.TrackedFeatures.Length; i++)
			{
				if (m_TrackingStatus[i] == 1)
				{
					CircleF circle = new CircleF(this.TrackedFeatures[i], 3.0f);
					this.CurrentImage.Draw(circle, new Bgr(Color.Red), 2);
				}
			}
		}

		private void DrawFlowVectors()
		{
			for (int i = 0; i < this.TrackedFeatures.Length; i++)
			{
				if (m_TrackingStatus[i] == 1)
				{
					LineSegment2DF lineSegment = new LineSegment2DF(this.PreviousFoundFeatures[i], this.TrackedFeatures[i]);
					this.FlowImage.Draw(lineSegment, new Bgr(Color.Red), 1);
					CircleF circle = new CircleF(this.TrackedFeatures[i], 2.0f);
					this.FlowImage.Draw(circle, new Bgr(Color.Red), 1);
				}
			}
		}

		private void RaiseChangedEvent()
		{
			EventHandler handler = this.Changed;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public void Dispose()
		{
			Settings.Default.MaxFeatureCount = this.MaxFeatureCount;
			Settings.Default.BlockSize = this.BlockSize;
			Settings.Default.QualityLevel = this.QualityLevel;
			Settings.Default.MinDistance = this.MinDistance;

			if (m_Capture != null)
			{
				m_Capture.Dispose();
			}
		}
	}
}
