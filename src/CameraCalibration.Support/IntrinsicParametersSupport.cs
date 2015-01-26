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
using System.IO;
using Emgu.CV;

namespace CameraCalibrator.Support
{
	public static class IntrinsicParametersSupport
	{
		public static void Save(IntrinsicCameraParameters cameraParameters, string filePath)
		{
			using (TextWriter writer = new StreamWriter(filePath))
			{
				WriteIntrinsics(cameraParameters.IntrinsicMatrix, writer);
				writer.WriteLine();
				WriteDistortionCoeffs(cameraParameters.DistortionCoeffs, writer);
			}
		}

		private static void WriteIntrinsics(Matrix<double> intrinsicMatrix, TextWriter writer)
		{
			writer.WriteLine("# Intrinsic parameters:");
			Write("fx:", intrinsicMatrix[0, 0], writer);
			Write("fy:", intrinsicMatrix[1, 1], writer);
			Write("cx:", intrinsicMatrix[0, 2], writer);
			Write("cy:", intrinsicMatrix[1, 2], writer);
		}

		private static void WriteDistortionCoeffs(Matrix<double> distortionCoeffs, TextWriter writer)
		{
			writer.WriteLine("# Distortion parameters:");
			Write("k1:", distortionCoeffs[0, 0], writer);
			Write("k2:", distortionCoeffs[1, 0], writer);
			Write("k3:", distortionCoeffs[4, 0], writer);
			Write("p1:", distortionCoeffs[2, 0], writer);
			Write("p2:", distortionCoeffs[3, 0], writer);
		}

		private static void Write(string name, double value, TextWriter writer)
		{
			writer.WriteLine(name + "\t" + value.ToString(CultureInfo.InvariantCulture));
		}

		public static IntrinsicCameraParameters Load(string filePath)
		{
			Dictionary<string, double> valuesByName = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
			using (TextReader reader = new StreamReader(filePath))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					if (line.Length == 0 || line.StartsWith("#"))
					{
						continue;
					}

					string[] lineParts = line.Split('\t');
					string name = lineParts[0];
					double value = double.Parse(lineParts[1], CultureInfo.InvariantCulture);
					valuesByName.Add(name, value);
				}
			}

			IntrinsicCameraParameters cameraParameters = new IntrinsicCameraParameters();

			// Intrinsic parameters
			cameraParameters.IntrinsicMatrix[0, 0] = valuesByName["fx:"];
			cameraParameters.IntrinsicMatrix[1, 1] = valuesByName["fy:"];
			cameraParameters.IntrinsicMatrix[0, 2] = valuesByName["cx:"];
			cameraParameters.IntrinsicMatrix[1, 2] = valuesByName["cy:"];
			cameraParameters.IntrinsicMatrix[2, 2] = 1.0;

			// Distortion coefficients
			cameraParameters.DistortionCoeffs[0, 0] = valuesByName["k1:"];
			cameraParameters.DistortionCoeffs[1, 0] = valuesByName["k2:"];
			cameraParameters.DistortionCoeffs[2, 0] = valuesByName["p1:"];
			cameraParameters.DistortionCoeffs[3, 0] = valuesByName["p2:"];
			cameraParameters.DistortionCoeffs[4, 0] = valuesByName["k3:"];

			return cameraParameters;
		}
	}
}
