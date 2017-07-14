 


using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System;
using System.Linq;
using Stareater.GameData;
using Stareater.Players;

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

		private StellarisAdmin(IkonComposite rawData, ObjectDeindexer deindexer) : base(rawData, deindexer) 
		{
			 
			 
		}

		private StellarisAdmin() 
		{ }
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

		public static StellarisAdmin Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new StellarisAdmin(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		protected override string TableTag { get { return "StellarisAdmin"; } }
		private const string StarKey = "star";
		private const string OwnerKey = "owner";
 
		#endregion

 
	}
}
