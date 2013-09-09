using System;
using Stareater.Players;

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
	}
}
