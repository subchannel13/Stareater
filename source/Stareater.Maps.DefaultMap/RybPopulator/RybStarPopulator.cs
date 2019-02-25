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

		private const string StarTypeKey = "StarType";
		private const string StarColorKey = "color";
		private const string StarMinRadiationKey = "minRadiation";
		private const string StarMaxRadiationKey = "maxRadiation";

		private SelectorParameter climateParameter;

		private StarType[] starTypes;
		private TraitType[] planetTraits;

		public void Initialize(string dataPath)
		{
			TaggableQueue<object, IkadnBaseObject> data;
			using (var parser = new IkonParser(new StreamReader(dataPath + ParametersFile)))
				data = parser.ParseAll();

			var starTypes = new List<StarType>();
			while (data.CountOf(StarTypeKey) > 0)
			{
				var starTypeData = data.Dequeue(StarTypeKey).To<IkonComposite>();
				starTypes.Add(new StarType(
					extractColor(starTypeData[StarColorKey].To<IkonArray>()),
					starTypeData[StarMinRadiationKey].To<double>(),
					starTypeData[StarMaxRadiationKey].To<double>()
				));
			}
			this.starTypes = starTypes.ToArray();

			//TODO(v0.8) make data driven
			this.climateParameter = new SelectorParameter(LanguageContext, "climate", new Dictionary<int, string>()
			{
				{0, "hostileClimate"},
				{1, "normalClimate"},
				{2, "paradiseClimate"},
			}, 1);
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
				return LocalizationManifest.Get.CurrentLanguage[LanguageContext]["description"].Text(new Dictionary<string, double>()
				{
					{"badClime", climateParameter.Value == 0 ? 1 : -1},
					{"avgClime", climateParameter.Value == 1 ? 1 : -1},
					{"goodClime", climateParameter.Value == 2 ? 1 : -1},
				});
			}
		}

		public IEnumerable<AParameterBase> Parameters
		{
			get { yield return climateParameter; }
		}

		public double MinPlanets
		{
			get { return 0; }
		}

		public double MaxPlanets
		{
			get { return 3; }
		}

		public double MinPlanetSize(PlanetType type)
		{
			//TODO(v0.8) load from file
			return 100;
		}

		public double MaxPlanetSize(PlanetType type)
		{
			//TODO(v0.8) load from file
			return 200;
		}

		public IEnumerable<StarSystemBuilder> Generate(Random rng, StarPositions starPositions)
		{
			int colorI = 0;
			var namer = new StarNamer(starPositions.Stars.Length, new Random());

			//TODO(v0.8): Randomize star type distribution
			//TODO(v0.8): Star size and trait distribution
			foreach (var position in starPositions.Stars)
			{
				var system = new StarSystemBuilder(starTypes[colorI++ % starTypes.Length].Hue, 1, namer.NextName(), position, new List<TraitType>());
				system.AddPlanet(1, PlanetType.Rock, Methods.Lerp(rng.NextDouble(), 50, 200), randomTraits(rng));
				system.AddPlanet(2, PlanetType.Asteriod, Methods.Lerp(rng.NextDouble(), 50, 200), randomTraits(rng));
				system.AddPlanet(3, PlanetType.GasGiant, Methods.Lerp(rng.NextDouble(), 50, 200), randomTraits(rng));

				yield return system;
			}
		}

		private List<TraitType> randomTraits(Random rng)
		{
			var targetCount = rng.Next(planetTraits.Length + 1);
			var options = new PickList<TraitType>(rng, planetTraits);

			while (options.Count() > targetCount)
				options.Take();

			return options.InnerList;
		}
	}
}
