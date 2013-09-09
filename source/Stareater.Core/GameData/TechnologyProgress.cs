using System;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.GameData
{
	public class TechnologyProgress
	{
		public const int NotStarted = -1;
			
		public int Level { get; private set; }
		public double InvestedPoints { get; private set; }
		public Technology Topic { get; private set; }
		public Player Owner { get; private set; }
		
		public TechnologyProgress(int level, double investedPoints, Technology topic, Player owner)
		{
			this.Level = level;
			this.InvestedPoints = investedPoints;
			this.Topic = topic;
			this.Owner = owner;
		}
		
		public TechnologyProgress(Technology topic, Player owner) : 
			this (NotStarted, 0, topic, owner)
		{ }
		
		public int NextLevel
		{
			get {
				if (Level == NotStarted)
					return 0;
				else if (Level < 0 || Level >= Topic.MaxLevel)
					throw new InvalidOperationException("Illegal technology level for " + Topic.IdCode + ", " + Owner.Name);
				
				return Level + 1;
			}
		}
		
		public bool CanProgress(Func<string, int> techLevelGetter)
		{
			if (Level >= Topic.MaxLevel)
				return false;
			
			var levelVars = new Var("lvl0", NextLevel).Get;
			foreach(Prerequisite prerequisite in Topic.Prerequisites)
				if (techLevelGetter(prerequisite.Code) < prerequisite.Level.Evaluate(levelVars))
					return false;
			
			return true;
		}
	}
}
