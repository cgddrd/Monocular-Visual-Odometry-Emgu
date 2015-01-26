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
using VisualOdometry.Utilities;

namespace VisualOdometry
{
	public class FramesCounter
	{
		private CircularBuffer<DateTime> m_FrameTimesBuffer = new CircularBuffer<DateTime>(10); // used for frames per seconds
		private double m_TicksPerSecond = (double)TimeSpan.FromSeconds(1).Ticks;

		internal FramesCounter()
		{
			this.FrameNumber = 0;
			this.FramesPerSecond = 0;
		}

		internal void Update()
		{
			m_FrameTimesBuffer.Add(DateTime.UtcNow);
			this.FrameNumber++;

			if (this.FrameNumber > 1)
			{
				this.FramesPerSecond = m_FrameTimesBuffer.Count * m_TicksPerSecond / (double)(m_FrameTimesBuffer[m_FrameTimesBuffer.Count - 1].Ticks - m_FrameTimesBuffer[0].Ticks);
			}
		}

		public int FrameNumber { get; private set; }
		public double FramesPerSecond { get; private set; }
	}
}
