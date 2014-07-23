using System;
using System.Linq;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy
{
	partial class Colony : AConstructionSite
	{
		public StarData Star
		{
			get {
				return Location.Star;
			}
		}
		
		public override SiteType Type
		{
			get { return SiteType.Colony; }
		}
		
		protected override IkadnBaseObject saveLocation(ObjectIndexer indexer)
		{
			return new IkonComposite(LocationTag).Add(IdKey, new IkonInteger(indexer.IndexOf(Location.Planet)));
		}
		
		#region Saving keys
		private const string LocationTag = "Planet";
		private const string IdKey = "id";
 		#endregion
	}
}
