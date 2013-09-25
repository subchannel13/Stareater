using System;
using Stareater.Galaxy;

namespace Stareater.Controllers
{
	public abstract class AConstructionSiteController
	{
		private AConstructionSite constructionSite;
		
		internal AConstructionSiteController(AConstructionSite constructionSite)
		{
			this.constructionSite = constructionSite;
		}
		
		
	}
}
