using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati.Strukture
{
	class BinaryIndexedTree<T>
	{
		double totalSum = 0;
		List<KeyValuePair<T, double>> items = new List<KeyValuePair<T, double>>();
		Dictionary<T, int> itemIndices = new Dictionary<T, int>();

		public void Add(T item, double value)
		{
			itemIndices.Add(item, items.Count);
			items.Add(new KeyValuePair<T, double>(item, value));
			totalSum += value;
		}

		private int indexOf(double targetSum)
		{
			double sum = 0;
			targetSum *= totalSum;

			for (int i = 0; i < items.Count; i++)
				if (targetSum >= sum && targetSum < sum + items[i].Value) {
					return i;
				}
				else
					sum += items[i].Value;

			throw new ArgumentException("tragetSum must be from [0, 1>");
		}

		public T this[double targetSum]
		{
			get
			{
				return items[indexOf(targetSum)].Key;
			}
		}

		public bool isEmpty()
		{
			return (items.Count == 0);
		}

		public void Remove(T item)
		{
			int lastIndex = items.Count - 1;
			int itemIndex = itemIndices[item];
			totalSum -= items[itemIndex].Value;

			if (lastIndex != itemIndex) {
				items[itemIndex] = items[lastIndex];
				itemIndices[items[lastIndex].Key] = itemIndex;
			}
			itemIndices.Remove(item);
			items.RemoveAt(lastIndex);
		}
	}
}
