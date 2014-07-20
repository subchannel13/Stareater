using System;
using Ikadn;

namespace Stareater.Localization.StarNames
{
	public interface IStarName
	{
		string ToText(Language language);
		IkadnBaseObject Save();
	}
}
