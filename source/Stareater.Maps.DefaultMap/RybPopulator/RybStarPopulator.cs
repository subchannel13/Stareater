using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Galaxy;
using Stareater.Galaxy.BodyTraits;
using Stareater.Galaxy.Builders;
using Stareater.Localization;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.PluginParameters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Stareater.Maps.DefaultMap.RybPopulator
{
	public class RybStarPopulator : IStarPopulator
	{
		private const string ParametersFile = "rybPopulator.txt";
		private const string LanguageContext = "DefaultPopulator";

		private const int SysGenRepeats = 10;
		private const int PointSpreadRetries = 5;

		private const string ClimateLevelKey = "Climate";
		private const string PotentialLevelKey = "Potential";
		private const string StarTypeKey = "StarType";
		private const string StarColorKey = "color";
		private const string StarMinRadiationKey = "minRadiation";
		private const string StarMaxRadiationKey = "maxRadiation";

		private SelectorParameter climateParameter;
		private SelectorParameter potentialParameter;

		private ClimateLevel[] climateLevels;
		private PotentialLevel[] potentialLevels;

		private StarType[] starTypes;
		private TraitType[] planetTraits;

		public void Initialize(string dataPath)
		{
			TaggableQueue<object, IkadnBaseObject> queue;
			using (var parser = new IkonParser(new StreamReader(dataPath + ParametersFile)))
				queue = parser.ParseAll();

			var generalData = queue.Dequeue("General").To<IkonComposite>();
			this.MinScore = generalData["minScore"].To<double>();
			this.MaxScore = generalData["maxScore"].To<double>();
			var ranges = generalData["ranges"].To<double[][]>();

			var climates = new List<ClimateLevel>();
			while (queue.CountOf(ClimateLevelKey) > 0)
			{
				var data = queue.Dequeue(ClimateLevelKey).To<IkonComposite>();
				climates.Add(new ClimateLevel(
					data["langCode"].To<string>(),
					data["rangeWeights"].To<double[]>().Select((x,i) => new WeightedRange(ranges[i][0], ranges[i][1], x)).ToArray()
				));
			}
			climateLevels = climates.ToArray();

			var potentials = new List<PotentialLevel>();
			while (queue.CountOf(PotentialLevelKey) > 0)
			{
				var data = queue.Dequeue(PotentialLevelKey).To<IkonComposite>();
				potentials.Add(new PotentialLevel(
					data["langCode"].To<string>(),
					data["rangeWeights"].To<double[]>().Select((x, i) => new WeightedRange(ranges[i][0], ranges[i][1], x)).ToArray()
				));
			}
			potentialLevels = potentials.ToArray();

			var starTypes = new List<StarType>();
			while (queue.CountOf(StarTypeKey) > 0)
			{
				var data = queue.Dequeue(StarTypeKey).To<IkonComposite>();
				starTypes.Add(new StarType(
					extractColor(data[StarColorKey].To<IkonArray>()),
					data[StarMinRadiationKey].To<double>(),
					data[StarMaxRadiationKey].To<double>()
				));
			}
			this.starTypes = starTypes.ToArray();

			//TODO(v0.8) why Dictionary<int, string>?
			this.climateParameter = new SelectorParameter(
				LanguageContext, "climate", 
				climates.Select((x, i) => i).ToDictionary(i => i, i => climates[i].LanguageCode),
				generalData["defaultClimate"].To<int>()
			);
			this.potentialParameter = new SelectorParameter(
				LanguageContext, "potential",
				potentials.Select((x, i) => i).ToDictionary(i => i, i => potentials[i].LanguageCode),
				generalData["defaultPotential"].To<int>()
			);
		}

		public void SetGameData(IEnumerable<TraitType> planetTraits)
		{
			this.planetTraits = planetTraits.ToArray();
		}

		private Color extractColor(IList<IkadnBaseObject> arrayValue)
		{
			return Color.FromArgb(
				(int)(arrayValue[0].To<double>() * 255),
				(int)(arrayValue[1].To<double>() * 255),
				(int)(arrayValue[2].To<double>() * 255)
			);
		}

		public string Code
		{
			get { return "RybPopulator"; }
		}

		public string Name
		{
			get { return LocalizationManifest.Get.CurrentLanguage[LanguageContext]["name"].Text(); }
		}

		public string Description
		{
			get
			{
				var vars = new Var().
					Init(climateLevels.Select(x => x.LanguageCode), -1).
					Set(climateLevels[climateParameter.Value].LanguageCode, 1).
					Init(potentialLevels.Select(x => x.LanguageCode), -1).
					Set(potentialLevels[potentialParameter.Value].LanguageCode, 1);

				return LocalizationManifest.Get.CurrentLanguage[LanguageContext]["description"].Text(vars.Get);
			}
		}

		public IEnumerable<AParameterBase> Parameters
		{
			get
			{
				yield return climateParameter;
				yield return potentialParameter;
			}
		}

		public double MinScore { get; private set; }
		public double MaxScore { get; private set; }

		public IEnumerable<StarSystemBuilder> Generate(Random rng, SystemEvaluator evaluator, StarPositions starPositions)
		{
			var namer = new StarNamer(starPositions.Stars.Length, new Random());

			//TODO(v0.8) set scores for homeworlds
			var potentials = new Dictionary<Vector2D, double>();
			var undistributed = new HashSet<Vector2D>(starPositions.Stars);
			var ranges = this.potentialLevels[this.potentialParameter.Value].Ranges;
			var weightSum = ranges.Sum(x => x.Weight);
			foreach (var range in ranges)
			{
				var count = (int)Math.Round(range.Weight * undistributed.Count() / weightSum);
				foreach(var star in spreadPoints(rng, undistributed, count))
				{
					potentials[star] = Methods.Lerp(rng.NextDouble(), range.Min, range.Max);
					undistributed.Remove(star);
				}
				weightSum -= range.Weight;
			}

			var starts = new Dictionary<Vector2D, double>();
			undistributed = new HashSet<Vector2D>(starPositions.Stars);
			ranges = this.climateLevels[this.climateParameter.Value].Ranges;
			weightSum = ranges.Sum(x => x.Weight);
			foreach (var range in ranges)
			{
				var undistributedCandidates = new HashSet<Vector2D>(undistributed.Where(x => potentials[x] >= range.Min));
				var count = Math.Min(
					(int)Math.Round(range.Weight * undistributed.Count / weightSum), 
					undistributedCandidates.Count
				);

				foreach (var star in spreadPoints(rng, undistributedCandidates, count))
				{
					starts[star] = Methods.Lerp(rng.NextDouble(), range.Min, range.Max);
					undistributed.Remove(star);
				}
				weightSum -= range.Weight;
			}

			//TODO(v0.8): Star size and trait distribution
			foreach (var position in starPositions.Stars)
			yield return generateSystem(namer, position, rng, evaluator, starts[position], potentials[position]);
		}

		private IEnumerable<Vector2D> spreadPoints(Random rng, HashSet<Vector2D> points, int count)
		{
			if (count == 1)
				return new Vector2D[] { new PickList<Vector2D>(rng, points).Pick() };

			var centroids = new HashSet<Vector2D>();
			var picker = new PickList<Vector2D>(rng, points);
			while (centroids.Count < count)
				centroids.Add(picker.Take());

			bool advanced;
			do
			{
				advanced = false;
				var grouping = centroids.ToDictionary(x => x, x => new List<Vector2D>());

				foreach (var point in points)
					grouping[
						Methods.FindBest(centroids, x => -(point - x).LengthSquared)
					].Add(point);

				centroids.Clear();
				foreach (var group in grouping)
				{
					var center = group.Value.Aggregate((a, b) => a + b) / group.Value.Count;
					var closest = Methods.FindBest(group.Value, x => -(center - x).LengthSquared);

					centroids.Add(closest);
					advanced |= closest != group.Key;
				}
			} while (advanced);

			return centroids;
		}

		private StarSystemBuilder generateSystem(StarNamer namer, Vector2D position, Random rng, SystemEvaluator evaluator, double startingScore, double potentialScore)
		{
			var starColor = starTypes[rng.Next(starTypes.Length)].Hue;
			var starName = namer.NextName();

			var systems = new List<StarSystemBuilder>();
			for (int i = 0; i < SysGenRepeats; i++)
			{
				var system = new StarSystemBuilder(starColor, 1, starName, position, new List<TraitType>());
				systems.Add(system);
				var planets = rng.Next(6);
				for(int p = 0; p < planets; p++)
					system.AddPlanet(p, bodyTypes()[rng.Next(3)], Methods.Lerp(rng.NextDouble(), 50, 200), randomTraits(rng));
			}

			return Methods.FindBest(systems, x => -Methods.MeanSquareError(
				evaluator.StartingScore(x) - startingScore,
				evaluator.PotentialScore(x) - potentialScore
			));
		}

		private List<TraitType> randomTraits(Random rng)
		{
			var targetCount = rng.Next(planetTraits.Length + 1);
			var options = new PickList<TraitType>(rng, planetTraits);

			while (options.Count() > targetCount)
				options.Take();

			return options.InnerList;
		}

		private static PlanetType[] bodyTypes()
		{
			return new[] { PlanetType.Asteriod, PlanetType.GasGiant, PlanetType.Rock };
		}
	}
}
