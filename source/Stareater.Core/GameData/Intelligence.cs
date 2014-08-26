 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using Stareater.Galaxy;

namespace Stareater.GameData 
{
	partial class Intelligence 
	{
		private Dictionary<StarData, StarIntelligence> starKnowledge;

		public Intelligence() 
		{
			this.starKnowledge = new Dictionary<StarData, StarIntelligence>();
 
		} 

		private Intelligence(Intelligence original, GalaxyRemap galaxyRemap) 
		{
			this.starKnowledge = new Dictionary<StarData, StarIntelligence>();
			foreach(var item in original.starKnowledge)
				this.starKnowledge.Add(galaxyRemap.Stars[item.Key], item.Value.Copy(galaxyRemap));
 
		}

		private  Intelligence(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var starKnowledgeSave = rawData[StarKnowledgeKey];
			this.starKnowledge = new Dictionary<StarData, StarIntelligence>();
			foreach(var item in starKnowledgeSave.To<IEnumerable<IkonComposite>>()) {
				var itemKey = item[StarDataKey];
				var itemValue = item[StarIntelligenceKey];
				this.starKnowledge.Add(
					deindexer.Get<StarData>(itemKey.To<int>()),
					StarIntelligence.Load(itemValue.To<IkonComposite>(), deindexer)
				);
			}
 
		}

		internal Intelligence Copy(GalaxyRemap galaxyRemap) 
		{
			return new Intelligence(this, galaxyRemap);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			var starKnowledgeData = new IkonArray();
			foreach(var item in this.starKnowledge) {
				var itemData = new IkonComposite(StarIntellTag);
				itemData.Add(StarDataKey, new IkonInteger(indexer.IndexOf(item.Key)));
				itemData.Add(StarIntelligenceKey, item.Value.Save(indexer));
				starKnowledgeData.Add(itemData);
			}
			data.Add(StarKnowledgeKey, starKnowledgeData);
			return data;
 
		}
		
		public static Intelligence Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new Intelligence(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}

		private const string TableTag = "Intelligence";
		private const string StarKnowledgeKey = "starKnowledge";
		private const string StarIntellTag = "StarIntell";
		private const string StarDataKey = "star";
		private const string StarIntelligenceKey = "intell";
 
		#endregion
	}
}
