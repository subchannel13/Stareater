using Stareater.Galaxy;
using Stareater.Utils.StateEngine;

namespace Stareater.Localization.StarNames
{
	[StateBaseType("loadName", typeof(StarData))]
	public interface IStarName
	{
		string ToText(Language language);
	}
}