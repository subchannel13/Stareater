using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Controllers.Views.Ships
{
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
