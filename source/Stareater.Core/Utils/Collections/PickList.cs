using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils.Collections
{
	public class PickList<T>
	{
		private static Random staticRng = new Random();
		private Random rng = null;

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
		
		public T PickOrTake(Predicate<T> shouldRemove)
		{
			return this.getRandom(shouldRemove);
		}
		
		public T Take()
		{
			return this.getRandom((x) => true);
		}

		private T getRandom(Predicate<T> removeAfter)
		{
			if (this.InnerList.Count < 1)
				return default(T);

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
