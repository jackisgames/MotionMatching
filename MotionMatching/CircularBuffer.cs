using System.Dynamic;

namespace MotionMatching
{
    public class CircularBuffer<T>
    {
	    private T[] m_buffer;
	    private int m_start = 0;
	    private int m_end = 0;

		public CircularBuffer( int capacity )
	    {
			m_buffer = new T[capacity];
	    }

	    public void Push(T v)
	    {
		    m_buffer[m_end] = v;
		    m_end = (m_end + 1) % m_buffer.Length;

		    if (m_start == m_end)
		    {
			    m_start = (m_start + 1) % m_buffer.Length;
            }
	    }

	    public T this[int i]
	    {
		    get { return m_buffer[GetIndex(i)]; }
	    }

	    public int Length {
		    get
		    {
			    if (m_start == 0)
			    {
				    return m_end - m_start + 1;
			    }
			    return m_buffer.Length;
		    }
	    }

	    private int GetIndex(int i)//internal
	    {
		    int index = (m_start + i) % m_buffer.Length;

		    if (m_start <= m_end && index > m_end )
		    {
			    throw new System.IndexOutOfRangeException();
		    }

		    if (m_start > m_end && index > m_start && index < m_end)
		    {
			    throw new System.IndexOutOfRangeException();
		    }

            return index;

	    }
    }
}
