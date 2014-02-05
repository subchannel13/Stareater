using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.GuiUtils
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
			Tag<T> other = obj as Tag<T>;

			if (other == null)
				return false;

			if (other.Value == null || Value == null)
				return false;
						
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
