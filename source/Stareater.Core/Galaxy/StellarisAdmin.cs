 
using Ikadn.Ikon.Types;
using System;
using System.Linq;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy 
{
	partial class StellarisAdmin : AConstructionSite 
	{
		
		public StellarisAdmin(StarData star, Player owner) : base(new LocationBody(star), owner) 
		{
			 
		} 

		private StellarisAdmin(StellarisAdmin original, StarData star, Player owner) : base(original, new LocationBody(star), owner) 
		{
			 
		}

		internal StellarisAdmin Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new StellarisAdmin(this, galaxyRemap.Stars[this.Location.Star], playersRemap.Players[this.Owner]);
 
		} 
 

		#region Saving
		public override IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = base.Save(indexer);
			return data;
 
		}

		private const string TableTag = "StellarisAdmin"; 
		private const string StarKey = "star";
		private const string OwnerKey = "owner";
 
		#endregion
	}
}
