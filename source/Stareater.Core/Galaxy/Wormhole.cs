using Stareater.Utils;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	public class Wormhole
	{
		[StateProperty]
		public Pair<StarData> Endpoints { get; private set; }

		public Wormhole(StarData fromStar, StarData toStar) 
		{
			this.Endpoints = new Pair<StarData>(fromStar, toStar);
 		} 

		private Wormhole() 
		{ }

		public override string ToString()
		{
			return this.Endpoints.First.ToString() + " - " + this.Endpoints.Second.ToString();
		}
	}
}
