using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater
{
	class Tag<T>
	{
		public T Value;
		public string DisplayText;

		public Tag(T value, string displayText)
		{
			this.Value = value;
			this.DisplayText = displayText;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (this.GetType() != obj.GetType())
				return false;

			Tag<T> other = obj as Tag<T>;
			return Value.Equals(other.Value);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return DisplayText;
		}
	}
}
