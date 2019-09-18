using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.AppData.Expressions;
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

		private const int SysGenRepeats = 20;

		private const string ClimateLevelKey = "Climate";
		private const string PotentialLevelKey = "Potential";
		private const string StarTypeKey = "StarType";
		
		private SelectorParameter climateParameter;
		private SelectorParameter potentialParameter;

		private ClimateLevel[] climateLevels;
		private PotentialLevel[] potentialLevels;

		private StarType[] starTypes;
		private Dictionary<string, PlanetTraitType> planetTraits;
		private Dictionary<string, StarTraitType> starTraits;
		private string[][] planetTraitIdGroups;
		private Dictionary<string, Formula> traitConditions;
		private Dictionary<PlanetType, PlanetTraitType[][]> planetTraitGroups;

		private StarType homeStarType;
		private double homeworldSize;
		private int homeworldPosition;
		private string[] homeworldTraits;

		public void Initialize(string dataPath)
		{
			TaggableQueue<object, IkadnBaseObject> queue;
			IkadnBaseObject homeStarData;
			using (var parser = new IkonParser(new StreamReader(dataPath + ParametersFile)))
			{
				queue = parser.ParseAll();
				homeStarData = parser.GetNamedObject("HomeStar");
			}

			var generalData = queue.Dequeue("General").To<IkonComposite>();
			var ranges = generalData["ranges"].To<double[][]>();
			this.MinScore = generalData["minScore"].To<double>();
			this.MaxScore = generalData["maxScore"].To<double>();
			this.planetTraitIdGroups = generalData["traitGroups"].To<string[][]>();
			this.homeworldSize = generalData["homeworldSize"].To<double>();
			this.homeworldPosition = generalData["homeworldPosition"].To<int>();
			this.homeworldTraits = generalData["homeworldTraits"].To<string[]>();

			this.traitConditions = new Dictionary<string, Formula>();
			var conditions = generalData["traitConditions"].To<IkonComposite>();
			foreach(var traitId in conditions.Keys)
			{
				var parser = new ExpressionParser(conditions[traitId].To<string>());
				parser.Parse();
				if (parser.errors.count > 0)
					throw new FormatException(parser.errors.errorMessages.ToString());
				this.traitConditions[traitId] = parser.ParsedFormula;
			}

			var climates = new List<ClimateLevel>();
			while (queue.CountOf(ClimateLevelKey) > 0)
			{
				var data = queue.Dequeue(ClimateLevelKey).To<IkonComposite>();
				climates.Add(new ClimateLevel(
					data["langCode"].To<string>(),
					data["rangeWeights"].To<double[]>().Select((x,i) => new WeightedRange(ranges[i][0], ranges[i][1], x)).ToArray(),
					data["homeSystemStart"].To<double>()
				));
			}
			climateLevels = climates.ToArray();

			var potentials = new List<PotentialLevel>();
			while (queue.CountOf(PotentialLevelKey) > 0)
			{
				var data = queue.Dequeue(PotentialLevelKey).To<IkonComposite>();
				potentials.Add(new PotentialLevel(
					data["langCode"].To<string>(),
					data["rangeWeights"].To<double[]>().Select((x, i) => new WeightedRange(ranges[i][0], ranges[i][1], x)).ToArray(),
					data["homeSystemPotential"].To<double>()
				));
			}
			potentialLevels = potentials.ToArray();

			var starTypes = new List<StarType>();
			while (queue.CountOf(StarTypeKey) > 0)
			{
				var data = queue.Dequeue(StarTypeKey).To<IkonComposite>();
				var type = new StarType(
					extractColor(data["color"].To<IkonArray>()),
					data["minSize"].To<double>(),
					data["maxSize"].To<double>(),
					data["traits"].To<string[]>()
				);

				starTypes.Add(type);
				if (data == homeStarData)
					this.homeStarType = type;
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

		public void SetGameData(Dictionary<string, StarTraitType> starTraits, Dictionary<string, PlanetTraitType> planetTraits, Dictionary<PlanetType, IEnumerable<string>> implicitTraits)
		{
			this.planetTraits = planetTraits;
			this.starTraits = starTraits;
			this.planetTraitGroups = bodyTypes().ToDictionary(
				type => type,
				type => this.planetTraitIdGroups.
					Where(group => implicitTraits[type].All(id => !group.Contains(id))).
					Select(group => group.Select(id => this.planetTraits[id]).ToArray()).
					ToArray()
			);
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
			var uninhabitedStars = starPositions.Stars.Where((x, i) => !starPositions.HomeSystems.Contains(i));
			var homeStars = new HashSet<Vector2D>(starPositions.HomeSystems.Select(i => starPositions.Stars[i]));

			var potentials = new Dictionary<Vector2D, double>();
			var starts = new Dictionary<Vector2D, double>();
			var climate = this.climateLevels[this.climateParameter.Value];
			var potential = this.potentialLevels[this.potentialParameter.Value];
			foreach (var home in homeStars)
			{
				potentials[home] = potential.HomesystemPotentialScore;
				starts[home] = Math.Min(climate.HomesystemStartScore, potentials[home]);
			}

			var undistributed = new HashSet<Vector2D>(uninhabitedStars);
			var weightSum = potential.Ranges.Sum(x => x.Weight);
			foreach (var range in potential.Ranges)
			{
				var count = (int)Math.Round(range.Weight * undistributed.Count() / weightSum);
				foreach(var star in spreadPoints(rng, undistributed, count))
				{
					potentials[star] = Methods.Lerp(rng.NextDouble(), range.Min, range.Max);
					undistributed.Remove(star);
				}
				weightSum -= range.Weight;
			}

			undistributed = new HashSet<Vector2D>(uninhabitedStars);
			weightSum = climate.Ranges.Sum(x => x.Weight);
			foreach (var range in climate.Ranges)
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
				yield return generateSystem(namer, position, rng, evaluator, starts[position], potentials[position], homeStars.Contains(position));
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
						Methods.FindWorst(centroids, x => (point - x).LengthSquared)
					].Add(point);

				centroids.Clear();
				foreach (var group in grouping)
				{
					var center = group.Value.Aggregate((a, b) => a + b) / group.Value.Count;
					var closest = Methods.FindWorst(group.Value, x => (center - x).LengthSquared);

					centroids.Add(closest);
					advanced |= closest != group.Key;
				}
			} while (advanced);

			return centroids;
		}

		private StarSystemBuilder generateSystem(StarNamer namer, Vector2D position, Random rng, SystemEvaluator evaluator, double startingScore, double potentialScore, bool isHomeSystem)
		{
			var starType = isHomeSystem ? this.homeStarType : starTypes[rng.Next(starTypes.Length)];
			var starName = namer.NextName();

			var fixedParts = new StarSystemBuilder(
				starType.Hue,
				(float)Methods.Lerp(rng.NextDouble(), starType.MinScale, starType.Maxscale),
				starName, position, 
				new List<StarTraitType>(starType.Traits.Select(x => this.starTraits[x]))
			);
			var usedPositions = new HashSet<int>();
			if (isHomeSystem)
			{
				fixedParts.AddPlanet(this.homeworldPosition, PlanetType.Rock, this.homeworldSize, this.homeworldTraits.Select(x => this.planetTraits[x]));
				usedPositions.Add(this.homeworldPosition);
			}

			var systems = new List<StarSystemBuilder>();
			for (int i = 0; i < SysGenRepeats; i++)
			{
				var system = new StarSystemBuilder(fixedParts);
				systems.Add(system);
				var planets = rng.Next(5);
				var bodyPosition = 1;
				for (int p = 0; p < planets; p++)
				{
					var type = bodyTypes()[rng.Next(3)];
					var size = Math.Round(Methods.Lerp(rng.NextDouble(), 50, 200));
					while (usedPositions.Contains(bodyPosition))
						bodyPosition++;
					
					system.AddPlanet(bodyPosition, type, size, randomTraits(rng, type, size));
					bodyPosition++;
				}
			}

			return Methods.FindWorst(systems, x => Methods.MeanSquareError(
				evaluator.StartingScore(x) - startingScore,
				evaluator.PotentialScore(x) - potentialScore
			));
		}

		private IEnumerable<PlanetTraitType> randomTraits(Random rng, PlanetType bodyType, double size)
		{
			var targetCount = rng.Next(this.planetTraitGroups[bodyType].Length + 1);
			var options = new PickList<PlanetTraitType[]>(rng, this.planetTraitGroups[bodyType]);

			while (options.Count() > targetCount)
				options.Take();

			var vars = new Var("size", size).
				And("asteroid", bodyType == PlanetType.Asteriod).
				And("rock", bodyType == PlanetType.Rock).
				And("gasGiant", bodyType == PlanetType.GasGiant).
				Init(this.planetTraits.Keys, false);
			foreach (var group in this.planetTraitGroups[bodyType].Where(x => options.InnerList.Contains(x)))
			{
				var applicableTraits = new PickList<PlanetTraitType>(rng, group.
					Where(x => !this.traitConditions.ContainsKey(x.IdCode) || this.traitConditions[x.IdCode].Evaluate(vars.Get) >= 0)
				);
				if (applicableTraits.Count() > 0)
				{
					var trait = applicableTraits.Pick();

					yield return trait;
					vars.Set(trait.IdCode, 1);
				}
			}
		}

		private static PlanetType[] bodyTypes()
		{
			return new[] { PlanetType.Asteriod, PlanetType.GasGiant, PlanetType.Rock };
		}
	}
}
