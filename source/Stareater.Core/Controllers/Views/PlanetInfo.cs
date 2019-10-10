using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views
{
	public class PlanetInfo : IEquatable<PlanetInfo>
	{
		internal Planet Data { get; private set; }
		
		internal PlanetInfo(Planet data, MainGame game) : this(
			data, 
			game.Statics.ColonyFormulas.UncolonizedMaxPopulation.Evaluate(new Var(ColonyProcessor.PlanetSizeKey, data.Size).Get)
		)
		{ }

		internal PlanetInfo(Planet data, double populationMax)
		{
			this.Data = data;
			this.PopulationMax = populationMax;
		}

		public PlanetType Type
		{
			get { return this.Data.Type; }
		}

		public int Position
		{
			get { return this.Data.Position; }
		}

		public IEnumerable<TraitInfo> Traits
		{
			get
			{
				return this.Data.Traits.Select(x => new TraitInfo(x));
			}
		}

		public StarInfo HostStar
		{
			get { return new StarInfo(this.Data.Star); }
		}

		public double PopulationMax { get; private set; }

		public bool Equals(PlanetInfo other)
		{
			if (other is null)
				return false;

			if (Object.ReferenceEquals(this, other))
				return true;

			if (this.GetType() != other.GetType())
				return false;

			return EqualityComparer<Planet>.Default.Equals(this.Data, other.Data);
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as Planet);
		}

		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		public static bool operator ==(PlanetInfo info1, PlanetInfo info2)
		{
			if (info1 is null)
				return info2 is null;
			return info1.Equals(info2);
		}

		public static bool operator !=(PlanetInfo info1, PlanetInfo info2)
		{
			return !(info1 == info2);
		}
	}
}
