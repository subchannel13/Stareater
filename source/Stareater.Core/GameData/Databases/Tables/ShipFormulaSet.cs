using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	public class ShipFormulaSet
	{
		public Formula CombatSpeed { get; private set; }
		public Formula Detection { get; private set; }
		public Formula Evasion { get; private set; }
		public Formula HitPoints { get; private set; }
		
		public ShipFormulaSet(Formula combatSpeed, Formula detection, Formula evasion, Formula hitPoints)
		{
			this.CombatSpeed = combatSpeed;
			this.Detection = detection;
			this.Evasion = evasion;
			this.HitPoints = hitPoints;
		}
	}
}
