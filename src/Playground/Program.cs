using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;

namespace Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{



				HistoryBuffer<int> history = new HistoryBuffer<int>(7);
				history.PrintContent("Empty -------");
				history.Add(1);
				history.Add(2);
				history.Add(3);
				history.PrintContent("3 in ------");
				history.Add(4);
				history.Add(5);
				history.Add(6);
				history.Add(7);
				history.PrintContent("Should be full ------");
				history.Add(8);
				history.Add(9);
				history.PrintContent("2 recycled ------");


				//IntrinsicCameraParameters intrinsicCameraParameters = new IntrinsicCameraParameters();
				//// We initialize the intrinsic matrix such that the two focal lengths have a ratio of 1.0
				//intrinsicCameraParameters.IntrinsicMatrix[0, 0] = 1.0;
				//intrinsicCameraParameters.IntrinsicMatrix[1, 1] = 1.0;

				//intrinsicCameraParameters.IntrinsicMatrix.Save("IntrinsixMatrix.xml");

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				Console.ReadLine();
			}
		}
	}
}
