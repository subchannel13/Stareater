using System;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.Ships;

namespace Stareater.GameLogic
{
	class ConstructionAddColonizer : IConstructionEffect
	{
		private readonly Design colonizerDesign;
		private readonly Planet destination;

		public ConstructionAddColonizer(Design colonizerDesign, Planet destination)
		{
			this.colonizerDesign = colonizerDesign;
			this.destination = destination;
		}
			
		#region IConstructionEffect implementation
		public void Apply(StatesDB states, TemporaryDB derivates, AConstructionSite site, long quantity)
		{
			//TODO(v0.5) check if colonizer can be added (planet already occupied)
			if (!states.ColonizationProjects.OfContains(destination))
				states.ColonizationProjects.Add(new ColonizationProject(site.Owner, destination));

			var project = states.ColonizationProjects.Of(destination);
			Fleet fleet = project.NewColonizers.FirstOrDefault(x => x.Position == site.Location.Star.Position);

			if (fleet == null)
			{
				fleet = new Fleet(site.Owner, site.Location.Star.Position, null);
				project.NewColonizers.Add(fleet);
			}

			if (fleet.Ships.DesignContains(colonizerDesign))
				fleet.Ships.Design(colonizerDesign).Quantity += quantity;
			else
				fleet.Ships.Add(new ShipGroup(colonizerDesign, quantity));
		}
		#endregion
		
	}
}
