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

namespace VisualOdometry
{
	public class Histogram
	{
		public double Min { get; private set; }
		public double Max { get; private set; }
		public double BinWidth { get; private set; }
		private HistogramBin[] m_Bins;
		private HistogramBin m_BelowMinBin;
		private HistogramBin m_AboveMaxBin;
		public int FullestBinIndex { get; private set; }

		public Histogram(double min, double max, int binCount)
		{
			this.Min = min;
			this.Max = max;
			this.BinWidth = (max - min) / binCount;
			m_Bins = new HistogramBin[binCount];

			double binMin = min;
			double binMax;

			for (int i = 0; i < binCount; i++)
			{
				binMax = min + (i + 1) * this.BinWidth;
				m_Bins[i] = new HistogramBin(binMin, binMax);
				binMin = binMax;
			}

			m_BelowMinBin = new HistogramBin(Double.MinValue, min);
			m_AboveMaxBin = new HistogramBin(max, Double.MaxValue);
		}

		public HistogramBin this[int binIndex]
		{
			get
			{
				if (binIndex == -1)
				{
					return m_BelowMinBin;
				}

				if (binIndex < m_Bins.Length)
				{
					return m_Bins[binIndex];
				}

				Debug.Assert(binIndex == m_Bins.Length);
				return m_AboveMaxBin;
			}
		}

		public int BinsCount
		{
			get { return m_Bins.Length; }
		}

		private void Clear()
		{
			m_BelowMinBin.Count = 0;
			m_AboveMaxBin.Count = 0;
			for (int i = 0; i < m_Bins.Length; i++)
			{
				m_Bins[i].Count = 0;
			}
		}

		public void Fill(double[] values)
		{
			Clear();
			this.FullestBinIndex = -1;
			Array.Sort<double>(values);
			int currentBinIndex = -1;
			HistogramBin currentBin = m_BelowMinBin;

			for (int i = 0; i < values.Length; i++)
			{
				double value = values[i];
				while (value > currentBin.Max)
				{
					currentBinIndex++;
					currentBin = this[currentBinIndex];
				}

				currentBin.Count++;

				if (currentBin.Count > this[this.FullestBinIndex].Count)
				{
					this.FullestBinIndex = currentBinIndex;
				}
			}
		}

		public int BelowMinCount
		{
			get { return m_BelowMinBin.Count; }
		}

		public int AboveMaxCount
		{
			get { return m_AboveMaxBin.Count; }
		}
	}
}
