using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisualOdometry;

namespace Playground.UI
{
	public class Data
	{
		public Histogram Histogram { get; private set; }
		private Random m_Random = new Random();
		public List<double> Values { get; private set; }

		public Data()
		{
			this.Histogram = new Histogram(2.5, 18.5, 16);
			this.Values = new List<double>();
			RefillValues(1000);
		}

		public void RefillValues(int count)
		{
			this.Values.Clear();
			for (int index = 1; index < count; index++)
			{
				int dice1 = m_Random.Next(6) + 1;
				int dice2 = m_Random.Next(6) + 1;
				int dice3 = m_Random.Next(6) + 1;

				double value = dice1 + dice2 + dice3;
				this.Values.Add(value);
			}

			this.Histogram.Fill(this.Values.ToArray());
		}
	}
}
