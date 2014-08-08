 

using Ikadn.Ikon.Types;
using System;

namespace Stareater.GameData 
{
	partial class PlanetIntelligence 
	{
		public double Explored { get; private set; }
		public int LastVisited { get; private set; }

		public PlanetIntelligence() 
		{
			this.Explored = Unexplored;
			this.LastVisited = NeverVisited;
 
		} 

		private PlanetIntelligence(PlanetIntelligence original) 
		{
			this.Explored = original.Explored;
			this.LastVisited = original.LastVisited;
 
		}

		internal PlanetIntelligence Copy() 
		{
			return new PlanetIntelligence(this);
 
		} 
 

		#region Saving
		public IkonComposite Save() 
		{
			var data = new IkonComposite(TableTag);
			data.Add(ExploredKey, new IkonFloat(this.Explored));

			data.Add(LastVisitedKey, new IkonInteger(this.LastVisited));
			return data;
 
		}

		private const string TableTag = "PlanetIntelligence";
		private const string ExploredKey = "explored";
		private const string LastVisitedKey = "lastVisited";
 
		#endregion
	}
}
