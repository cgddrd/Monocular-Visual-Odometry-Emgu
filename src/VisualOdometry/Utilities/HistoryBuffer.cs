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
using System.Diagnostics;

namespace VisualOdometry.Utilities
{
	/// <summary>
	/// A cyclic buffer that holds the most recent value in slot 0, the previous value in slot -1, etc.
	/// </summary>
	public class HistoryBuffer<T> : CircularBuffer<T>
	{
		public HistoryBuffer(int size)
			: base(size)
		{
		}

		public override T this[int index]
		{
			get
			{
				if (index > 0 || index <= -this.Size)
				{
					string errorMessage = String.Format("Index must be between 0 and {0}.", -(this.Size - 1));
					throw new ArgumentException(errorMessage);
				}
				return base[this.Count - 1 + index];
			}
		}

		internal override void PrintContent(string heading)
		{
			Debug.WriteLine(heading);
			for (int i = 0; i > -this.Size; i--)
			{
				Debug.WriteLine("{0}: {1}", i, this[i]);
			}
			Debug.WriteLine("Is full: " + this.IsFull.ToString());
		}
	}
}
