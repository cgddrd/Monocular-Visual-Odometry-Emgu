using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Playground
{
	public class CircularBuffer<T>
	{
		private int m_Size;
		private T[] m_History;
		private int m_IndexForNextValue = 0;
		private int m_ValueCount = 0;

		public CircularBuffer(int size)
		{
			m_Size = size;
			m_History = new T[m_Size];
		}

		public int Size
		{
			get { return m_Size; }
		}

		public int ValueCount
		{
			get { return m_ValueCount; }
		}
	
		public void Add(T value)
		{
			if (m_ValueCount < m_Size)
			{
				m_ValueCount++;
			}

			m_History[m_IndexForNextValue] = value;
			m_IndexForNextValue = (m_IndexForNextValue + 1) % m_Size;
		}

		public bool IsFull
		{
			get { return (m_ValueCount == m_Size); }
		}

		public virtual T this[int index]
		{
			get
			{
				int internalIndex = (m_Size + m_IndexForNextValue - m_ValueCount + index) % m_Size;
				return m_History[internalIndex];
			}
		}

		internal virtual void PrintContent(string heading)
		{
			Debug.WriteLine(heading);
			for (int i = 0; i < m_Size; i++)
			{
				Debug.WriteLine("{0}: {1}", i, this[i]);
			}
			Debug.WriteLine("Is full: " + this.IsFull.ToString());
		}
	}
}
