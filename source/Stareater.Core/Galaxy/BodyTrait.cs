using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Galaxy
{
	//TODO(later) make planet and star traits internal
	public class BodyTrait
	{
		internal BodyTraitType Type { get; private set; }
		internal ITraitEffect Effect { get; private set; }

		private BodyTrait(BodyTraitType type, LocationBody location)
		{
			this.Type = type;
			this.Effect = type.Effect.Instantiate(location, this);
		}

		internal BodyTrait(BodyTraitType type, StarData location) : 
			this(type, new LocationBody(location))
		{ }

		internal BodyTrait(BodyTraitType type, Planet location) :
			this(type, new LocationBody(location.Star, location))
		{ }
	}
}
