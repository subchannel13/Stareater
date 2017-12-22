using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class PlanetForumlaSet
	{
		public Formula SpaceliftFactor { get; private set; }
		public Formula MaintenanceCost { get; private set; }

		public PlanetForumlaSet(Formula spaceliftFactor, Formula maintenanceCost)
		{
			this.SpaceliftFactor = spaceliftFactor;
			this.MaintenanceCost = maintenanceCost;
		}
	}
}
