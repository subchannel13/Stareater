using System;
using Stareater.Galaxy;
using System.Collections.Generic;
using Stareater.Controllers.Data;

namespace Stareater.Controllers
{
	public abstract class AConstructionSiteController
	{
		private AConstructionSite constructionSite;
		
		internal AConstructionSiteController(AConstructionSite constructionSite)
		{
			this.constructionSite = constructionSite;
		}

		public abstract IEnumerable<ConstructableItem> ConstructableItems { get; }
		public abstract IEnumerable<ConstructableItem> ConstructionQueue { get; }
	}
}
