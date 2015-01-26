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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace VisualOdometry.UI
{
	public partial class RotationAnalysisForm : Form
	{
		private static readonly double s_RadToDegree = 180.0 / Math.PI;

		private Histogram m_Histogram;
		private const string c_HistogramSeriesName = "Histogram";
		private const string c_CurrentHeadingChangeSeriesName = "CurrentHeading";
		private Series m_HistogramSeries;
		private Series m_CurrentHeadingChangeSeries;
		private VerticalLineAnnotation m_CurrentHeadingChangeAnnotation;

		public RotationAnalysisForm()
		{
			InitializeComponent();
			this.ShowInTaskbar = false;

			InitializeHistogram();
		}

		private void InitializeHistogram()
		{
			m_Histogram = new Histogram(-2.0, 2.0, 200);
			InitializeHistogramChart();
		}

		private void InitializeHistogramChart()
		{
			// Add new series
			m_HistogramSeries = m_AnglesChart.Series.Add(c_HistogramSeriesName);

			// Set new series chart type and other attributes
			m_HistogramSeries.ChartType = SeriesChartType.Column;
			m_HistogramSeries.BorderColor = Color.Black;
			m_HistogramSeries.BorderWidth = 1;
			m_HistogramSeries.BorderDashStyle = ChartDashStyle.Solid;
			m_HistogramSeries["PointWidth"] = "1.0";
			m_HistogramSeries["BarLabelStyle"] = "Center";

			// This series is solely used for anchoring the annotation that indicates the current heading change
			m_CurrentHeadingChangeSeries = m_AnglesChart.Series.Add(c_CurrentHeadingChangeSeriesName);
			m_CurrentHeadingChangeSeries.ChartType = SeriesChartType.Point;
			m_CurrentHeadingChangeSeries.ChartArea = m_HistogramSeries.ChartArea;
			m_CurrentHeadingChangeSeries.MarkerSize = 0;

			// Adjust chart area
			ChartArea chartArea = m_AnglesChart.ChartAreas[m_HistogramSeries.ChartArea];
			chartArea.AxisY.Title = "Frequency";
			chartArea.AxisX.Minimum = m_Histogram.Min;
			chartArea.AxisX.Maximum = m_Histogram.Max;

			chartArea.AxisX.Interval = 0.5;
			chartArea.AxisX.MajorGrid.Interval = 0.5;
			chartArea.AxisX.MajorTickMark.Interval = 0.1;

			m_CurrentHeadingChangeAnnotation = new VerticalLineAnnotation();

			m_CurrentHeadingChangeAnnotation.Height = -100;
			m_CurrentHeadingChangeAnnotation.LineWidth = 2;
			m_CurrentHeadingChangeAnnotation.LineColor = Color.Red;

			m_AnglesChart.Annotations.Add(m_CurrentHeadingChangeAnnotation);
		}

		internal void Update(VisualOdometer visualOdometer)
		{
			if (!this.Created)
			{
				return;
			}

			List<double> headingChanges = visualOdometer.RotationAnalyzer.MeasuredRotationIncrements;
			if (headingChanges.Count == 0)
			{
				return;
			}

			double[] degreeAngles = new double[headingChanges.Count];
			for (int i = 0; i < headingChanges.Count; i++)
			{
				degreeAngles[i] = headingChanges[i] * s_RadToDegree;
			}

			m_Histogram.Fill(degreeAngles);
			m_HistogramSeries.Points.Clear();

			for (int i = 0; i < m_Histogram.BinsCount; i++)
			{
				HistogramBin bin = m_Histogram[i];
				// Add data point into the histogram series
				double x = (bin.Min + bin.Max) / 2.0;
				m_HistogramSeries.Points.AddXY(x, bin.Count);
			}

			m_CurrentHeadingChangeSeries.Points.Clear();
			m_CurrentHeadingChangeSeries.Points.AddXY(visualOdometer.RotationAnalyzer.HeadingChange.Degrees, 0);
			m_CurrentHeadingChangeAnnotation.AnchorDataPoint = m_CurrentHeadingChangeSeries.Points[0];
		}
	}
}
