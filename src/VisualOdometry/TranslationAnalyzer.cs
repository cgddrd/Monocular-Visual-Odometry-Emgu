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
using System.Windows;
using System.Diagnostics;

namespace VisualOdometry
{
	public class TranslationAnalyzer
	{
		private VisualOdometer m_VisualOdometer;
		private HomographyMatrix m_GroundProjectionTransformation;
		private List<TrackedFeature> m_GroundFeatures;
		private List<TrackedFeature> m_UsedGroundFeatures;
		private List<TrackedFeature> m_ScratchPadUsedGroundFeatures;
		private Random m_Random = new Random();
		private List<Point> m_TranslationIncrements;
		private Point m_CurrentLocationChange;
		private Angle m_AcceptedDirectionMisalignment;

		internal TranslationAnalyzer(VisualOdometer visualOdometer, HomographyMatrix groundProjectionTransformation)
		{
			m_VisualOdometer = visualOdometer;
			m_GroundProjectionTransformation = groundProjectionTransformation;
			m_GroundFeatures = new List<TrackedFeature>();
			m_UsedGroundFeatures = new List<TrackedFeature>();
			m_ScratchPadUsedGroundFeatures = new List<TrackedFeature>();
			m_TranslationIncrements = new List<Point>();
			m_AcceptedDirectionMisalignment = Angle.FromDegrees(45);
		}

		public Angle AcceptedDirectionMisalignment
		{
			get { return m_AcceptedDirectionMisalignment; }
			set { m_AcceptedDirectionMisalignment = value; }
		}

		public Point LocationChange
		{
			get { return m_CurrentLocationChange; }
		}

		internal void CalculateTranslation(Angle headingChange)
		{
			PopulateRotationCorrectedTranslationIncrements(headingChange);
			DeterminMostLikelyTranslationVector();
		}

		private void PopulateRotationCorrectedTranslationIncrements(Angle headingChange)
		{
			double s = Math.Sin(headingChange.Rads);
			double c = Math.Cos(headingChange.Rads);

			m_TranslationIncrements.Clear();
			System.Drawing.PointF[] featurePointPair = new System.Drawing.PointF[2];

			List<TrackedFeature> trackedFeatures = m_VisualOdometer.TrackedFeatures;
			m_GroundFeatures.Clear();
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
				//Debug.WriteLine("Raw:");
				//Debug.WriteLine("\tPrevious dx_r: {0:0.000}  dy_r: {1:0.000}", featurePointPair[0].X, featurePointPair[0].Y);
				//Debug.WriteLine("\tCurrent  dx_r: {0:0.000}  dy_r: {1:0.000}", featurePointPair[1].X, featurePointPair[1].Y);

				ProjectOnFloor(featurePointPair);
				//Debug.WriteLine("Ground:");
				//Debug.WriteLine("\tPrevious dx_r: {0:0.000}  dy_r: {1:0.000}", featurePointPair[0].X, featurePointPair[0].Y);
				//Debug.WriteLine("\tCurrent  dx_r: {0:0.000}  dy_r: {1:0.000}", featurePointPair[1].X, featurePointPair[1].Y);


				// Remove rotation effect on current feature location. The center of the rotation is the previous feature location
				Point rotationCorrectedEndPoint = new Point(
					c * featurePointPair[1].X - s * featurePointPair[1].Y,
					s * featurePointPair[1].X + c * featurePointPair[1].Y);

				Point translationIncrement = new Point(
					featurePointPair[0].X - rotationCorrectedEndPoint.X,
					featurePointPair[0].Y - rotationCorrectedEndPoint.Y);

				//double translationAngle = Math.Abs(Math.Atan2(translationIncrement.X, translationIncrement.Y));
				////Debug.WriteLine(translationAngle * 180 / Math.PI);
				//if (translationAngle > acceptedDirectionMisaligment)
				//{
				//    continue;
				//}

				//m_UsedGroundFeatures.Add(trackedFeature);
				m_TranslationIncrements.Add(translationIncrement);
				m_GroundFeatures.Add(trackedFeature);
			}
		}

		private void DeterminMostLikelyTranslationVector()
		{
			Point mostLikelyTranslation = new Point();
			int maxVotes = 0;

			// We pick a couple of random translation vectors and then determine for which there exist the higest number of
			// similar translation vectors
			const int maxPicks = 40;
			int randomPicksCount = m_TranslationIncrements.Count < maxPicks ? m_TranslationIncrements.Count : maxPicks;
			for (int i = 0; i < randomPicksCount; i++)
			{
				m_ScratchPadUsedGroundFeatures.Clear();
				int index = m_Random.Next(m_TranslationIncrements.Count);
				Point translationVector = m_TranslationIncrements[index];

				double netX = 0, netY = 0;
				int votes = 0;

				for (int j = 0; j < m_TranslationIncrements.Count; j++)
				{
					if (i == j)
					{
						continue;
					}

					double dx = m_TranslationIncrements[j].X - translationVector.X;
					double dy = m_TranslationIncrements[j].Y - translationVector.Y;

					if ((dx * dx + dy * dy) < 0.5)
					{
						votes++;
						netX += dx;
						netY += dy;
						m_ScratchPadUsedGroundFeatures.Add(m_GroundFeatures[j]);
					}
				}

				if (votes > maxVotes)
				{
					maxVotes = votes;
					mostLikelyTranslation = new Point(
						translationVector.X + netX / votes,
						translationVector.Y + netY / votes);

					List<TrackedFeature> temp = m_UsedGroundFeatures;
					m_UsedGroundFeatures = m_ScratchPadUsedGroundFeatures;
					m_ScratchPadUsedGroundFeatures = temp;
				}
			}

			m_CurrentLocationChange = mostLikelyTranslation;
		}

