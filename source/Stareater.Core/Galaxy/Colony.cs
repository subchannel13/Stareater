using Stareater.Utils.StateEngine;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Galaxy
{
	[StateType(saveTag: Tag)]
	class Colony : AConstructionSite 
	{
		[StateProperty]
		public double Population { get; set; }

		public Colony(double population, Planet planet, Player owner) : base(new LocationBody(planet.Star, planet), owner) 
		{
			this.Population = population;
 		} 
		
		private Colony() 
		{ }

		public StarData Star
		{
			get
			{
				return Location.Star;
			}
		}

		public override SiteType Type
		{
			get { return SiteType.Colony; }
		}

		public const string Tag = "Colony";
	}
}
