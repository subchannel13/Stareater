using System;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Controllers.Views.Ships
{
	//TODO(v0.5) This class has to be repurposed to represent all fleet missions or something close to that
	public class MoveMissionInfo : AMissionInfo
	{
		public Vector2D Destionation { get; private set; }
		
		internal MoveMissionInfo(Vector2D destionation)
		{
			this.Destionation = destionation;
		}
		
		public override FleetMissionType Type {
			get {
				return FleetMissionType.Move;
			}
		}
	}
}
