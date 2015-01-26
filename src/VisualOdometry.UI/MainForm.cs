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
using VisualOdometry.UI.Properties;
using Emgu.CV;
using Emgu.CV.Structure;
using CameraCalibrator.Support;
using System.Configuration;

namespace VisualOdometry.UI
{
    public partial class MainForm : Form
    {
        private Capture m_Capture;
        private VisualOdometer m_VisualOdometer;
        private RotationAnalysisForm m_RotationAnalysisForm = new RotationAnalysisForm();
        private AuxiliaryViewsForm m_AuxiliaryViewsForm;
        private HomographyMatrix m_GroundProjectionTransformationForUI;
        private RobotPath m_RobotPath = new RobotPath();
        private MapForm m_MapForm;
        private double m_UnitFactor;
        private double[] m_UnitFactors = new double[]
		{
			1.0 / 1000.0, // m
			1.0 / 10.0,   // cm
			1.0,          // mm
			0.0032808399, // ft
			0.0393700787  // in
		};

        public MainForm()
        {
            InitializeComponent();
            m_UnitsComboBox.SelectedIndex = 0;

            CameraParameters cameraParameters = null;
            HomographyMatrix groundProjectionTransformation = null;

            bool useCamera = false;

            if (useCamera)
            {
                m_Capture = new Capture();
                m_Capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);
                m_Capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 720);

                cameraParameters = CameraParameters.Load(ConfigurationSettings.AppSettings["RootFilePath"].ToString() + @"CalibrationFiles\MicrosoftCinema\Focus14\1280x720\MicrosoftCinemaFocus14_1280x720.txt");

                groundProjectionTransformation = HomographyMatrixSupport.Load(ConfigurationSettings.AppSettings["RootFilePath"].ToString() + @"CalibrationFiles\MicrosoftCinema\Focus14\1280x720\BirdsEyeViewTransformationForCalculation.txt");
                m_GroundProjectionTransformationForUI = HomographyMatrixSupport.Load(ConfigurationSettings.AppSettings["RootFilePath"].ToString() + @"CalibrationFiles\MicrosoftCinema\Focus14\1280x720\BirdsEyeViewTransformationForUI.txt");
            }
            else
            {
                m_Capture = new Capture(ConfigurationSettings.AppSettings["RootFilePath"].ToString() + @"TestVideos\testvideo.wmv");


                m_Timer.Interval = 33;
                m_Timer.Enabled = true;

                cameraParameters = CameraParameters.Load(ConfigurationSettings.AppSettings["RootFilePath"].ToString() + @"CalibrationFiles\MicrosoftCinema\Focus12\1280x720\MicrosoftCinemaFocus12_1280x720.txt");

                groundProjectionTransformation = HomographyMatrixSupport.Load(ConfigurationSettings.AppSettings["RootFilePath"].ToString() + @"CalibrationFiles\MicrosoftCinema\Focus12\1280x720\BirdsEyeViewTransformationForCalculation.txt");
                m_GroundProjectionTransformationForUI = HomographyMatrixSupport.Load(ConfigurationSettings.AppSettings["RootFilePath"].ToString() + @"CalibrationFiles\MicrosoftCinema\Focus12\1280x720\BirdsEyeViewTransformationForUI.txt");
            }

            m_VisualOdometer = new VisualOdometer(m_Capture, cameraParameters, groundProjectionTransformation, new OpticalFlow());

            UpdateFromModel();

