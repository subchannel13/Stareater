using Stareater.Galaxy.BodyTraits;

namespace Stareater.Controllers.Views
{
	public class TraitInfo
	{
		internal TraitInfo(PlanetTraitType data)
		{
			this.ImagePath = data.ImagePath;
		}

		internal TraitInfo(StarTraitType data)
		{
			this.ImagePath = data.ImagePath;
		}

		public string ImagePath { get; private set; }
	}
}
