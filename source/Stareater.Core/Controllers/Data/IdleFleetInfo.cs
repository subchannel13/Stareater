using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Data
{
	public class IdleFleetInfo
	{
		public PlayerInfo Owner { get; private set; }
		public StarData Location { get; private set; }
		public Vector2D VisualPosition { get; private set; }
		
		internal Fleet Fleet { get; private set; }
		
		internal IdleFleetInfo(Fleet fleet, Game game, Methods.VisualPositionFunc visualPositionFunc)
		{
			this.Fleet = fleet;
			
			this.Location = game.States.Stars.At(fleet.Position);
			this.Owner = new PlayerInfo(fleet.Owner);
			
			this.VisualPosition = visualPositionFunc(fleet.Position);
		}
	}
}
