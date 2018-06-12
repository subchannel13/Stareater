using Stareater.GameData;
using Stareater.GameData.Construction;

namespace Stareater.GameLogic.Planning
{
	class ConstructionComparer : IConstructionProjectVisitor
	{
		bool found = false;
		ConstructableType targetType;

		public bool Compare(IConstructionProject project, ConstructableType targetType)
		{
			this.found = false;
			this.targetType = targetType;
			project.Accept(this);

			return this.found;
		}

		void IConstructionProjectVisitor.Visit(ColonizerProject project)
		{
			//no operation
		}

		void IConstructionProjectVisitor.Visit(ShipProject project)
		{
			//no operation
		}

		 void IConstructionProjectVisitor.Visit(StaticProject project)
		{
			this.found |= project.Type == this.targetType;
		}
	}
}
