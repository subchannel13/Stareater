using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Galaxy.Builders;
using Stareater.Localization;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.RybPopulator
{
	public class RybStarPopulator : IStarPopulator
	{
		private const string MapsFolder = "./maps/"; //TODO(v0.7) try to move it to view
		private const string ParametersFile = "rybPopulator.txt";

		private const string LanguageContext = "DefaultPopulator";

		private const string StarTypeKey = "StarType";
		private const string StarColorKey = "color";
		private const string StarMinRadiationKey = "minRadiation";
		private const string StarMaxRadiationKey = "maxRadiation";

		private SelectorParameter climateParameter;

		private readonly StarType[] starTypes;

		public RybStarPopulator()
		{
			TaggableQueue<object, IkadnBaseObject> data; 
			using (var parser = new IkonParser(new StreamReader(MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			var starTypes = new List<StarType>();
			while (data.CountOf(StarTypeKey) > 0) {
				var starTypeData = data.Dequeue(StarTypeKey).To<IkonComposite>();
				starTypes.Add(new StarType(
					extractColor(starTypeData[StarColorKey].To<IkonArray>()),
					starTypeData[StarMinRadiationKey].To<double>(),
					starTypeData[StarMaxRadiationKey].To<double>()
				));
			}
			this.starTypes = starTypes.ToArray();

			this.climateParameter = new SelectorParameter(LanguageContext, "climate", new Dictionary<int, string>()
			{
				{0, "hostileClimate"},
				{1, "normalClimate"},
				{2, "paradiseClimate"},
			}, 1);
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

		public IEnumerable<StarSystem> Generate(Random rng, StarPositions starPositions, IEnumerable<BodyTraitType> planetTraits)
		{
			int colorI = 0;
			var namer = new StarNamer(starPositions.Stars.Length);

			//UNDONE(later): Picks star types cyclicaly
			//TODO(later): Randomize star type distribution
			//TODO(later): Star size and radiation distribution
			foreach (var position in starPositions.Stars) {
				var star = new StarData(starTypes[colorI++ % starTypes.Length].Hue, 1, namer.NextName(), position, new List<BodyTraitType>());
				
				yield return new StarSystem(
					star,
					new Planet[] {
						new Planet(star, 1, PlanetType.Rock, 100, planetTraits.Take(1).ToList()),
						new Planet(star, 2, PlanetType.Asteriod, 100, new List<BodyTraitType>()),
						new Planet(star, 3, PlanetType.GasGiant, 100, new List<BodyTraitType>()),
					});
			}
		}
	}
}
