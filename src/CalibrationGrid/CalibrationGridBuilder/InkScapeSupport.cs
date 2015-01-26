using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace GridBuilder
{
	public class InkScapeSupport
	{
		public const string InstallPath = @"C:\Program Files\Inkscape\inkscape.exe";

		public static void ConvertSvgToPdf(string pathToSvg, string pathToPdf)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(InstallPath);
			startInfo.Arguments = String.Format(
				CultureInfo.InvariantCulture,
				"-z --file=\"{0}\" --export-pdf=\"{1}\"",
				pathToSvg, pathToPdf);

			Process process = Process.Start(startInfo);
			process.WaitForExit();
		}
	}
}
