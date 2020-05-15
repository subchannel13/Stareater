using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Ships.Missions;
using Stareater.Utils;
using System.Linq;

namespace Stareater.Galaxy
{
	class Fleet 
	{
		[StatePropertyAttribute]
		public Player Owner { get; private set; }

		[StatePropertyAttribute]
		public Vector2D Position { get; private set; }

		[StatePropertyAttribute]
		public LinkedList<AMission> Missions { get; private set; }

		[StatePropertyAttribute]
		public ShipGroupCollection Ships { get; private set; }

		[StatePropertyAttribute]
		public List<Fleet> PreviousTurn { get; private set; }

		public Fleet(Player owner, Vector2D position, LinkedList<AMission> missions, List<Fleet> previousStates) 
		{
			this.Owner = owner;
			this.Position = position;
			this.Missions = missions;
			this.Ships = new ShipGroupCollection();
			this.PreviousTurn = previousStates;
		}

		public Fleet(Player owner, Vector2D position, LinkedList<AMission> missions) :
			this(owner, position, missions, new List<Fleet>())
		{ }

		public Fleet(Player owner, Vector2D position, LinkedList<AMission> missions, Fleet previousState) :
			this(owner, position, missions, new List<Fleet> { previousState })
		{ }

		public Fleet(Player owner, Vector2D position, LinkedList<AMission> missions, IEnumerable<Fleet> previousStates) :
			this(owner, position, missions, previousStates.Where(x => x != null).Distinct().ToList())
		{ }

		private Fleet() 
		{ }
	}
}
