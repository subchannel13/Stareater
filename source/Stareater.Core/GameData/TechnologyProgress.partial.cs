using System;
using System.Collections.Generic;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.GameData
{
	partial class TechnologyProgress
	{
		public const int NotStarted = -1;
		public const int Unordered = -1;
			
		public TechnologyProgress(Technology topic, Player owner) : 
			this (owner, topic, NotStarted, 0)
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
			return Level < Topic.MaxLevel && 
				Prerequisite.AreSatisfied(Topic.Prerequisites, NextLevel, techLevels);
			
		}
		
		public void Progress(ScienceResult progressData)
		{
			this.Level += (int)progressData.CompletedCount;
			this.InvestedPoints = progressData.LeftoverPoints;
		}
		
		public ScienceResult SimulateInvestment(double points, IDictionary<string, int> techLevels)
		{
			int tmplevel = Level;
			int newLevels = 0;
			double tmpInvested = InvestedPoints;
			double totalInvested = 0;
			
			while(tmplevel < Topic.MaxLevel && Prerequisite.AreSatisfied(Topic.Prerequisites, tmplevel + 1, techLevels))
			{
				double pointsLeft = this.Topic.Cost.Evaluate(new Var(Technology.LevelKey, tmplevel + 1).Get) - tmpInvested;
				
				if (pointsLeft > points)
					return new ScienceResult(newLevels, totalInvested + points, this, tmpInvested + points);
				
				tmplevel++;
				newLevels++;
				
				tmpInvested = 0;
				totalInvested += pointsLeft;
				points -= pointsLeft;
			}
			
			return new ScienceResult(newLevels, totalInvested, this, tmpInvested);
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as TechnologyProgress;
			if (other == null)
				return false;
			return object.Equals(this.Topic, other.Topic) && object.Equals(this.Owner, other.Owner);
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (Topic != null)
					hashCode += 1000000007 * Topic.GetHashCode();
				if (Owner != null)
					hashCode += 1000000009 * Owner.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(TechnologyProgress lhs, TechnologyProgress rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(TechnologyProgress lhs, TechnologyProgress rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	}
}
