 
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
			copyStars(original, galaxyRemap);
 
		}

		internal Intelligence Copy(GalaxyRemap galaxyRemap) 
		{
			return new Intelligence(this, galaxyRemap);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			IkonComposite data = new IkonComposite(TableTag);
			
			data.Add(StarKnowledgeKey, saveStars(indexer));
 

			return data;
		}

		private const string TableTag = "Intelligence"; 
		private const string StarKnowledgeKey = "starKnowledge";
 
		#endregion
	}
}