		//internal void CalculateTranslationOld(Angle headingChange)
		//{
		//    m_UsedGroundFeatures.Clear();

		//    double s = Math.Sin(headingChange.Rads);
		//    double c = Math.Cos(headingChange.Rads);

		//    //double acceptedDirectionMisaligment = m_AcceptedDirectionMisalignment.Rads;

		//    m_TranslationIncrements.Clear();
		//    System.Drawing.PointF[] featurePointPair = new System.Drawing.PointF[2];

		//    //double sumX = 0, sumY = 0;
		//    int groundFeatureCount = 0;

		//    List<TrackedFeature> trackedFeatures = m_VisualOdometer.TrackedFeatures;
		//    for (int i = 0; i < trackedFeatures.Count; i++)
		//    {
		//        TrackedFeature trackedFeature = trackedFeatures[i];
		//        if (trackedFeature.Count < 2)
		//        {
		//            continue;
		//        }

		//        // previous and current feature points need to be in the ground region
		//        if (!(trackedFeature[-1].Y > m_VisualOdometer.GroundRegionTop && trackedFeature[0].Y > m_VisualOdometer.GroundRegionTop))
		//        {
		//            continue;
		//        }

		//        groundFeatureCount++;

		//        featurePointPair[0] = trackedFeature[-1]; // previous feature location
		//        featurePointPair[1] = trackedFeature[0];  // current featue location
		//        //Debug.WriteLine("Raw:");
		//        //Debug.WriteLine("\tPrevious dx_r: {0:0.000}  dy_r: {1:0.000}", featurePointPair[0].X, featurePointPair[0].Y);
		//        //Debug.WriteLine("\tCurrent  dx_r: {0:0.000}  dy_r: {1:0.000}", featurePointPair[1].X, featurePointPair[1].Y);

		//        ProjectOnFloor(featurePointPair);
		//        //Debug.WriteLine("Ground:");
		//        //Debug.WriteLine("\tPrevious dx_r: {0:0.000}  dy_r: {1:0.000}", featurePointPair[0].X, featurePointPair[0].Y);
		//        //Debug.WriteLine("\tCurrent  dx_r: {0:0.000}  dy_r: {1:0.000}", featurePointPair[1].X, featurePointPair[1].Y);


		//        // Remove rotation effect on current feature location. The center of the rotation is the previous feature location
		//        Point rotationCorrectedEndPoint = new Point(
		//            c * featurePointPair[1].X - s * featurePointPair[1].Y,
		//            s * featurePointPair[1].X + c * featurePointPair[1].Y);

		//        Point translationIncrement = new Point(
		//            featurePointPair[0].X - rotationCorrectedEndPoint.X,
		//            featurePointPair[0].Y - rotationCorrectedEndPoint.Y);

		//        //double translationAngle = Math.Abs(Math.Atan2(translationIncrement.X, translationIncrement.Y));
		//        ////Debug.WriteLine(translationAngle * 180 / Math.PI);
		//        //if (translationAngle > acceptedDirectionMisaligment)
		//        //{
		//        //    continue;
		//        //}

		//        //m_UsedGroundFeatures.Add(trackedFeature);
		//        //m_TranslationIncrements.Add(translationIncrement);
		//        //sumX += translationIncrement.X;
		//        //sumY += translationIncrement.Y;
		//    }

		//    //Debug.WriteLine("Used ground features %: " + ((double)m_TranslationIncrements.Count/(double)groundFeatureCount).ToString());

		//    //if (m_TranslationIncrements.Count > 0)
		//    //{
		//    //    m_CurrentLocationChange = new Point(sumX / m_TranslationIncrements.Count, sumY / m_TranslationIncrements.Count);
		//    //}
		//    //Debug.WriteLine("Average: dx_r: {0:0.000}  dy_r: {1:0.000}", m_CurrentLocationChange.X, m_CurrentLocationChange.Y);
		//}

		public System.Drawing.PointF RemoveRotationEffect(Angle headingChange, System.Drawing.PointF point)
		{
			float s = (float)Math.Sin(headingChange.Rads);
			float c = (float)Math.Cos(headingChange.Rads);

			return new System.Drawing.PointF(
				c * point.X - s * point.Y,
				s * point.X + c * point.Y);
		}

		private void ProjectOnFloor(System.Drawing.PointF[] featurePoints)
		{
			m_GroundProjectionTransformation.ProjectPoints(featurePoints);
		}

		public List<TrackedFeature> UsedGroundFeatures
		{
			get { return m_UsedGroundFeatures; }
		}
	}
}
