using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views
{
	public class PlanetInfo
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

		public StarInfo HostStar
		{
			get { return new StarInfo(this.Data.Star); }
		}

		public double PopulationMax { get; private set; }

		public override bool Equals(object obj)
		{
			var other = obj as PlanetInfo;
			return other != null &&
				   EqualityComparer<Planet>.Default.Equals(this.Data, other.Data);
		}

		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		public static bool operator ==(PlanetInfo info1, PlanetInfo info2)
		{
			return EqualityComparer<PlanetInfo>.Default.Equals(info1, info2);
		}

		public static bool operator !=(PlanetInfo info1, PlanetInfo info2)
		{
			return !(info1 == info2);
		}
	}
}
