using Ikadn.Ikon.Types;
using Stareater.Utils.StateEngine;

namespace Stareater.Localization.StarNames
{
	[StateType(saveMethod: "Save")]
	public interface IStarName
	{
		string ToText(Language language);
		IkonBaseObject Save(SaveSession session);

		//TODO(v0.7) remove
		IkonBaseObject Save();
	}
}
