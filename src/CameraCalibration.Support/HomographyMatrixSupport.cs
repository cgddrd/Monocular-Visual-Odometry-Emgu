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
using System.IO;
using System.Globalization;

namespace CameraCalibrator.Support
{
	public class HomographyMatrixSupport
	{
		public static void Save(HomographyMatrix homographyMatrix, string filePath)
		{
			using (TextWriter writer = new StreamWriter(filePath))
			{
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						writer.WriteLine(homographyMatrix[x, y].ToString(CultureInfo.InvariantCulture));
					}
				}
			}
		}

		public static HomographyMatrix Load(string filePath)
		{
			HomographyMatrix homographyMatrix = new HomographyMatrix();
			using (TextReader reader = new StreamReader(filePath))
			{
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						homographyMatrix[x, y] = GetNextValue(reader);
					}
				}
			}

			return homographyMatrix;
		}

		private static double GetNextValue(TextReader reader)
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				line = line.Trim();
				if (line.Length == 0 || line.StartsWith("#"))
				{
					continue;
				}

				double value = double.Parse(line, CultureInfo.InvariantCulture);
				return value;
			}

			throw new EndOfStreamException("Unexpected end of file");
		}
	}
}
