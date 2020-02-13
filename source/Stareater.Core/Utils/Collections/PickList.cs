using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils.Collections
{
	public class PickList<T>
	{
		private static readonly Random staticRng = new Random();
		private readonly Random rng;

		public PickList()
		{
			this.InnerList = new List<T>();
		}

		public PickList(IEnumerable<T> list) : this()
		{
			this.InnerList.AddRange(list);
		}

		public PickList(Random rng)
		{
			this.InnerList = new List<T>();
			this.rng = rng;
		}

		public PickList(Random rng, IEnumerable<T> list)
			: this(rng)
		{
			this.InnerList.AddRange(list);
		}

		public T Pick()
		{
			return this.getRandom((x) => false);
		}
		
		public T PickOrTake(Predicate<T> removeCondition)
		{
			if (removeCondition == null)
				throw new ArgumentNullException(nameof(removeCondition));

			return this.getRandom(removeCondition);
		}
		
		//TODO(v0.8) map populator may use a method that return certain number of elements
		public T Take()
		{
			return this.getRandom((x) => true);
		}

		private T getRandom(Predicate<T> removeAfter)
		{
			if (this.InnerList.Count < 1)
				return default;

			int which = (this.rng ?? staticRng).Next(this.InnerList.Count);
			T ret = this.InnerList[which];
			
			if (removeAfter(ret)) {
				this.InnerList[which] = this.InnerList[this.InnerList.Count - 1];
				this.InnerList.RemoveAt(this.InnerList.Count - 1);
			}

			return ret;
		}

		public void Add(T element)
		{
			this.InnerList.Add(element);
		}

		public int Count()
		{
			return this.InnerList.Count;
		}

		public List<T> InnerList { get; private set; }
		
		public void RemoveAt(int i)
		{
			this.InnerList[i] = this.InnerList[this.InnerList.Count - 1];
			this.InnerList.RemoveAt(this.InnerList.Count - 1);
		}
	}
}
