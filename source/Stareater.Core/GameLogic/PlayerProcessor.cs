using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.GameLogic
{
	class PlayerProcessor
	{
		public const string LevelSufix = "Lvl";
		
		public Player Player { get; private set; }
		
		public PlayerProcessor(Player player, IEnumerable<Technology> technologies)
		{
			this.Player = player;
			
			this.TechLevels = new Dictionary<string, double>();
			foreach (var tech in technologies) {
				this.TechLevels.Add(tech.IdCode + LevelSufix, TechnologyProgress.NotStarted);
			}
		}

		public PlayerProcessor(Player player)
		{
			this.Player = player;
		}

		#region Galaxy phase
		
		public IDictionary<string, double> TechLevels { get; private set; }

		public void Calculate(IEnumerable<TechnologyProgress> techAdvances)
		{
			foreach (var tech in techAdvances) {
				TechLevels[tech.Topic.IdCode + LevelSufix] = tech.Level;
			}
		}
		#endregion
		
		internal PlayerProcessor Copy(PlayersRemap playersRemap)
		{
			PlayerProcessor copy = new PlayerProcessor(playersRemap.Players[this.Player]);
			
			copy.TechLevels = new Dictionary<string, double>(this.TechLevels);

			return copy;
		}
		
		#region Precombat processing
		
		private double developmentPoints = 0;
		
		public void ProcessPrecombat(IList<ColonyProcessor> colonyProcessors)
		{
			foreach(var colonyProc in colonyProcessors)
				developmentPoints += colonyProc.DevelopmentPoints();
		}
		#endregion
		
		public void ProcessPostcombat(IEnumerable<TechnologyProgress> advanceOrder)
		{
			var techLevels = advanceOrder.ToDictionary(x => x.Topic.IdCode, x => x.Level);
			
			foreach(var tech in advanceOrder)
			{
				developmentPoints = tech.Invest(developmentPoints, techLevels);
			}
		}
	}
}
