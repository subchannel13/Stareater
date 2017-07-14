using System;
using System.Linq;
using Ikadn;
using Ikadn.Ikon.Types;

namespace Stareater.Galaxy
{
	//TODO(later) make planet and star traits internal
	public class BodyTrait
	{
		internal BodyTraitType Type { get; private set; }
		//TODO(v0.7) can have mutable state, needs to be copied
		internal ITraitEffect Effect { get; private set; }

		private BodyTrait(BodyTraitType type, LocationBody location)
		{
			this.Type = type;
			this.Effect = type.Effect.Instantiate(location, this);
		}
		
		private BodyTrait(BodyTraitType type, LocationBody location, IkonComposite loadData)
		{
			this.Type = type;
			this.Effect = type.Effect.Load(location, this, loadData);
		}
		
		private BodyTrait(BodyTraitType type, ITraitEffect effect)
		{
			this.Type = type;
			this.Effect = effect;
		}

		internal BodyTrait(BodyTraitType type, Planet location) :
			this(type, new LocationBody(location.Star, location))
		{ }

		internal BodyTrait(BodyTraitType type, StarData location) : 
			this(type, new LocationBody(location))
		{ }

		internal BodyTrait(BodyTraitType type, Planet location, IkonComposite loadData) :
			this(type, new LocationBody(location.Star, location), loadData)
		{ }

		internal BodyTrait(BodyTraitType type, StarData location, IkonComposite loadData) :
			this(type, new LocationBody(location), loadData)
		{ }
		
		internal BodyTrait Copy()
		{
			return new BodyTrait(this.Type, this.Effect.Copy());
		}
		
		internal IkadnBaseObject Save()
		{
			var data = new IkonComposite(this.Type.IdCode);
			this.Effect.Save(data);
			
			return data;
		}
	}
}
