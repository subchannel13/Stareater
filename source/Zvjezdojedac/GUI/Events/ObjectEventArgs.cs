using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.GUI.Events
{
	public class ObjectEventArgs<T> : EventArgs
	{
		public delegate void Handler(object sender, ObjectEventArgs<T> eventArgs);

		public ObjectEventArgs(T obj)
		{
			this.Value = obj;
		}

		public T Value { get; private set; }
	}
}
