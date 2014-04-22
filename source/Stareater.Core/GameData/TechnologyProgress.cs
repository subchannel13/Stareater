﻿using System;
using System.Collections.Generic;
using Stareater.GameLogic;
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
		
		public void Progress(DevelopmentResult progressData)
		{
			this.Level += progressData.NewLevels;
			this.InvestedPoints += progressData.LeftoverPoints;
		}
		
		public DevelopmentResult SimulateInvestment(double points, IDictionary<string, int> techLevels)
		{
			int tmplevel = Level;
			int newLevels = 0;
			double tmpInvested = InvestedPoints;
			double totalInvested = 0;
			
			while(tmplevel < Topic.MaxLevel && Prerequisite.AreSatisfied(Topic.Prerequisites, tmplevel + 1, techLevels))
			{
				double pointsLeft = this.Topic.Cost.Evaluate(new Var(Technology.LevelKey, tmplevel + 1).Get) - tmpInvested;
				
				if (pointsLeft > points)
					return new DevelopmentResult(newLevels, totalInvested + points, this, tmpInvested + points);
				
				tmplevel++;
				newLevels++;
				
				tmpInvested = 0;
				totalInvested += pointsLeft;
				points -= pointsLeft;
			}
			
			return new DevelopmentResult(newLevels, totalInvested, this, tmpInvested);
		}
		
		internal TechnologyProgress Copy(Player player)
		{
			return new TechnologyProgress(Level, InvestedPoints, Topic, player);
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			TechnologyProgress other = obj as TechnologyProgress;
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
