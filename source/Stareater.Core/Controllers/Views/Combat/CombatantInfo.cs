﻿using System;
using Stareater.Controllers.Views.Ships;
using Stareater.GameLogic;
using Stareater.SpaceCombat;

namespace Stareater.Controllers.Views.Combat
{
	public class CombatantInfo
	{
		internal readonly Combatant Data;
		private readonly DesignStats stats;
		
		internal CombatantInfo(Combatant data, MainGame game)
		{
			this.Data = data;
			this.stats = game.Derivates.Of(data.Owner).DesignStats[data.Ships.Design];
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
		
		public DesignInfo Design
		{ 
			get { return new DesignInfo(this.Data.Ships.Design, stats); }
		}
	}
}