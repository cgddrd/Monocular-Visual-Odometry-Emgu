using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using VisualOdometry;

namespace Playground.UI
{
	public partial class Form1 : Form
	{
		private Data m_Data;
		private Series m_HistogramSeries = null;


		public Form1()
		{
			InitializeComponent();
			m_Data = new Data();
			//InitializeChart();
			InitializeHistogramChart();
		}

		private void InitializeChart()
		{
			Series series1 = new Series();
			m_Chart.Series.Add(series1);
			for (int index = 1; index < m_Data.Values.Count; index++)
			{
				double value = m_Data.Values[index];
				series1.Points.AddY(value);
			}

			// HistogramChartHelper is a helper class found in the samples Utilities folder. 
			HistogramChartHelper histogramHelper = new HistogramChartHelper();

			// Show the percent frequency on the right Y axis.
			histogramHelper.ShowPercentOnSecondaryYAxis = false;

			// Specify number of segment intervals
			//histogramHelper.SegmentIntervalNumber = 20;

			// Or you can specify the exact length of the interval
			histogramHelper.SegmentIntervalWidth = 1;

			// Create histogram series    
			histogramHelper.CreateHistogram(m_Chart, "Series1", "Histogram");
		}

		private void InitializeHistogramChart()
		{
			m_HistogramSeries = m_Chart.Series.Add("Histogram");

			// Set new series chart type and other attributes
			m_HistogramSeries.ChartType = SeriesChartType.Column;
			m_HistogramSeries.BorderColor = Color.Black;
			m_HistogramSeries.BorderWidth = 1;
			m_HistogramSeries.BorderDashStyle = ChartDashStyle.Solid;
			m_HistogramSeries["PointWidth"] = "1.0";
			m_HistogramSeries["BarLabelStyle"] = "Center";

			// Adjust chart area
			ChartArea chartArea = m_Chart.ChartAreas[m_HistogramSeries.ChartArea];
			//chartArea.AxisY.Title = "Frequency";
			chartArea.AxisX.Minimum = 3;
			chartArea.AxisX.Maximum = 18;

			//Series series1 = new Series();
			//m_Chart.Series.Add(series1);
			//for (int index = 3; index < 19; index++)
			//{
			//    m_HistogramSeries.Points.AddXY(index, 2 * index);
			//}
			for (int i = 0; i < m_Data.Histogram.BinsCount; i++)
			{
				HistogramBin bin = m_Data.Histogram[i];
				// Add data point into the histogram series
				double x = (bin.Min + bin.Max) / 2.0;
				m_HistogramSeries.Points.AddXY(x, bin.Count);
			}
		}
	}
}
