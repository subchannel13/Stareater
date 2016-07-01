using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Library;

namespace Stareater.Controllers
{
	public class LibraryController
	{
		private readonly GameController gameController;
		
		internal LibraryController(GameController gameController)
		{
			this.gameController = gameController;
		}
		
		public IEnumerable<TechnologyGeneralInfo> DevelpmentTopics
		{
			get
			{
				foreach(var tech in this.gameController.GameInstance.Statics.Technologies.
				        Where(x => x.Category == TechnologyCategory.Development).
				        OrderBy(x => x.IdCode))
					yield return new TechnologyGeneralInfo(tech);
			}
		}
		
		public IEnumerable<TechnologyGeneralInfo> ResearchTopics
		{
			get
			{
				foreach(var tech in this.gameController.GameInstance.Statics.Technologies.
				        Where(x => x.Category == TechnologyCategory.Research).
				        OrderBy(x => x.IdCode))
					yield return new TechnologyGeneralInfo(tech);
			}
		}
	}
}
