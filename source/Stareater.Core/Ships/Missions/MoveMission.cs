using Stareater.Galaxy;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	[StateTypeAttribute(saveTag: MissionTag)]
	class MoveMission : AMission
	{
		[StatePropertyAttribute]
		public StarData Destination { get; private set; }

		[StatePropertyAttribute(saveKey: "via")]
		public Wormhole UsedWormhole { get; private set; }
		
		public MoveMission(StarData destination, Wormhole usedWormhole)
		{
			this.Destination = destination;
			this.UsedWormhole = usedWormhole;
		}

		private MoveMission()
		{ }

		public override bool FullTurnAction
		{
			get { return false; }
		}

		public override void Accept(IMissionVisitor visitor)
		{
			visitor.Visit(this);
		}
		
		public override bool Equals(object obj)
		{
			var other = obj as MoveMission;
			return other != null && this.Destination == other.Destination && this.UsedWormhole == other.UsedWormhole;
		}
		
		public override int GetHashCode()
		{
			return this.Destination.GetHashCode();
		}
		
		public const string MissionTag = "Move";
	}
}
