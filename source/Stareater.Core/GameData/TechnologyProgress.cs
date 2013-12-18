using System;
using System.Collections.Generic;
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
		
		public bool CanProgress(IDictionary<string, int> techLevels)
		{
			if (Level >= Topic.MaxLevel)
				return false;
			
			return Prerequisite.AreSatisfied(Topic.Prerequisites, NextLevel, techLevels);
		}

		public double Invest(double points, IDictionary<string, int> techLevels)
		{
			while(CanProgress(techLevels))
			{
				double pointsLeft = this.Topic.Cost.Evaluate(new Var(Technology.LevelKey, this.NextLevel).Get) - this.InvestedPoints;
				
				if (pointsLeft > points) {
					InvestedPoints += points;
					return 0;
				}
				
				this.Level = NextLevel;
				this.InvestedPoints = 0;
				points -= pointsLeft;
				//TODO: add new tech level message
			}
			
			return points;
		}
		
		internal TechnologyProgress Copy(Player player)
		{
			TechnologyProgress copy = new TechnologyProgress(Level, InvestedPoints, Topic, player);
			copy.Order = this.Order;

			return copy;
		}
	}
}
