using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SvgNet.SvgTypes;
using SvgNet.SvgElements;
using System.Drawing;

namespace GridBuilder
{
	internal class CheckerBoardBuilder
	{
		private static readonly SvgStyle s_FilledBlack = "fill:#000000;stroke:none;fill-opacity:1";

		public static string BuildSvg(Page page, int xCount, int yCount, float squareLength)
		{
			CheckerBoardBuilder gridBuilder = new CheckerBoardBuilder(page, xCount, yCount, squareLength);
			return gridBuilder.BuildSvg();
		}

		private Page m_Page;
		private int m_XCount;
		private int m_YCount;
		private float m_SquareLength;
		private SvgSvgElement m_Root;
		private PointF m_UpperLeft;

		private CheckerBoardBuilder(Page page, int xCount, int yCount, float squareLength)
		{
			m_Page = page;
			m_XCount = xCount;
			m_YCount = yCount;
			m_SquareLength = squareLength;

			m_UpperLeft = new PointF(
				-(xCount * squareLength) / 2.0f,
				-(yCount * squareLength) / 2.0f);
		}

		private string BuildSvg()
		{
			m_Root = new SvgSvgElement(
				m_Page.SvgLengthWidth, m_Page.SvgLengthHeight,
				new SvgNumList(new float[] { -m_Page.Width / 2, -m_Page.Height / 2, m_Page.Width, m_Page.Height }));

			SvgGroupElement mainGroup = new SvgGroupElement("Main");

			for (int xIndex = 0; xIndex < m_XCount; xIndex++)
			{
				for (int yIndex = 0; yIndex < m_YCount; yIndex++)
				{
					InsertSquare(mainGroup, xIndex, yIndex);
				}
			}

			m_Root.AddChild(mainGroup);
			return m_Root.WriteSVGString(true, false);
		}

		private void InsertSquare(SvgGroupElement group, int xIndex, int yIndex)
		{
			bool isBlack = ((xIndex + yIndex) % 2 == 0);
			if (!isBlack)
			{
				// nothing to draw in case of a white square
				return;
			}

			float x = m_UpperLeft.X + xIndex * m_SquareLength;
			float y = m_UpperLeft.Y + yIndex * m_SquareLength;

			SvgRectElement rectangle = new SvgRectElement(
				x, y,
				m_SquareLength, m_SquareLength
				);
			rectangle.Style = s_FilledBlack;

			group.AddChild(rectangle);
		}
	}
}
