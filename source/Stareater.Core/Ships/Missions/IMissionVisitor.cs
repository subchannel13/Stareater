namespace Stareater.Ships.Missions
{
	interface IMissionVisitor
	{
		void Visit(DisembarkMission mission);
		void Visit(LoadMission mission);
		void Visit(MoveMission mission);
		void Visit(SkipTurnMission mission);
	}
}
