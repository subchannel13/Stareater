using Ikadn.Ikon.Types;

namespace Stareater.Maps.DefaultMap.ProximityLanes
{
	struct DegreeOption
	{
		const string NameKey = "nameKey";
		const string RatioKey = "ratio";

		public string Name;
		public double Ratio;

		public DegreeOption(IkonComposite data)
		{
			this.Name = data[NameKey].To<string>();
			this.Ratio = data[RatioKey].To<double>();
		}
	}
}
