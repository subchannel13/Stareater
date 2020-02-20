using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace Stareater.Utils.Collections
{
	[Serializable]
	public class PendableSet<T> : HashSet<T>, IDelayedCollection<T>
	{
		private List<T> toAdd = null;
		private List<T> toRemove = null;
		
		public PendableSet()
		{ }
			
		public PendableSet(IEnumerable<T> elements) : base(elements)
		{ }

		protected PendableSet(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
				throw new ArgumentNullException(nameof(info));

			this.toAdd = (List<T>)info.GetValue("toAdd", typeof(List<T>));
			this.toRemove = (List<T>)info.GetValue("toRemove", typeof(List<T>));
		}

		public void PendAdd(T element)
		{
			if (toAdd == null) toAdd = new List<T>();
			toAdd.Add(element);
		}

		public void PendRemove(T element)
		{
			if (toRemove == null) toRemove = new List<T>();
			toRemove.Add(element);
		}

		public void ApplyPending()
		{
			if (toRemove != null && toRemove.Count > 0) {
				foreach (var element in toRemove)
					this.Remove(element);
				toRemove.Clear();
			}
			
			if (toAdd != null && toAdd.Count > 0) {
				foreach (var element in toAdd)
					this.Remove(element);
				toAdd.Clear();
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException(nameof(info));

			base.GetObjectData(info, context);

			info.AddValue("toAdd", this.toAdd);
			info.AddValue("toRemove", this.toRemove);
		}
	}
}
