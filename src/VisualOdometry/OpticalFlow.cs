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
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Runtime.InteropServices;

namespace VisualOdometry
{
	public class OpticalFlow : IDisposable
	{
		private const int c_WinSize = 10;

		private int m_MaxFeatureCount;
		private int m_BlockSize;
		private double m_QualityLevel;
		private double m_MinDistance;

		private MCvTermCriteria m_SubCornerTerminationCriteria;
		private MCvTermCriteria m_OpticalFlowTerminationCriteria;

		public OpticalFlowResult OpticalFlowResult { get; private set; }

		private Image<Gray, Byte> m_PreviousPyrBufferParam;
		private Image<Gray, Byte> m_CurrentPyrBufferParam;
		private Image<Gray, Byte> m_MaskImage;

		public event EventHandler Changed;

		public OpticalFlow()
			: this(
				OpticalFlowSettings.Default.MaxFeatureCount,
				OpticalFlowSettings.Default.BlockSize,
				OpticalFlowSettings.Default.QualityLevel,
				OpticalFlowSettings.Default.MinDistance)
		{
		}

		public OpticalFlow(int maxFeatureCount, int blockSize, double qualityLevel, double minDistance)
		{
			m_MaxFeatureCount = maxFeatureCount;
			m_BlockSize = blockSize;
			m_QualityLevel = qualityLevel;
			m_MinDistance = minDistance;

			m_SubCornerTerminationCriteria.max_iter = 20;
			m_SubCornerTerminationCriteria.epsilon = 0.1;
			m_SubCornerTerminationCriteria.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_EPS | Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;

			m_OpticalFlowTerminationCriteria.max_iter = 20;
			m_OpticalFlowTerminationCriteria.epsilon = 0.3;
			m_OpticalFlowTerminationCriteria.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_EPS | Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;

			this.OpticalFlowResult = new OpticalFlowResult();
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

		public Image<Gray, Byte> MaskImage
		{
			get { return m_MaskImage; }
		}

		internal PointF[] FindFeaturesToTrack(Image<Gray, Byte> grayImage, List<TrackedFeature> currentlyTrackedFeatures, int skyRegionBottom, int groundRegionTop)
		{
			if (m_MaskImage == null)
			{
				InitializeMaskImage(grayImage);
			}
			PointF[][] foundFeaturesInChannels = null;

			UpdateMaskImage(grayImage.Size, currentlyTrackedFeatures, skyRegionBottom, groundRegionTop);
			foundFeaturesInChannels = GoodFeaturesToTrack(grayImage, this.MaxFeatureCount, this.QualityLevel, this.MinDistance, this.BlockSize, m_MaskImage);

			//using (Image<Gray, Byte> maskImage = CreateMask(grayImage.Size, currentlyTrackedFeatures, skyRegionBottom, groundRegionTop))
			//{
			//    foundFeaturesInChannels = GoodFeaturesToTrack(grayImage, this.MaxFeatureCount, this.QualityLevel, this.MinDistance, this.BlockSize, maskImage);
			//}
			// Next we refine the location of the found features
			grayImage.FindCornerSubPix(foundFeaturesInChannels, new Size(c_WinSize, c_WinSize), new Size(-1, -1), m_SubCornerTerminationCriteria);
			return foundFeaturesInChannels[0];
		}

		private void InitializeMaskImage(Image<Gray, Byte> grayImage)
		{
			m_MaskImage = new Image<Gray, byte>(grayImage.Width, grayImage.Height);
		}

		/// <summary>
		/// Creates a mask for masking out the horizon region and circlea around each currently tracked feature.
		/// </summary>
		/// <param name="currentlyTrackedFeatures"></param>
		/// <param name="skyBottom"></param>
		/// <param name="groundTop"></param>
		private void UpdateMaskImage(Size size, List<TrackedFeature> currentlyTrackedFeatures, int skyRegionBottom, int groundRegionTop)
		{
			Gray blockedAreaColor = new Gray(0); // black
			Gray transparentAreaColor = new Gray(255); // white

			m_MaskImage.SetValue(transparentAreaColor); // fill

			// We mask out the area between the top of the ground region and the bottom of the sky region
			Rectangle rectangle = new Rectangle(0, skyRegionBottom, m_MaskImage.Width, groundRegionTop - skyRegionBottom);
			m_MaskImage.Draw(rectangle, blockedAreaColor, -1);

			// We also mask out a circle around each currently tracked feature to avoid the we find a new feature too close to an already tracked feature.
			int circleRadius = (int)(m_MinDistance + 0.5);
			for (int i = 0; i < currentlyTrackedFeatures.Count; i++)
			{
				CircleF circle = new CircleF(currentlyTrackedFeatures[i][0], circleRadius);
				m_MaskImage.Draw(circle, blockedAreaColor, -1);
			}
		}

		/// <summary>
		/// Finds corners with big eigenvalues in the image. Unfortunately we cannot use the function in Emgu since it does not support a mask. 
		/// </summary>
		/// <remarks>The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). The next step is rejecting the corners with the minimal eigenvalue less than quality_level?max(eig_image(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features</remarks>
		/// <param name="maxFeaturesPerChannel">The maximum features to be detected per channel</param>
		/// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
		/// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used. </param>
		/// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
		/// <param name="maskImage">If not null, can block out areas that feature points must be taken from.</param>
		/// <returns>The good features for each channel</returns>
		public PointF[][] GoodFeaturesToTrack(Image<Gray, Byte> grayImage, int maxFeaturesPerChannel, double qualityLevel, double minDistance, int blockSize, Image<Gray, Byte> maskImage)
		{
			using (Image<Gray, Single> eigImage = new Image<Gray, float>(grayImage.Width, grayImage.Height))
			using (Image<Gray, Single> tmpImage = new Image<Gray, float>(grayImage.Width, grayImage.Height))
			{
				int cornercount = maxFeaturesPerChannel;
				PointF[] pts = new PointF[maxFeaturesPerChannel];
				GCHandle handle = GCHandle.Alloc(pts, GCHandleType.Pinned);
				CvInvoke.cvGoodFeaturesToTrack(
					grayImage.Ptr,
					eigImage.Ptr,
					tmpImage.Ptr,
					handle.AddrOfPinnedObject(),
					ref cornercount,
					qualityLevel,
					minDistance,
					maskImage.Ptr,
					blockSize,
					0,
					0);
				handle.Free();
				Array.Resize(ref pts, cornercount);
				return new PointF[][] { pts };
			}
		}

		//internal void ClearPyramids()
		//{
		//    if (m_PreviousPyrBufferParam != null)
		//    {
		//        m_PreviousPyrBufferParam.Dispose();
		//    }
		//    m_PreviousPyrBufferParam = null;

		//    if (m_CurrentPyrBufferParam != null)
		//    {
		//        m_CurrentPyrBufferParam.Dispose();
		//    }
		//    m_CurrentPyrBufferParam = null;
		//}

		internal OpticalFlowResult CalculateOpticalFlow(Image<Gray, Byte> previousGrayImage, Image<Gray, Byte> currentGrayImage, PointF[] previousFoundFeaturePoints)
		{
			LKFLOW_TYPE flags = LKFLOW_TYPE.DEFAULT;
			if (m_PreviousPyrBufferParam != null)
			{
				// We have a prefilled pyramid
				m_PreviousPyrBufferParam = m_CurrentPyrBufferParam;
				flags = LKFLOW_TYPE.CV_LKFLOW_PYR_A_READY;
			}
			else
			{
				m_PreviousPyrBufferParam = new Image<Gray, byte>(currentGrayImage.Width + 8, currentGrayImage.Height / 3);
				m_CurrentPyrBufferParam = new Image<Gray, byte>(currentGrayImage.Width + 8, currentGrayImage.Height / 3);
			}

			PointF[] trackedFeaturePoints;
			float[] trackingErrors;
			byte[] trackingStatusIndicators;

			Emgu.CV.OpticalFlow.PyrLK(
				previousGrayImage,
				currentGrayImage,
				m_PreviousPyrBufferParam,
				m_CurrentPyrBufferParam,
				previousFoundFeaturePoints,
				new Size(c_WinSize, c_WinSize),
				5, // level
				m_OpticalFlowTerminationCriteria,
				flags,
				out trackedFeaturePoints,
				out trackingStatusIndicators,
				out trackingErrors);

			this.OpticalFlowResult.TrackedFeaturePoints = trackedFeaturePoints;
			this.OpticalFlowResult.TrackingStatusIndicators = trackingStatusIndicators;
			this.OpticalFlowResult.TrackingErrors = trackingErrors;

			return this.OpticalFlowResult;
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
			if (m_MaskImage != null)
			{
				m_MaskImage.Dispose();
			}
			OpticalFlowSettings.Default.MaxFeatureCount = this.MaxFeatureCount;
			OpticalFlowSettings.Default.BlockSize = this.BlockSize;
			OpticalFlowSettings.Default.QualityLevel = this.QualityLevel;
			OpticalFlowSettings.Default.MinDistance = this.MinDistance;
			OpticalFlowSettings.Default.Save();
		}
	}
}
