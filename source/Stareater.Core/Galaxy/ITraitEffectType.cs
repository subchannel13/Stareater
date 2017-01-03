using System;

namespace Stareater.Galaxy
{
	interface ITraitEffectType
	{
		ITraitEffect Instantiate(LocationBody location, BodyTrait parentTrait);
	}
}
