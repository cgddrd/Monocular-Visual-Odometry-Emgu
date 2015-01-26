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
using Emgu.CV.Structure;
using System.Drawing;

namespace CameraCalibrator.Support
{
	public class ChessBoard
	{
		public int XCount { get; private set; }
		public int YCount { get; private set; }
		public int CornerCount { get; private set; }
		public Size PatternSize { get; private set; }
		public MCvPoint3D32f[] CornerPoints { get; private set; }

		public ChessBoard(int xCount, int yCount)
		{
			this.XCount = xCount;
			this.YCount = yCount;

			this.PatternSize = new Size(this.XCount - 1, this.YCount - 1);
			this.CornerCount = this.PatternSize.Width * this.PatternSize.Height;

			List<MCvPoint3D32f> cornerPoints = new List<MCvPoint3D32f>(this.CornerCount);
			for (int x = 0; x < this.XCount - 1; x++)
			{
				for (int y = 0; y < this.YCount - 1; y++)
				{
					MCvPoint3D32f cornerPoint = new MCvPoint3D32f(x * 25, y * 25, 0);
					cornerPoints.Add(cornerPoint);
				}
			}
			this.CornerPoints = cornerPoints.ToArray();
		}
	}
}
