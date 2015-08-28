using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	public class ShipFormulaSet
	{
		public Formula HitPoints { get; private set; }
		
		public ShipFormulaSet(Formula hitPoints)
		{
			this.HitPoints = hitPoints;
		}
	}
}
