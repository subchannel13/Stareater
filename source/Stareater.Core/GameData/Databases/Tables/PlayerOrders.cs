 

using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Databases.Tables 
{
	partial class PlayerOrders 
	{
		public int DevelopmentFocusIndex { get; set; }
		public Dictionary<string, int> DevelopmentQueue { get; set; }
		public string ResearchFocus { get; set; }
		public Dictionary<AConstructionSite, ConstructionOrders> ConstructionPlans { get; set; }

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
			this.DevelopmentQueue = new Dictionary<string, int>();
			foreach(var item in original.DevelopmentQueue)
				this.DevelopmentQueue.Add(item.Key, item.Value);
			this.ResearchFocus = original.ResearchFocus;
			this.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();
			foreach(var item in original.ConstructionPlans)
				this.ConstructionPlans.Add(playersRemap.Site(item.Key), item.Value.Copy());
 
		}

		internal PlayerOrders Copy(PlayersRemap playersRemap) 
		{
			return new PlayerOrders(this, playersRemap);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(DevelopmentFocusIndexKey, new IkonInteger(this.DevelopmentFocusIndex));

			data.Add(DevelopmentQueueKey, saveDevelopment());

			data.Add(ResearchFocusKey, new IkonText(this.ResearchFocus));

			data.Add(ConstructionPlansKey, saveConstruction(indexer));
			return data;
 
		}

		private const string TableTag = "PlayerOrders";
		private const string DevelopmentFocusIndexKey = "developmentFocusIndex";
		private const string DevelopmentQueueKey = "developmentQueue";
		private const string DevelopmentQueueTag = "developmentQueue";
		private const string stringKey = "string";
		private const string intKey = "int";
		private const string ResearchFocusKey = "researchFocus";
		private const string ConstructionPlansKey = "constructionPlans";
		private const string ConstructionPlansTag = "constructionPlans";
		private const string AConstructionSiteKey = "aconstructionsite";
		private const string ConstructionOrdersKey = "constructionorders";
 
		#endregion
	}
}
