 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;

namespace Stareater.GameData.Databases.Tables 
{
	partial class PlayerOrders 
	{
		public int DevelopmentFocusIndex { get; set; }
		public Dictionary<string, int> DevelopmentQueue { get; set; }
		public string ResearchFocus { get; set; }
		public Dictionary<AConstructionSite, ConstructionOrders> ConstructionPlans { get; set; }
		public Dictionary<Vector2D, HashSet<Fleet>> ShipOrders { get; set; }
		public Dictionary<Planet, ColonizationPlan> ColonizationOrders { get; set; }

		public PlayerOrders() 
		{
			this.DevelopmentFocusIndex = 0;
			this.DevelopmentQueue = new Dictionary<string, int>();
			this.ResearchFocus = null;
			this.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();
			this.ShipOrders = new Dictionary<Vector2D, HashSet<Fleet>>();
			this.ColonizationOrders = new Dictionary<Planet, ColonizationPlan>();
 
			 
		} 

		private PlayerOrders(PlayerOrders original, PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			this.DevelopmentFocusIndex = original.DevelopmentFocusIndex;
			this.DevelopmentQueue = new Dictionary<string, int>();
			foreach(var item in original.DevelopmentQueue)
				this.DevelopmentQueue.Add(item.Key, item.Value);
			this.ResearchFocus = original.ResearchFocus;
			this.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();
			foreach(var item in original.ConstructionPlans)
				this.ConstructionPlans.Add(playersRemap.Site(item.Key), item.Value.Copy());
			this.ShipOrders = new Dictionary<Vector2D, HashSet<Fleet>>();
			foreach(var item in original.ShipOrders)
				this.ShipOrders.Add(item.Key, copyFleetRegroup(item.Value, playersRemap));
			this.ColonizationOrders = new Dictionary<Planet, ColonizationPlan>();
			foreach(var item in original.ColonizationOrders)
				this.ColonizationOrders.Add(galaxyRemap.Planets[item.Key], item.Value);
 
			 
		}

		private PlayerOrders(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var developmentFocusIndexSave = rawData[DevelopmentFocusIndexKey];
			this.DevelopmentFocusIndex = developmentFocusIndexSave.To<int>();

			var developmentQueueSave = rawData[DevelopmentQueueKey];
			this.DevelopmentQueue = loadDevelopmet(developmentQueueSave, deindexer);

			var researchFocusSave = rawData[ResearchFocusKey];
			this.ResearchFocus = researchFocusSave.To<string>();

			var constructionPlansSave = rawData[ConstructionPlansKey];
			this.ConstructionPlans = loadConstruction(constructionPlansSave, deindexer);

			var shipOrdersSave = rawData[ShipOrdersKey];
			this.ShipOrders = loadShipOrders(shipOrdersSave, deindexer);

			var colonizationOrdersSave = rawData[ColonizationOrdersKey];
			this.ColonizationOrders = loadColonizationOrders(colonizationOrdersSave, deindexer);
 
			 
		}

		internal PlayerOrders Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new PlayerOrders(this, playersRemap, galaxyRemap);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(DevelopmentFocusIndexKey, new IkonInteger(this.DevelopmentFocusIndex));

			data.Add(DevelopmentQueueKey, saveDevelopment());

			data.Add(ResearchFocusKey, new IkonText(this.ResearchFocus));

			data.Add(ConstructionPlansKey, saveConstruction(indexer));

			data.Add(ShipOrdersKey, saveShipOrders(indexer));

			data.Add(ColonizationOrdersKey, saveColonizationOrders(indexer));
			return data;
 
		}

		public static PlayerOrders Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new PlayerOrders(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
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
		private const string ShipOrdersKey = "shipOrders";
		private const string ShipOrdersTag = "shipOrders";
		private const string Position = "position";
		private const string Fleets = "fleets";
		private const string ColonizationOrdersKey = "colonizationOrders";
		private const string ColonizationOrdersTag = "colonizationOrders";
		private const string PlanetKey = "planet";
		private const string ColonizationPlanKey = "colonizationplan";
 
		#endregion

 
	}
}
