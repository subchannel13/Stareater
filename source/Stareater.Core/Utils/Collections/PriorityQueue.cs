using System;
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
			this.items.Add(new QueueItem(item, priority));
			this.bubbleUp(this.items.Count - 1);
		}

		public TKey Dequeue()
		{
			var result = this.items[0].Item;

			this.items[0] = this.items[this.items.Count - 1];
			this.items.RemoveAt(this.items.Count - 1);

			if (this.items.Count == 0)
				return result;

			this.bubbleDown(0);

			return result;
		}

		public void EnqueueOrUpdate(TKey item, double priority)
		{
			//TODO(later) track item indices in a dictionary
			var index = this.items.FindIndex(x => x.Item.Equals(item));

			if (index < 0)
			{
				this.Enqueue(item, priority);
				return;
			}

			var oldPriority = priority;
			this.items[index] = new QueueItem(item, priority);

			if (priority > oldPriority)
				this.bubbleDown(index);
			else
				this.bubbleUp(index);
		}

		public bool Contains(TKey item)
		{
			return this.items.Any(x => x.Item.Equals(item));
		}

		private void bubbleDown(int index)
		{
			while (true)
			{
				var child1 = 2 * index + 1;
				var child2 = 2 * index + 2;
				var child1Priority = child1 < this.items.Count ? this.items[child1].Priority : double.PositiveInfinity;
				var child2Priority = child2 < this.items.Count ? this.items[child2].Priority : double.PositiveInfinity;
				var priorityI = this.items[index].Priority;

				if (child1Priority >= priorityI && child2Priority >= priorityI)
					break;

				var nextI = child1Priority <= child2Priority ? child1 : child2;
				this.swap(index, nextI);
				index = nextI;
			}
		}

		private void bubbleUp(int index)
		{
			while (index > 0)
			{
				var parent = (index - 1) / 2;

				if (this.items[index].Priority >= this.items[parent].Priority)
					break;

				this.swap(index, parent);
				index = parent;
			}
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
				this.Item = item;
				this.Priority = priority;
			}

			public override string ToString()
			{
				return this.Priority + ": " + this.Item;
			}
		}
	}
}
