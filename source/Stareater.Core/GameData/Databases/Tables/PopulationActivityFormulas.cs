using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Utils;

namespace Stareater.GameData.Databases.Tables
{
	public class PopulationActivityFormulas
	{
		public Formula Improvised { get; private set; }
		public Formula Organized { get; private set; }
		public Formula OrganizationFactor { get; private set; }

		public PopulationActivityFormulas(Formula improvised, Formula organized, Formula organizationFactor)
		{
			this.Improvised = improvised;
			this.Organized = organized;
			this.OrganizationFactor = organizationFactor;
		}
		
		public double Evaluate(double organizationRatio, IDictionary<string, double> variables)
		{
			return Methods.Lerp(
				organizationRatio * Methods.Clamp(this.OrganizationFactor.Evaluate(variables), 0, 1),
				Improvised.Evaluate(variables),
				Organized.Evaluate(variables)
			);
		}
	}
}
