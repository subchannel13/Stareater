 

using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Utils.Collections;

namespace Stareater.GameData 
{
	partial class PlayerOrders 
	{
		public int DevelopmentFocusIndex { get; set; }
		public IDictionary<string, int> DevelopmentQueue { get; set; }
		public string ResearchFocus { get; set; }
		public IDictionary<AConstructionSite, ConstructionOrders> ConstructionPlans { get; set; }

		public PlayerOrders() 
		{
			this.DevelopmentFocusIndex = 0;
			this.DevelopmentQueue = new Dictionary<string, int>();
			this.ResearchFocus = null;
			this.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();
 
		} 

		private PlayerOrders(PlayerOrders original, PlayersRemap playersRemap) 
		{
			this.DevelopmentFocusIndex = original.DevelopmentFocusIndex;
			copyDevelopment(original);
			this.ResearchFocus = original.ResearchFocus;
			copyConstruction(original, playersRemap);
 
		}
		internal PlayerOrders Copy(PlayersRemap playersRemap) 
		{
			return new PlayerOrders(this, playersRemap);
 
		} 
 

		#region Saving
		public  IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(DevelopmentFocusIndexKey, new IkonInteger(this.DevelopmentFocusIndex));

			data.Add(ResearchFocusKey, new IkonText(this.ResearchFocus));
			return data;
 
		}

		private const string TableTag = "PlayerOrders"; 
		private const string DevelopmentFocusIndexKey = "developmentFocusIndex";
		private const string DevelopmentQueueKey = "developmentQueue";
		private const string ResearchFocusKey = "researchFocus";
		private const string ConstructionPlansKey = "constructionPlans";
 
		#endregion
	}
}
