using System.Collections.Generic;
using System.Linq;

namespace Stareater.Utils.Collections
{
	public class PriorityQueue<TKey>
	{
		private readonly List<QueueItem> items = new List<QueueItem>();

		public int Count
		{
			get { return this.items.Count; }
		}

		public void Enqueue(TKey item, double priority)
		{
			int i = this.items.Count;
			this.items.Add(new QueueItem(item, priority));

			while (i > 0)
			{
				var parentI = (i - 1) / 2;

				if (this.items[i].Priority.CompareTo(this.items[parentI].Priority) >= 0)
					break;

				this.swap(i, parentI);
			}
		}

		public TKey Dequeue()
		{
			var result = this.items[0].Item;

			this.items[0] = this.items[this.items.Count - 1];
			this.items.RemoveAt(this.items.Count - 1);

			if (this.items.Count == 0)
				return result;

			int i = 0;
			while(true)
			{
				var child1 = 2 * i + 1;
				var child2 = 2 * i + 2;
				var child1Priority = child1 < this.items.Count ? this.items[child1].Priority : double.PositiveInfinity;
				var child2Priority = child2 < this.items.Count ? this.items[child2].Priority : double.PositiveInfinity;
				var priorityI = this.items[i].Priority;

				if (child1Priority >= priorityI && child2Priority >= priorityI)
					break;

				var nextI = child1Priority.CompareTo(child2Priority) <= 0 ? child1 : child2;
				this.swap(i, nextI);
				i = nextI;
			}

			return result;
		}

		public bool Contains(TKey item)
		{
			return this.items.Any(x => x.Item.Equals(item));
		}

		private void swap(int index1, int index2)
		{
			var temp = this.items[index1];
			this.items[index1] = this.items[index2];
			this.items[index2] = temp;
		}

		struct QueueItem
		{
			public readonly TKey Item;
			public readonly double Priority;

			public QueueItem(TKey item, double priority)
			{
				Item = item;
				Priority = priority;
			}
		}
	}
}
