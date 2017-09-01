namespace Stareater.GameData.Construction
{
	interface IConstructionProjectVisitor
	{
		void Visit(ColonizerProject project);
		void Visit(ShipProject project);
		void Visit(StaticProject project);
	}
}
