 

using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Utils.Collections;

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

		private const string TableTag = "Intelligence";
		private const string StarKnowledgeKey = "StarIntell";
		private const string StarIntellTag = "StarIntell";
		private const string StarDataKey = "star";
		private const string StarIntelligenceKey = "intell";
 
		#endregion
	}
}
