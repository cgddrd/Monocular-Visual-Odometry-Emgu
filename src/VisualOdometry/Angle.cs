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
using System.Globalization;

namespace VisualOdometry
{
	public struct Angle
	{
		private static readonly double s_TwoPI = 2 * Math.PI;
		private static readonly double s_RadToDegree = 180.0 / Math.PI;
		private static readonly double s_DegreeToRad = Math.PI / 180.0;

		/// <summary>
		/// Nomalizes the provided angle (degree) so that it falls in the range
		/// (-180, 180]
		/// </summary>
		/// <param name="angle">The angle to normalize in degree.</param>
		/// <returns></returns>
		public static Angle Normalize(Angle angle)
		{
			double factor = Math.Floor(angle.Rads / s_TwoPI);
			double normalizedAngleRad = angle.Rads - factor * s_TwoPI;
			if (normalizedAngleRad < 0)
			{
				normalizedAngleRad = angle.Rads + s_TwoPI;
			}

			if (normalizedAngleRad > Math.PI)
			{
				normalizedAngleRad = normalizedAngleRad - s_TwoPI;
			}
			return Angle.FromRads(normalizedAngleRad);
		}

		/// <summary>
		/// Nomalizes the provided angle (rad) so that it falls in the range
		/// (-pi, pi]
		/// </summary>
		/// <param name="rads">The angle to normalize in degree.</param>
		/// <returns></returns>
		public static double NormalizeRad(double rads)
		{
			double factor = Math.Floor(rads / s_TwoPI);
			double normalizedAngleRad = rads - factor * s_TwoPI;
			if (normalizedAngleRad < 0)
			{
				normalizedAngleRad = rads + s_TwoPI;
			}

			if (normalizedAngleRad > Math.PI)
			{
				normalizedAngleRad = normalizedAngleRad - s_TwoPI;
			}
			return normalizedAngleRad;
		}

		public static Angle FromRads(double rad)
		{
			return new Angle(rad);
		}

		public static Angle FromDegrees(double degree)
		{
			return new Angle(degree * s_DegreeToRad);
		}

		public static Angle operator +(Angle a1, Angle a2)
		{
			return new Angle(a1.Rads + a2.Rads);
		}

		public static Angle operator -(Angle a1, Angle a2)
		{
			return new Angle(a1.Rads - a2.Rads);
		}

		public static Angle operator /(Angle a1, double value)
		{
			return new Angle(a1.Rads / value);
		}

		public static Angle operator *(Angle a1, double value)
		{
			return new Angle(a1.Rads * value);
		}

		private Angle(double rad)
		{
			Rads = NormalizeRad(rad);
		}

		public double Degrees
		{
			get { return Rads * s_RadToDegree; }
		}

		public double Rads;

		public override string ToString()
		{
			return this.Degrees.ToString(CultureInfo.InvariantCulture) + " deg";
		}
	}
}
