using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using Stareater.Players;
using System.Collections.Generic;
using Stareater.GameData.Databases;
using Stareater.GameLogic.Planning;

namespace Stareater.GameData
{
	class DevelopmentProgress 
	{
		public const int NotStarted = -1;
		public const int Unordered = -1;

		[StateProperty]
		public Player Owner { get; private set; }

		//TODO(v0.8) saves as ordinal number so doesn't load proper topic after modding (or adding more content)
		[StateProperty]
		public DevelopmentTopic Topic { get; private set; }

		[StateProperty]
		public int Level { get; private set; }

		[StateProperty]
		public double InvestedPoints { get; private set; }

		[StateProperty]
		public int Priority { get; set; }

		public DevelopmentProgress(Player owner, DevelopmentTopic topic, int level, double investedPoints, int priority) 
		{
			this.Owner = owner;
			this.Topic = topic;
			this.Level = level;
			this.InvestedPoints = investedPoints;
			this.Priority = priority;
		}

		public DevelopmentProgress(DevelopmentTopic topic, Player owner) :
			this(owner, topic, NotStarted, 0, 0)
		{ }

		private DevelopmentProgress() 
		{ }

		public int NextLevel
		{
			get
			{
				if (Level < 0)
					return 0;
				else if (Level >= Topic.MaxLevel)
					return Topic.MaxLevel;

				return Level + 1;
			}
		}

		public bool CanProgress(Dictionary<string, double> researchLevels, StaticsDB statics)
		{
			var requirement = statics.DevelopmentRequirements.ContainsKey(this.Topic.IdCode) ?
				statics.DevelopmentRequirements[this.Topic.IdCode] :
				null;

			return Level < Topic.MaxLevel && (requirement == null || researchLevels[requirement.Code] >= requirement.Level);
		}

		public void Progress(DevelopmentResult progressData)
		{
			this.Level += (int)progressData.CompletedCount;
			this.InvestedPoints = progressData.LeftoverPoints;
		}

		//TODO(v0.8) consider moving to player processor
		public DevelopmentResult SimulateInvestment(double points, IDictionary<string, double> techLevels)
		{
			int tmplevel = Level;
			int newLevels = 0;
			double tmpInvested = InvestedPoints;
			double totalInvested = 0;

			while (tmplevel < Topic.MaxLevel)
			{
				var vars = new Var(DevelopmentTopic.LevelKey, tmplevel + 1).
					And(DevelopmentTopic.PriorityKey, this.Priority).Get;
				double pointsLeft = this.Topic.Cost.Evaluate(vars) - tmpInvested;

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

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as DevelopmentProgress;
			if (other == null)
				return false;
			return object.Equals(this.Topic, other.Topic) && object.Equals(this.Owner, other.Owner);
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked
			{
				if (Topic != null)
					hashCode += 1000000007 * Topic.GetHashCode();
				if (Owner != null)
					hashCode += 1000000009 * Owner.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(DevelopmentProgress lhs, DevelopmentProgress rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(DevelopmentProgress lhs, DevelopmentProgress rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
}
