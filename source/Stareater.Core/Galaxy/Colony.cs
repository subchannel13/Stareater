 

using Ikadn.Ikon.Types;
using System;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy 
{
	partial class Colony : AConstructionSite 
	{
		public double Population { get; set; }

		public Colony(double population, Planet planet, Player owner) : base(new LocationBody(planet.Star, planet), owner) 
		{
			this.Population = population;
 
		} 

		private Colony(Colony original, Planet planet, Player owner) : base(original, new LocationBody(planet.Star, planet), owner) 
		{
			this.Population = original.Population;
 
		}

		internal Colony Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new Colony(this, galaxyRemap.Planets[this.Location.Planet], playersRemap.Players[this.Owner]);
 
		} 
 

		#region Saving
		public override IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = base.Save(indexer);
			data.Add(PopulationKey, new IkonFloat(this.Population));
			return data;
 
		}

		protected override string TableTag { get { return "Colony"; } }
		private const string PopulationKey = "population";
		private const string PlanetKey = "planet";
		private const string OwnerKey = "owner";
 
		#endregion
	}
}
