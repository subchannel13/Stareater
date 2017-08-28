using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Galaxy.BodyTraits
{
	[StateType(true)]
	public class TraitType
	{
		public string LanguageCode { get; private set; }

		public string ImagePath { get; private set; }
		public string IdCode { get; private set; }

		internal ITraitEffectType Effect { get; private set; }

		internal TraitType(string languageCode, string imagePath, string idCode, ITraitEffectType effect)
		{
			this.LanguageCode = languageCode;
			this.ImagePath = imagePath;
			this.IdCode = idCode;
			this.Effect = effect;
		}

		internal ITrait Make()
		{
			return this.Effect.Instantiate(this);
		}

		private static ITrait LoadTrait(Ikadn.IkadnBaseObject loadData, LoadSession session)
		{
			var tag = loadData.Tag as string;

			if (loadData.Tag.Equals(EffectAfflictPlanets.SaveTag))
				return session.Load<EffectAfflictPlanets>(loadData);
			else if (loadData.Tag.Equals(EffectPassive.SaveTag))
				return session.Load<EffectPassive>(loadData);
			else
				throw new KeyNotFoundException("Unknown order type: " + loadData.Tag);
		}
	}
}
