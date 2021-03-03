using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
	public class PriorityQueue<T> where T : IComparable<T>
	{
		List<T> _heap = new List<T>();

		public int Count { get { return _heap.Count; } }

		// O(logN)
		public void Push(T data)
		{
			// Insert data to end of the heap
			_heap.Add(data);

			int now = _heap.Count - 1;
			// Compare and re-sort
			while (now > 0)
			{
				// Compare insert node to parent node
				int next = (now - 1) / 2;
				if (_heap[now].CompareTo(_heap[next]) < 0)
					break; 

				// Swap
				T temp = _heap[now];
				_heap[now] = _heap[next];
				_heap[next] = temp;

				// move cursor to parent node
				now = next;
			}
		}

		// O(logN)
		public T Pop()
		{
			// Get return value
			T ret = _heap[0];

			// Move end data of list to root node
			int lastIndex = _heap.Count - 1;
			_heap[0] = _heap[lastIndex];
			_heap.RemoveAt(lastIndex);
			lastIndex--;

			// Compare and re-sort
			int now = 0;
			while (true)
			{
				int left = 2 * now + 1;
				int right = 2 * now + 2;

				int next = now;
				// Compare current index to left
				if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
					next = left;
				// Compare current index to right
				if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
					next = right;

				// Break if both left and right less than now
				if (next == now)
					break;

				// Swap
				T temp = _heap[now];
				_heap[now] = _heap[next];
				_heap[next] = temp;

				// move cursor to child node
				now = next;
			}

			return ret;
		}

		public T Peek()
		{
			if (_heap.Count == 0)
				return default(T);
			return _heap[0];
		}
	}
}

