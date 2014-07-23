 
using Ikadn.Ikon.Types;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

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
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			IkonComposite data = new IkonComposite(TableTag);
			
			data.Add(FromStarKey, new IkonInteger(indexer.IndexOf(this.FromStar)));

			data.Add(ToStarKey, new IkonInteger(indexer.IndexOf(this.ToStar)));
 

			return data;
		}

		private const string TableTag = "Wormhole"; 
		private const string FromStarKey = "from";
		private const string ToStarKey = "to";
 
		#endregion
	}
}
