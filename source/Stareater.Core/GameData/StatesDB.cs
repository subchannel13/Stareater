using System;
using Stareater.GameData.Tables;

namespace Stareater.GameData
{
	internal class StatesDB
	{
		public StarsCollection Stars { get; private set; }
		public WormholeCollection Wormholes { get; private set; }
		public PlanetsCollection Planets { get; private set; }
		
		public StatesDB(StarsCollection stars, WormholeCollection wormholes, PlanetsCollection planets)
		{
			this.Planets = planets;
			this.Stars = stars;
			this.Wormholes = wormholes;
		}
	}
}
