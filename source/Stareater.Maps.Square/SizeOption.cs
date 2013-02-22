using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikon.Ston.Values;

namespace Stareater.Maps.Square
{
	struct SizeOption
	{
		const string NameKey = "nameKey";
		const string SizeKey = "size";

		public string Name;
		public int Size;

		public SizeOption(ObjectValue data)
		{
			Name = data[NameKey].To<string>();
			Size = data[SizeKey].To<int>();
		}
	}
}
