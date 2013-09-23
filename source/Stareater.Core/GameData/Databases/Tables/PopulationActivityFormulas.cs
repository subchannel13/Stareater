using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Utils;

namespace Stareater.GameData.Databases.Tables
{
	public class PopulationActivityFormulas
	{
		public Formula Improvised { get; private set; }
		public Formula Organized { get; private set; }
		
		public PopulationActivityFormulas(Formula improvised, Formula organized)
		{
			this.Improvised = improvised;
			this.Organized = organized;
		}
		
		public double Evaluate(double organizationRatio, IDictionary<string, double> variables)
		{
			return Methods.Lerp(
				organizationRatio, 
				Improvised.Evaluate(variables),
				Organized.Evaluate(variables)
			);
		}
	}
}
