using System;
using Stareater.GameData.Construction;
using Stareater.Localization;

namespace Stareater.Controllers
{
	class ConstructionInfoExtractor : IConstructionProjectVisitor
	{
		private const string LangContext = "Constructables";

		public string Name { get; private set; }
		public string Description { get; private set; }
		public string ImagePath { get; private set; }

		public ConstructionInfoExtractor(IConstructionProject project)
		{
			project.Accept(this);
		}

		public void Visit(StaticProject project)
		{
			this.Description = LocalizationManifest.Get.CurrentLanguage[LangContext].Description(project.Type.LanguageCode).Text();
			this.ImagePath = project.Type.ImagePath;
			this.Name = LocalizationManifest.Get.CurrentLanguage[LangContext].Name(project.Type.LanguageCode).Text();
		}

		public void Visit(ShipProject project)
		{
			this.Description = "";
			this.ImagePath = project.Type.ImagePath;
			this.Name = project.Type.Name;
		}

		public void Visit(ColonizerProject project)
		{
			this.Description = "";
			this.ImagePath = project.Colonizer.ImagePath;
			this.Name = project.Colonizer.Name;
		}
	}
}
