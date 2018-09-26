namespace Stareater.GameData.Construction
{
	interface IConstructionProjectVisitor
	{
		void Visit(ShipProject project);
		void Visit(StaticProject project);
	}
}
