using System;

namespace Stareater.Ships.Missions
{
	interface IMissionVisitor
	{
		void Visit(ColonizationMission mission);
		void Visit(MoveMission mission);
		void Visit(SkipTurnMission mission);
	}
}
