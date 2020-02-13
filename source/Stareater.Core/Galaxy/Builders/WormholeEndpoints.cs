namespace Stareater.Galaxy.Builders 
{
	public class WormholeEndpoints 
	{
		public int FromIndex { get; private set; }
		public int ToIndex { get; private set; }

		public WormholeEndpoints(int fromIndex, int toIndex) 
		{
			this.FromIndex = fromIndex;
			this.ToIndex = toIndex;
		}
	}
}
