 
using Ikadn.Ikon.Types;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Galaxy 
{
	public class Wormhole 
	{
		public StarData FromStar { get; private set; }
		public StarData ToStar { get; private set; }

		public Wormhole(StarData fromStar, StarData toStar) 
		{
			this.FromStar = fromStar;
			this.ToStar = toStar;
 
		} 

		internal Wormhole Copy(GalaxyRemap galaxyRemap) 
		{
			return new Wormhole(galaxyRemap.Stars[this.FromStar], galaxyRemap.Stars[this.ToStar]);
 
		} 
 

		#region Saving
		public IkonComposite Save() 
		{
			IkonComposite data = new IkonComposite(TableTag);
			
			 

			return data;
		}

		private const string TableTag = "Wormhole"; 
		 
		#endregion
	}
}
