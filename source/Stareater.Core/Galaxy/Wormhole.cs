using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	public class Wormhole
	{
		[StateProperty]
		public StarData FromStar { get; private set; }

		[StateProperty]
		public StarData ToStar { get; private set; }

		public Wormhole(StarData fromStar, StarData toStar) 
		{
			this.FromStar = fromStar;
			this.ToStar = toStar;
 		} 

		private Wormhole() 
		{ }
	}
}