            m_VisualOdometer.Changed += new EventHandler(OnVisualOdometerChanged);
            Application.Idle += OnApplicationIdle;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            ProcessFrame();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Size formSize = Settings.Default.Size;
            if (formSize.Height != 0)
            {
                this.Size = formSize;
                this.Location = Settings.Default.Location;
            }
        }

        private void OnVisualOdometerChanged(object sender, EventArgs e)
        {
            UpdateFromModel();
        }

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            ProcessFrame();
        }

        private void ProcessFrame()
        {
            m_VisualOdometer.ProcessFrame();
            if (m_VisualOdometer.CurrentImage == null)
            {
                return;
            }

            m_FoundFeaturesCountTextBox.Text = m_VisualOdometer.InitialFeaturesCount.ToString();
            m_TrackedFeaturesCountTextBox.Text = m_VisualOdometer.TrackedFeatures.Count.ToString();
            m_NotTrackedFeaturesCount.Text = m_VisualOdometer.NotTrackedFeaturesCount.ToString();

            m_FrameTextBox.Text = m_VisualOdometer.FramesCounter.FrameNumber.ToString();
            m_FramesPerSecondTextBox.Text = String.Format("{0:0.0}", m_VisualOdometer.FramesCounter.FramesPerSecond);

            m_HeadingTextBox.Text = String.Format(
                "{0:0.000}", m_VisualOdometer.RobotPose.Heading.Degrees);
            m_LocationTextBox.Text = String.Format(
                "({0:0.000}, {1:0.000})", m_VisualOdometer.RobotPose.Location.X * m_UnitFactor, m_VisualOdometer.RobotPose.Location.Y * m_UnitFactor);
            //m_LocationChangeTextBox.Text = String.Format(
            //    "dx_r: {0:0.000}  dy_r: {1:0.000}", m_VisualOdometer.TranslationAnalyzer.LocationChange.X, m_VisualOdometer.TranslationAnalyzer.LocationChange.Y);

            m_PathLengthTextBox.Text = String.Format("{0:0.000}", m_RobotPath.PathLength * m_UnitFactor);
            m_DistanceFromStartTextBox.Text = String.Format("{0:0.000}", m_RobotPath.DistanceFromStart * m_UnitFactor);

            if (m_ShowImageCheckBox.Checked)
            {
                DrawRegionBounderies();
                m_ImageBox.Image = m_VisualOdometer.CurrentImage;
            }

            Pose newPose = new Pose(m_VisualOdometer.RobotPose);
            m_RobotPath.Add(newPose);

            if (!m_RotationAnalysisForm.IsDisposed)
            {
                m_RotationAnalysisForm.Update(m_VisualOdometer);
            }
            if (m_AuxiliaryViewsForm != null && !m_AuxiliaryViewsForm.IsDisposed)
            {
                m_AuxiliaryViewsForm.Update(m_DrawFeaturesCheckBox.Checked);
            }
            if (m_MapForm != null && !m_MapForm.IsDisposed)
            {
                m_MapForm.UpdateMap();
            }
            if (m_DrawFeaturesCheckBox.Checked)
            {
                DrawAllFeatureLocationsPreviousAndCurrent();
            }
        }

        private void DrawRegionBounderies()
        {
            DrawRegionBoundary(m_VisualOdometer.CurrentImage, m_VisualOdometer.SkyRegionBottom);
            DrawRegionBoundary(m_VisualOdometer.CurrentImage, m_VisualOdometer.GroundRegionTop);
        }

        private void DrawRegionBoundary(Image<Bgr, Byte> image, int yPos)
        {
            PointF start = new PointF(0, yPos);
            PointF end = new PointF(image.Width, yPos);
            LineSegment2DF lineSegment = new LineSegment2DF(start, end);
            image.Draw(lineSegment, new Bgr(Color.Red), 1);
        }

        private Bgr m_FeatureColorPreviousPartialHistory = new Bgr(Color.Yellow);
        private Bgr m_FeatureColorCurrentPartialHistory = new Bgr(Color.Orange);

        private Bgr m_FeatureColorPreviousFullHistory = new Bgr(Color.Lime);
        private Bgr m_FeatureColorCurrentFullHistory = new Bgr(Color.Red);

        private Bgr m_UsedGroundFeatureColor = new Bgr(Color.Blue);

        private void DrawAllFeatureLocationsPreviousAndCurrent()
        {
            List<TrackedFeature> trackedFeatures = m_VisualOdometer.TrackedFeatures;
            // draw previous location
            for (int i = 0; i < trackedFeatures.Count; i++)
            {
                TrackedFeature trackedFeature = trackedFeatures[i];
                if (trackedFeature.Count > 1)
                {
                    // We have a previous value
                    DrawPreviousFeatureLocation(trackedFeature[-1], trackedFeature.IsFull, m_VisualOdometer.CurrentImage);
                }
            }

            // draw current location
            for (int i = 0; i < trackedFeatures.Count; i++)
            {
                TrackedFeature trackedFeature = trackedFeatures[i];
                DrawCurrentFeatureLocation(trackedFeature[0], trackedFeature.IsFull, m_VisualOdometer.CurrentImage);
            }

            if (m_ShowGroundFeaturesCheckBox.Checked)
            {
                DrawUsedGroundFeatures();
            }
        }

        internal void DrawPreviousFeatureLocation(PointF previousFeatureLocation, bool hasFullHistory, Image<Bgr, Byte> image)
        {
            CircleF circle = new CircleF(previousFeatureLocation, 3.0f);
            if (!hasFullHistory)
            {
                image.Draw(circle, m_FeatureColorPreviousPartialHistory, 2);
            }
            else
            {
                image.Draw(circle, m_FeatureColorPreviousFullHistory, 2);
            }
        }

        internal void DrawCurrentFeatureLocation(PointF currentFeatureLocation, bool hasFullHistory, Image<Bgr, Byte> image)
        {
            CircleF circle = new CircleF(currentFeatureLocation, 3.0f);
            if (!hasFullHistory)
            {
                image.Draw(circle, m_FeatureColorCurrentPartialHistory, 2);
            }
            else
            {
                image.Draw(circle, m_FeatureColorCurrentFullHistory, 2);
            }
        }

        private void DrawUsedGroundFeatures()
        {
            List<TrackedFeature> usedGroundFeatures = m_VisualOdometer.TranslationAnalyzer.UsedGroundFeatures;
            // draw previous location
            for (int i = 0; i < usedGroundFeatures.Count; i++)
            {
                TrackedFeature usedGroundFeature = usedGroundFeatures[i];
                CircleF circle = new CircleF(usedGroundFeature[0], 6.0f);
                m_VisualOdometer.CurrentImage.Draw(circle, m_UsedGroundFeatureColor, 4);
            }
        }

        private void UpdateFromModel()
        {
            m_MaxFeatureCountTextBox.Text = m_VisualOdometer.OpticalFlow.MaxFeatureCount.ToString();
            m_BlockSizeTextBox.Text = m_VisualOdometer.OpticalFlow.BlockSize.ToString();
            m_QualityLevelTextBox.Text = m_VisualOdometer.OpticalFlow.QualityLevel.ToString();
            m_MinDistanceTextBox.Text = m_VisualOdometer.OpticalFlow.MinDistance.ToString();

            m_SkyBottomTextBox.Text = m_VisualOdometer.SkyRegionBottom.ToString();
            m_GroundTopTextBox.Text = m_VisualOdometer.GroundRegionTop.ToString();
        }

        private void OnApplyButtonClicked(object sender, EventArgs e)
        {
            int maxFeatureCount = m_VisualOdometer.OpticalFlow.MaxFeatureCount;
            Int32.TryParse(m_MaxFeatureCountTextBox.Text, out maxFeatureCount);

            int blockSize = m_VisualOdometer.OpticalFlow.BlockSize;
            Int32.TryParse(m_BlockSizeTextBox.Text, out blockSize);

            double qualityLevel = m_VisualOdometer.OpticalFlow.QualityLevel;
            Double.TryParse(m_QualityLevelTextBox.Text, out qualityLevel);

            double minDistance = m_VisualOdometer.OpticalFlow.MinDistance;
            Double.TryParse(m_MinDistanceTextBox.Text, out minDistance);

            OpticalFlow opticalFlow = new OpticalFlow(maxFeatureCount, blockSize, qualityLevel, minDistance);
            m_VisualOdometer.OpticalFlow = opticalFlow;

            int skyBottom;
            if (Int32.TryParse(m_SkyBottomTextBox.Text, out skyBottom))
            {
                m_VisualOdometer.SkyRegionBottom = skyBottom;
            }

            int groundTop;
            if (Int32.TryParse(m_GroundTopTextBox.Text, out groundTop))
            {
                m_VisualOdometer.GroundRegionTop = groundTop;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (m_Capture != null)
            {
                m_Capture.Dispose();
            }
            if (m_VisualOdometer != null)
            {
                m_VisualOdometer.Dispose();
            }

            // If the MainForm is not minimized, save the current Size and 
            // location to the settings file.  Otherwise, the existing values 
            // in the settings file will not change...
            if (this.WindowState != FormWindowState.Minimized)
            {
                Settings.Default.Size = this.Size;
                Settings.Default.Location = this.Location;
            }
            Settings.Default.Save();
        }

        private void OnDetailsButtonClicked(object sender, EventArgs e)
        {
            if (m_RotationAnalysisForm.IsDisposed)
            {
                m_RotationAnalysisForm = new RotationAnalysisForm();
            }
            m_RotationAnalysisForm.Show(this);
        }

        private void OnOtherViewsButtonClicked(object sender, EventArgs e)
        {
            if (m_AuxiliaryViewsForm == null || m_AuxiliaryViewsForm.IsDisposed)
            {
                m_AuxiliaryViewsForm = new AuxiliaryViewsForm(this, m_VisualOdometer, m_GroundProjectionTransformationForUI);
            }
            m_AuxiliaryViewsForm.Show(this);
        }

        private void OnMapButtonClicked(object sender, EventArgs e)
        {
            if (m_MapForm == null || m_MapForm.IsDisposed)
            {
                m_MapForm = new MapForm(m_RobotPath);
            }
            m_MapForm.Show(this);
        }

        private void OnSelectedUnitChanged(object sender, EventArgs e)
        {
            InitializeUnitFactor();
        }

        private void InitializeUnitFactor()
        {
            m_UnitFactor = m_UnitFactors[m_UnitsComboBox.SelectedIndex];
        }

        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            m_VisualOdometer.ResetOdometer();
            m_RobotPath.Clear();
            if (m_MapForm != null && !m_MapForm.IsDisposed)
            {
                m_MapForm.RedrawMap();
            }
        }
    }
}
