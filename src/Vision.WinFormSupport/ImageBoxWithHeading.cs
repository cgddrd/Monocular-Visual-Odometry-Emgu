using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.UI;

namespace Vision.WinForm
{
	public partial class ImageBoxWithHeading : UserControl
	{
		public ImageBoxWithHeading()
		{
			InitializeComponent();
		}

		public string Heading
		{
			get { return m_HeadingLabel.Text; }
			set { m_HeadingLabel.Text = value; }
		}

		public ImageBox ImageBox
		{
			get { return m_ImageBox; }
		}
	}
}
