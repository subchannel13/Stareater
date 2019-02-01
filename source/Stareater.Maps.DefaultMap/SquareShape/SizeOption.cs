using Ikadn.Ikon.Types;

namespace Stareater.Maps.DefaultMap.SquareShape
{
	struct SizeOption
	{
		const string NameKey = "nameKey";
		const string SizeKey = "size";

		public string Name;
		public int Size;

		public SizeOption(IkonComposite data)
		{
			Name = data[NameKey].To<string>();
			Size = data[SizeKey].To<int>();
		}
	}
}
