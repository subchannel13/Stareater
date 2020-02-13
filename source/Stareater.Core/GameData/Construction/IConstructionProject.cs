using Stareater.AppData.Expressions;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameData.Construction
{
	[StateBaseTypeAttribute("Load", typeof(ProjectFactory))]
    interface IConstructionProject
	{
		string StockpileGroup { get; }
		Formula Condition { get; }
		Formula Cost { get; }
		Formula TurnLimit { get; }
		bool IsVirtual { get;}

		IEnumerable<IConstructionEffect> Effects { get; }

		void Accept(IConstructionProjectVisitor visitor);

		bool Equals(IConstructionProject project);
	}
}
