 
using System;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Galaxy 
{
	abstract partial class AConstructionSite 
	{
		public LocationBody Location { get; private set; }
		public Player Owner { get; private set; }
		public IDictionary<string, double> Buildings { get; private set; }
		public IDictionary<Constructable, double> Stockpile { get; private set; }

		public AConstructionSite(LocationBody location, Player owner) : this() 
		{
			this.Location = location;
			this.Owner = owner;
			this.Buildings = new Dictionary<string, double>();
			this.Stockpile = new Dictionary<Constructable, double>();
 
		} 

		protected AConstructionSite(AConstructionSite original, LocationBody location, Player owner) : this(location, owner) 
		{
			copyBuildings(original);
			copyStockpile(original);
 
		}

 
	}
}
