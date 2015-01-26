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
using Emgu.CV;
using Emgu.CV.Structure;
using CameraCalibrator.Support;
using System.Diagnostics;

namespace BirdsEyeView.UI
{
	public partial class MainForm : Form
	{
		private CameraParameters m_CameraParameters;
		public Image<Bgr, Byte> m_RawImage;
		public Image<Bgr, Byte> CurrentImage { get; private set; }
		public Image<Bgr, Byte> BirdsEyeImage { get; private set; }
		public ChessBoard ChessBoard { get; private set; }
		private Matrix<float> m_UndistortMapX;
		private Matrix<float> m_UndistortMapY;
		private HomographyMatrix m_BirdsEyeViewTransformationForUI;
		private HomographyMatrix m_BirdsEyeViewTransformationForCalculation;

		public MainForm()
		{
			InitializeComponent();
			m_CameraParameters = CameraParameters.Load(@"..\..\..\..\CalibrationFiles\MicrosoftCinema\Focus14\1280x720\MicrosoftCinemaFocus14_1280x720.txt");

			m_RawImage = new Image<Bgr, byte>(@"..\..\..\..\CalibrationFiles\MicrosoftCinema\Focus14\1280x720\GroundProjectionCalibration.jpg");
			this.CurrentImage = m_RawImage.Clone();
			this.BirdsEyeImage = m_RawImage.Clone();

			InitializeUndistortMap(m_RawImage);

			Undistort(m_RawImage, this.CurrentImage);

			this.ChessBoard = new ChessBoard(8, 10);
			PointF[] foundCorners = CollectImageCorners();

			DrawFoundCorners(this.CurrentImage, foundCorners);

			// We pick four corners for perspective transform
			PointF[] outerCorners = new PointF[4];
			outerCorners[0] = foundCorners[0];
			outerCorners[1] = foundCorners[this.ChessBoard.PatternSize.Width - 1];
			outerCorners[2] = foundCorners[this.ChessBoard.PatternSize.Width * this.ChessBoard.PatternSize.Height - this.ChessBoard.PatternSize.Width];
			outerCorners[3] = foundCorners[this.ChessBoard.PatternSize.Width * this.ChessBoard.PatternSize.Height - 1];
			DrawOuterCorners(this.CurrentImage, outerCorners);

			float side;
			float bottom;
			float centerX;

			side = 25.0f;
			bottom = 310.0f;

			PointF[] physicalPointsForCalculation = new PointF[4];
			physicalPointsForCalculation[0] = new PointF(-3 * side, bottom + 8 * side);
			physicalPointsForCalculation[1] = new PointF(+3 * side, bottom + 8 * side);
			physicalPointsForCalculation[2] = new PointF(-3 * side, bottom);
			physicalPointsForCalculation[3] = new PointF(+3 * side, bottom);

			m_BirdsEyeViewTransformationForCalculation = CameraCalibration.GetPerspectiveTransform(outerCorners, physicalPointsForCalculation);
			HomographyMatrixSupport.Save(m_BirdsEyeViewTransformationForCalculation, "BirdsEyeViewTransformationForCalculation.txt");

			side = 8f;
			bottom = 700.0f;
			centerX = (float)m_CameraParameters.Intrinsic.Cx;

			PointF[] physicalPointsForUI = new PointF[4];
			physicalPointsForUI[0] = new PointF(-3 * side + centerX, bottom - 8 * side);
			physicalPointsForUI[1] = new PointF(+3 * side + centerX, bottom - 8 * side);
			physicalPointsForUI[2] = new PointF(-3 * side + centerX, bottom);
			physicalPointsForUI[3] = new PointF(+3 * side + centerX, bottom);

			m_BirdsEyeViewTransformationForUI = CameraCalibration.GetPerspectiveTransform(outerCorners, physicalPointsForUI);
			HomographyMatrixSupport.Save(m_BirdsEyeViewTransformationForUI, "BirdsEyeViewTransformationForUI.txt");

			//m_BirdsEyeViewTransformationForCalculation.ProjectPoints(outerCorners);

			CreateAndDrawBirdsEyeView();
		}

		private void InitializeUndistortMap(Image<Bgr, Byte> image)
		{
			m_CameraParameters.IntrinsicCameraParameters.InitUndistortMap(
				image.Width,
				image.Height,
				out m_UndistortMapX,
				out m_UndistortMapY);
		}

		private void Undistort(Image<Bgr, Byte> sourceImage, Image<Bgr, Byte> targetImage)
		{
			CvInvoke.cvRemap(sourceImage.Ptr, targetImage.Ptr, m_UndistortMapX.Ptr, m_UndistortMapY.Ptr, (int)Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, new MCvScalar());
		}

		private PointF[] CollectImageCorners()
		{
			Image<Gray, Byte> grayImage = this.CurrentImage.Convert<Gray, Byte>();

			PointF[] foundCorners;
			bool foundAllCorners = CameraCalibration.FindChessboardCorners(
				grayImage,
				this.ChessBoard.PatternSize,
				Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS,
				out foundCorners);

			PointF[][] foundPointsForChannels = new PointF[][] { foundCorners };
			if (foundAllCorners)
			{
				MCvTermCriteria terminationCriteria;
				terminationCriteria.max_iter = 30;
				terminationCriteria.epsilon = 0.1;
				terminationCriteria.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_EPS | Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;

				grayImage.FindCornerSubPix(foundPointsForChannels, new Size(11, 11), new Size(-1, -1), terminationCriteria);
			}

			return foundCorners;
		}

		private void DrawFoundCorners(Image<Bgr, Byte> image, PointF[] foundCorners)
		{
			CvInvoke.cvDrawChessboardCorners(
			   image.Ptr,
			   this.ChessBoard.PatternSize,
			   foundCorners,
			   foundCorners.Length,
			   foundCorners.Length > 0 ? 1 : 0);
		}

		private void DrawOuterCorners(Image<Bgr, Byte> image, PointF[] outerCorners)
		{
			DrawCircle(image, outerCorners[0], new Bgr(255, 0, 0)); // blue
			DrawCircle(image, outerCorners[1], new Bgr(0, 255, 0)); // green
			DrawCircle(image, outerCorners[2], new Bgr(0, 0, 255)); // red
			DrawCircle(image, outerCorners[3], new Bgr(0, 255, 255)); // yellow
		}

		private void DrawCircle(Image<Bgr, Byte> image, PointF location, Bgr color)
		{
			CircleF circle = new CircleF(location, 9.0f);
			image.Draw(circle, color, 5);
		}

		private void CreateAndDrawBirdsEyeView()
		{
			double z = Double.Parse(m_ZTextBox.Text);

			//Debug.WriteLine(m_HomographyMatrix[2, 2]);
			m_BirdsEyeViewTransformationForUI[2, 2] = z;

			CvInvoke.cvWarpPerspective(
				this.CurrentImage.Ptr,
				this.BirdsEyeImage.Ptr,
				m_BirdsEyeViewTransformationForUI.Ptr,
				//(int)Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR | (int)Emgu.CV.CvEnum.WARP.CV_WARP_INVERSE_MAP | (int)Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS,
				(int)Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR | (int)Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS,
				new MCvScalar());

			m_CurrentImageImageBox.ImageBox.Image = this.CurrentImage;
			m_BirdsEyeViewImageBox.ImageBox.Image = this.BirdsEyeImage;
		}

		private void OnApplyButtonClick(object sender, EventArgs e)
		{
			CreateAndDrawBirdsEyeView();
		}
	}
}
