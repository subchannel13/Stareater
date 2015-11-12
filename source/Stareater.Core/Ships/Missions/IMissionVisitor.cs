using System;

namespace Stareater.Ships.Missions
{
	interface IMissionVisitor
	{
		void Visit(MoveMission mission);
	}
}
