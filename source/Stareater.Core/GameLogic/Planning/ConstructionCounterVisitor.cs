using Stareater.GameData.Construction;
using System.Collections.Generic;

namespace Stareater.GameLogic.Planning
{
	class ConstructionCounterVisitor : IConstructionProjectVisitor
	{
		private IDictionary<string, double> variables;
		private long completedCount;

		public ConstructionCounterVisitor(IDictionary<string, double> variables)
		{
			this.variables = variables;
		}

		public void Count(IConstructionProject project, long completedCount)
		{
			this.completedCount = completedCount;
			project.Accept(this);
		}

		public void Visit(StaticProject project)
		{
			variables[project.Type.IdCode + ColonyProcessor.NewBuidingPrefix] = completedCount;
        }

		public void Visit(ShipProject project)
		{
			//no operation
		}
	}
}
