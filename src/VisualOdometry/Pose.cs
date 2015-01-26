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
using System.Windows;

namespace VisualOdometry
{
	public class Pose
	{
		public Pose()
		{
			this.Location = new Point();
			this.Heading = Angle.FromRads(0);
		}

		public Pose(Point location, Angle heading)
		{
			this.Location = location;
			this.Heading = heading;
		}

		public Pose(double x, double y, Angle heading)
		{
			this.Location = new Point(x, y);
			this.Heading = heading;
		}

		public Pose(Pose pose)
		{
			this.Location = pose.Location;
			this.Heading = pose.Heading; ;
		}

		public Point Location { get; set; }

		public double X
		{
			get { return this.Location.X; }
			set { this.Location = new Point(value, this.Y); }
		}

		public double Y
		{
			get { return this.Location.Y; }
			set { this.Location = new Point(this.X, value); }
		}

		private Angle m_Heading;
		public Angle Heading
		{
			get { return m_Heading; }
			set { m_Heading = value; }
		}

		public override string ToString()
		{
			return String.Format("x={0}, y={1}, heading={2}", this.X, this.Y, this.Heading.Degrees);
		}
	}
}
