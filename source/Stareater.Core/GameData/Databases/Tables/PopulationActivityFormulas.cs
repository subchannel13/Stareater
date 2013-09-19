using System;
using Stareater.AppData.Expressions;

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
	}
}
