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
using VisualOdometry;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace VisualOdometry.UI
{
	public partial class MapForm : Form
	{
		private RobotPath m_RobotPath;
		private Bitmap m_Bitmap;
		private Graphics m_Graphics;

		private Pen m_RobotPen = new Pen(Color.Blue);
		private Pen m_PathPen = new Pen(Color.Black);

		private int m_CircleRadius = 6;
		private float m_ZoomFactor = 1.0f;

		private float m_OriginX = 0;
		private float m_OriginY = 0;
		private float m_TranslationX = 0.0f;
		private float m_TranslationY = 0.0f;

		public MapForm(RobotPath robotPath)
		{
			InitializeComponent();

			m_OriginX = m_PictureBox.Width / 2.0f;
			m_OriginY = -m_PictureBox.Height / 2.0f;

			m_RobotPath = robotPath;
			InitializeMapImage();
		}

		private void InitializeMapImage()
		{
			InitializeBitmap();
			m_PictureBox.Image = m_Bitmap;
			DrawFullPath();
		}

		private void InitializeBitmap()
		{
			m_Bitmap = new Bitmap(m_PictureBox.Width, m_PictureBox.Height);
			m_Graphics = Graphics.FromImage(m_Bitmap);

			Matrix matrix = new Matrix();
			matrix.Scale(1.0f, -1.0f);
			matrix.Translate(m_OriginX, m_OriginY);
			matrix.Translate(m_TranslationX, m_TranslationY);
			matrix.Scale(m_ZoomFactor, m_ZoomFactor);

			m_Graphics.Transform = matrix;
		}

		private void DrawFullPath()
		{
			//Debug.WriteLine("Drawing full path");
			m_Graphics.Clear(Color.White);
			for (int i = 0; i < m_RobotPath.Poses.Count; i++)
			{
				DrawPose(i);
			}
		}

		private void DrawPose(int index)
		{
			if (m_AutoScaleCheckBox.Checked)
			{
				PointF lowerLeftBound = new PointF((float)m_RobotPath.MinX, (float)m_RobotPath.MinY);
				PointF upperRightBound = new PointF((float)m_RobotPath.MaxX, (float)m_RobotPath.MaxY);
				
				PointF[] points = new PointF[] { lowerLeftBound, upperRightBound };

				m_Graphics.Transform.TransformPoints(points);

				PointF lowerLeftPixel = points[0];
				PointF upperRightPixel = points[1];
				if (lowerLeftPixel.X < 0 || lowerLeftPixel.Y > m_Bitmap.Height || upperRightPixel.X > m_Bitmap.Width || upperRightPixel.Y < 0)
				{
					ZoomOut();
					return;
				}
			}

			Pose pose = m_RobotPath.Poses[index];
			if (index % 5 == 0)
			{
				// Draw robot
				m_Graphics.DrawEllipse(
					m_RobotPen,
					RoundToInt(pose.X - m_CircleRadius),
					RoundToInt(pose.Y - m_CircleRadius),
					RoundToInt(2 * m_CircleRadius),
					RoundToInt(2 * m_CircleRadius));



				m_Graphics.DrawLine(
					m_RobotPen,
					RoundToInt(pose.X),
					RoundToInt(pose.Y),
					RoundToInt(pose.X - 2 * m_CircleRadius * Math.Sin(pose.Heading.Rads)),
					RoundToInt(pose.Y + 2 * m_CircleRadius * Math.Cos(pose.Heading.Rads)));
			}

			if (index > 0)
			{
				// draw line from last location
				Pose previousPose = m_RobotPath.Poses[index - 1];
				m_Graphics.DrawLine(
					m_PathPen,
					RoundToInt(previousPose.X),
					RoundToInt(previousPose.Y),
					RoundToInt(pose.X),
					RoundToInt(pose.Y));
			}
		}

		private int RoundToInt(double value)
		{
			return (int)(value + 0.5);
		}

		private void ZoomIn()
		{
			Zoom(1.25f);
		}

		private void ZoomOut()
		{
			Zoom(0.75f);
		}

		private void Zoom(float factor)
		{
			Matrix matrix = m_Graphics.Transform;
			matrix.Scale(factor, factor);
			m_ZoomFactor = matrix.Elements[0];
			m_Graphics.Transform = matrix;
			DrawFullPath();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (m_RobotPath != null)
			{
				InitializeMapImage();
			}
		}

		bool m_Dragging = false;
		Point m_LastPosition;

		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				m_Dragging = true;
				m_LastPosition = e.Location;
				//Debug.WriteLine("Dragging start");
			}
		}

		private void OnMouseUp(object sender, MouseEventArgs e)
		{
			m_Dragging = false;
			//Debug.WriteLine("Dragging end");
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (m_Dragging)
			{
				Point currentLocation = e.Location;

				float dX = (float)(currentLocation.X - m_LastPosition.X);
				float dY = -(float)(currentLocation.Y - m_LastPosition.Y);

				m_TranslationX += dX;
				m_TranslationY += dY;

				Matrix matrix = m_Graphics.Transform;
				matrix.Translate(dX / m_ZoomFactor, dY / m_ZoomFactor);
				m_Graphics.Transform = matrix;
				
				DrawFullPath();
				Refresh();

				m_LastPosition = e.Location;
			}
		}

		internal void UpdateMap()
		{
			DrawPose(m_RobotPath.Poses.Count - 1);
			Refresh();
		}

		private void OnZoomInButtonClicked(object sender, EventArgs e)
		{
			m_AutoScaleCheckBox.Checked = false;
			ZoomIn();
			Refresh();
		}

		private void OnZoomOutButtonClicked(object sender, EventArgs e)
		{
			m_AutoScaleCheckBox.Checked = false;
			ZoomOut();
			Refresh();
		}

		public void RedrawMap()
		{
			InitializeMapImage();
		}
	}
}
