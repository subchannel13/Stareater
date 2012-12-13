using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils.Collections
{
	public class PickList<T>
	{
		private static Random rng = new Random();

		public PickList()
		{
			this.InnerList = new List<T>();
		}

		public PickList(IEnumerable<T> list) : this()
		{
			this.InnerList.AddRange(list);
		}

		public T Pick()
		{
			return getRandom(false);
		}

		public T Take()
		{
			return getRandom(true);
		}

		private T getRandom(bool removeAfter)
		{
			if (InnerList.Count < 1)
				return default(T);

			int which = rng.Next(InnerList.Count);
			T ret = InnerList[which];
			if (removeAfter) {
				InnerList[which] = InnerList[InnerList.Count - 1];
				InnerList.RemoveAt(InnerList.Count - 1);
			}

			return ret;
		}

		public void Add(T element)
		{
			InnerList.Add(element);
		}

		public int Count()
		{
			return InnerList.Count;
		}

		public List<T> InnerList { get; private set; }
	}
}
