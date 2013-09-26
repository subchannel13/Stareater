using System;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.GameData
{
	internal class TechnologyProgress
	{
		public const int NotStarted = -1;
		public const int Unordered = -1;
			
		public int Level { get; private set; }
		public double InvestedPoints { get; private set; }
		public Technology Topic { get; private set; }
		internal Player Owner { get; private set; }
		
		public int Order { get; set; }
		
		public TechnologyProgress(int level, double investedPoints, Technology topic, Player owner)
		{
			this.Level = level;
			this.InvestedPoints = investedPoints;
			this.Topic = topic;
			this.Owner = owner;
			this.Order = Unordered;
		}
		
		public TechnologyProgress(Technology topic, Player owner) : 
			this (NotStarted, 0, topic, owner)
		{ }
		
		public int NextLevel
		{
			get {
				if (Level < 0)
					return 0;
				else if (Level >= Topic.MaxLevel)
					return Topic.MaxLevel;
				
				return Level + 1;
			}
		}
		
		public bool CanProgress(Func<string, int> techLevelGetter)
		{
			if (Level >= Topic.MaxLevel)
				return false;
			
			return Prerequisite.AreSatisfied(Topic.Prerequisites, NextLevel, techLevelGetter);
			
			//TODO: delete if OK
			/*var levelVars = new Var("lvl0", NextLevel).Get;
			foreach(Prerequisite prerequisite in Topic.Prerequisites) {
				double requiredLevel = prerequisite.Level.Evaluate(levelVars);
				if (requiredLevel >= 0 && techLevelGetter(prerequisite.Code) < requiredLevel)
					return false;
			}
			
			return true;*/
		}
	}
}
