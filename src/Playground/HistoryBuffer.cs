using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Playground
{
	/// <summary>
	/// A cyclic buffer that holds the most recent value in slot 0, the previous value in slot -1, etc.
	/// </summary>
	public class HistoryBuffer<T> : CircularBuffer<T>
	{
		public HistoryBuffer(int size)
			: base(size)
		{
		}

		public override T this[int index]
		{
			get
			{
				if (index > 0 || index <= -this.Size)
				{
					string errorMessage = String.Format("Index must be between 0 and {0}.", -(this.Size - 1));
					throw new ArgumentException(errorMessage);
				}
				return base[this.ValueCount - 1 + index];
			}
		}

		internal override void PrintContent(string heading)
		{
			Debug.WriteLine(heading);
			for (int i = 0; i > -this.Size; i--)
			{
				Debug.WriteLine("{0}: {1}", i, this[i]);
			}
			Debug.WriteLine("Is full: " + this.IsFull.ToString());
		}
	}
}
