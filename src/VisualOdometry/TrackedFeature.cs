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
using VisualOdometry.Utilities;
using System.Diagnostics;

namespace VisualOdometry
{
	public class TrackedFeature : HistoryBuffer<PointF>
	{
		public const int HistoryCount = 7;
		private static readonly double s_SmoothTransitionLimit = 0.25;
		private static readonly double s_SmoothRotationLimit = 30.0 * Math.PI / 180;

		private int m_Score = 0;
		private int m_CurrentScoreChange = 0;
		private bool m_IsSmooth;

		public TrackedFeature()
			: base(HistoryCount)
		{
		}

		public override void Add(PointF featurePoint)
		{
			base.Add(featurePoint);
			if (this.IsFull)
			{
				GradeSmoothness();
			}
		}

		private void GradeSmoothness()
		{
			PointF point0 = this[0];
			PointF point1 = this[-1];
			PointF point2 = this[-2];
			PointF point6 = this[-6];

			// one back in history
			double dx1 = point0.X - point1.X;
			double dy1 = point0.Y - point1.Y;
			double direction1 = Math.Atan2(dy1, dx1);

			// two back in history
			double dx2 = point0.X - point2.X;
			double dy2 = point0.Y - point2.Y;
			double direction2 = Math.Atan2(dy2, dx2);

			// six back in history
			double dx6 = point0.X - point6.X;
			double dy6 = point0.Y - point6.Y;
			double direction6 = Math.Atan2(dy6, dx6);

			// simple distance (approximation of real Euclidean distance)
			double distance2 = Math.Abs(dx2) + Math.Abs(dy2);
			double distance1 = Math.Abs(dx1) + Math.Abs(dy1);

			double directionChange6to2 = Math.Abs(SubtractAngles(direction6, direction2));
			double directionChange2to1 = Math.Abs(SubtractAngles(direction2, direction1));

			bool previousChangeIsSmooth = distance2 < s_SmoothTransitionLimit || directionChange6to2 < s_SmoothRotationLimit;
			bool currentChangeIsSmooth = distance1 < s_SmoothTransitionLimit || directionChange2to1 < s_SmoothRotationLimit;

			if (previousChangeIsSmooth && currentChangeIsSmooth)
			{
				m_IsSmooth = true;
				m_CurrentScoreChange = -1;
			}
			else
			{
				m_IsSmooth = false;
				if (currentChangeIsSmooth && !previousChangeIsSmooth)
				{
					m_CurrentScoreChange = 5;
				}
				else
				{
					m_CurrentScoreChange = 8;
				}
			}
		}

		private double SubtractAngles(double a, double b)
		{
			double delta = a - b; ;
			while (delta >= Math.PI) { delta -= Math.PI; }
			while (delta < -Math.PI) { delta += Math.PI; }
			return delta;
		}

		public void ApplyScoreChange()
		{
			m_Score += m_CurrentScoreChange;
			//Debug.WriteLine(m_Score);
		}

		public int Score
		{
			get { return m_Score; }
		}

		public bool IsSmooth
		{
			get
			{
				if (!this.IsFull)
				{
					throw new InvalidOperationException("Full history is required to determine smoothness.");
				}
				return m_IsSmooth;
			}
		}

		public bool IsOut
		{
			get { return m_Score > 10; }
		}
	}
}
