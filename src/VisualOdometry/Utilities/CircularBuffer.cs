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
	public class CircularBuffer<T>
	{
		private int m_Size;
		private T[] m_History;
		private int m_IndexForNextValue = 0;
		private int m_Count = 0;

		public CircularBuffer(int size)
		{
			m_Size = size;
			m_History = new T[m_Size];
		}

		public int Size
		{
			get { return m_Size; }
		}

		public int Count
		{
			get { return m_Count; }
		}

		public virtual void Add(T value)
		{
			if (m_Count < m_Size)
			{
				m_Count++;
			}

			m_History[m_IndexForNextValue] = value;
			m_IndexForNextValue = (m_IndexForNextValue + 1) % m_Size;
		}

		public bool IsFull
		{
			get { return (m_Count == m_Size); }
		}

		public virtual T this[int index]
		{
			get
			{
				int internalIndex = (m_Size + m_IndexForNextValue - m_Count + index) % m_Size;
				return m_History[internalIndex];
			}
		}

		internal virtual void PrintContent(string heading)
		{
			Debug.WriteLine(heading);
			for (int i = 0; i < m_Size; i++)
			{
				Debug.WriteLine("{0}: {1}", i, this[i]);
			}
			Debug.WriteLine("Is full: " + this.IsFull.ToString());
		}
	}
}
