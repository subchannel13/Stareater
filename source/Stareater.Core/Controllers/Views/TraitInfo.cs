using Stareater.Galaxy.BodyTraits;

namespace Stareater.Controllers.Views
{
	public class TraitInfo
	{
		internal TraitInfo(PlanetTraitType data)
		{
			this.ImagePath = data.ImagePath;
			this.LangCode = data.LanguageCode;
		}

		internal TraitInfo(StarTraitType data)
		{
			this.ImagePath = data.ImagePath;
			this.LangCode = data.LanguageCode;
		}

		public string ImagePath { get; private set; }
		public string LangCode { get; private set; }
	}
}
