using System;
using Stareater.SpaceCombat;

namespace Stareater.Controllers.Views.Combat
{
	public class CombatantInfo
	{
		private Combatant Data;
		
		internal CombatantInfo(Combatant data)
		{
			this.Data = data;
		}
		
		public int X 
		{ 
			get { return this.Data.X; }
		}
		
		public int Y 
		{ 
			get { return this.Data.Y; }
		}
		
		public PlayerInfo Owner 
		{ 
			get { return new PlayerInfo(this.Data.Owner); }
		}
	}
}
