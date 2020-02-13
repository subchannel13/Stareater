using Stareater.AppData.Expressions;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy.BodyTraits
{
	[StateTypeAttribute(true)]
	public class PlanetTraitType : IIdentifiable
	{
		public string LanguageCode { get; private set; }

		public string ImagePath { get; private set; }
		public string IdCode { get; private set; }
		public Formula MaintenanceCost { get; private set; }

		public PlanetTraitType(string languageCode, string imagePath, string idCode, Formula maintenanceCost)
		{
			this.LanguageCode = languageCode;
			this.ImagePath = imagePath;
			this.IdCode = idCode;
			this.MaintenanceCost = maintenanceCost;
		}

		public override bool Equals(object obj)
		{
			var other = obj as PlanetTraitType;
			return other != null && object.Equals(this.IdCode, other.IdCode);
		}

		public override int GetHashCode()
		{
			return this.IdCode.GetHashCode();
		}

		public override string ToString()
		{
			return this.IdCode;
		}
	}
}
