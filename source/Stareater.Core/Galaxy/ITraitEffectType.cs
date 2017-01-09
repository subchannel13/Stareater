using System;
using Ikadn.Ikon.Types;

namespace Stareater.Galaxy
{
	interface ITraitEffectType
	{
		ITraitEffect Instantiate(LocationBody location, BodyTrait parentTrait);
		ITraitEffect Load(LocationBody location, BodyTrait parentTrait, IkonComposite loadData);
	}
}
