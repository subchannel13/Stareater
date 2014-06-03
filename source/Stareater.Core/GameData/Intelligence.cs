 
using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;

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
 
	}
}
