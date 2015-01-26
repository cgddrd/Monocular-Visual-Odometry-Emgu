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
using System.Drawing;

namespace VisualOdometry
{
	public class RotationAnalyzer
	{
		private VisualOdometer m_VisualOdometer;
		private double m_FocalLengthX;
		private double m_CenterX;

		private Angle m_HeadingChange;
		private List<double> m_RotationIncrements;

		internal RotationAnalyzer(VisualOdometer visualOdometer)
		{
			m_VisualOdometer = visualOdometer;
			m_FocalLengthX = visualOdometer.CameraParameters.Intrinsic.Fx;
			m_CenterX = visualOdometer.CameraParameters.Intrinsic.Cx;

			m_RotationIncrements = new List<double>();
		}

		public Angle HeadingChange
		{
			get { return m_HeadingChange; }
		}

		public List<double> MeasuredRotationIncrements
		{
			get { return m_RotationIncrements; }
		}

		internal void CalculateRotation()
		{
			m_RotationIncrements.Clear();

			List<TrackedFeature> trackedFeatures = m_VisualOdometer.TrackedFeatures;
			for (int i = 0; i < trackedFeatures.Count; i++)
			{
				TrackedFeature trackedFeature = trackedFeatures[i];
				if (trackedFeature.Count < 2)
				{
					continue;
				}
				PointF previousFeatureLocation = trackedFeature[-1];
				PointF currentFeatureLocation = trackedFeature[0];

				if (currentFeatureLocation.Y <= m_VisualOdometer.SkyRegionBottom)
				{
					double previousAngularPlacement = Math.Atan2(previousFeatureLocation.X - m_CenterX, m_FocalLengthX);
					double currentAngularPlacement = Math.Atan2(currentFeatureLocation.X - m_CenterX, m_FocalLengthX);
					double rotationIncrement = currentAngularPlacement - previousAngularPlacement;
					//Debug.WriteLine(headingChange * 180.0 / Math.PI);
					m_RotationIncrements.Add(rotationIncrement);
				}
			}

			//Debug.WriteLine("Max delta x: " + maxAbsDeltaX);
			if (m_RotationIncrements.Count > 0)
			{
				double meanRotationIncrement = DetermineBestRotationIncrement();
				m_HeadingChange = Angle.FromRads(meanRotationIncrement);
			}
		}

		private double DetermineBestRotationIncrement()
		{
			m_RotationIncrements.Sort();
			double meanRotationIncrement = m_RotationIncrements[m_RotationIncrements.Count / 2];
			return meanRotationIncrement;
		}
	}
}
