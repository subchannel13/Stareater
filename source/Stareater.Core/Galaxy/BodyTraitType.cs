using Ikadn.Ikon.Types;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	[StateType(true)]
	public class BodyTraitType
	{
		public string LanguageCode { get; private set; }
		
		public string ImagePath { get; private set; }
		public string IdCode { get; private set; }

		internal ITraitEffectType Effect { get; private set; }

		internal BodyTraitType(string languageCode, string imagePath, string idCode, ITraitEffectType effect)
		{
			this.LanguageCode = languageCode;
			this.ImagePath = imagePath;
			this.IdCode = idCode;
			this.Effect = effect;
		}

		internal BodyTrait Instantiate(Planet location)
		{
			return new BodyTrait(this, location);
		}
		
		internal BodyTrait Instantiate(StarData location)
		{
			return new BodyTrait(this, location);
		}

		public BodyTrait Load(Planet location, Ikadn.IkadnBaseObject loadData)
		{
			return new BodyTrait(this, location, loadData.To<IkonComposite>());
		}
		
		public BodyTrait Load(StarData location, Ikadn.IkadnBaseObject loadData)
		{
			return new BodyTrait(this, location, loadData.To<IkonComposite>());
		}

		private static ITraitEffect Load(Ikadn.IkadnBaseObject loadData, LoadSession session)
		{
			return null; //TODO(v0.7) stub, refactor traits so they don't depend on a body
		}
	}
}
