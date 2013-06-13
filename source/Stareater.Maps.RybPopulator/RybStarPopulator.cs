using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;
using Stareater.Utils.PluginParameters;
using Ikadn;
using System.IO;
using Ikadn.Ikon.Types;
using System.Drawing;
using Ikadn.Ikon;
using Ikadn.Utilities;

namespace Stareater.Maps.RybPopulator
{
	public class RybStarPopulator : IStarPopulator
	{
		private const string ParametersFile = "ryb_populator.txt";

		private const string LanguageContext = "DefaultPopulator";

		private const string StarTypeKey = "StarType";
		private const string StarColorKey = "color";
		private const string StarMinRadiationKey = "minRadiation";
		private const string StarMaxRadiationKey = "maxRadiation";

		private ParameterList parameters;
		private SelectorParameter climateParameter;

		private StarType[] starTypes;

		public RybStarPopulator()
		{
			TaggableQueue<object, IkadnBaseObject> data; 
			using (var parser = new IkonParser(new StreamReader(MapAssets.MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			List<StarType> starTypes = new List<StarType>();
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

			parameters = new ParameterList(new ParameterBase[]{
				climateParameter,
			});
		}

		private Color extractColor(IkonArray arrayValue)
		{
			return Color.FromArgb(
				(int)(arrayValue[0].To<double>() * 255),
				(int)(arrayValue[1].To<double>() * 255),
				(int)(arrayValue[2].To<double>() * 255)
			);
		}

		public string Name
		{
			get { return Settings.Get.Language[LanguageContext]["name"].Text(); }
		}

		public string Description
		{
			get
			{
				return Settings.Get.Language[LanguageContext]["description"].Text(new Dictionary<string, double>()
				{
					{"badClime", climateParameter.Value == 0 ? 1 : -1},
					{"avgClime", climateParameter.Value == 1 ? 1 : -1},
					{"goodClime", climateParameter.Value == 2 ? 1 : -1},
				});
			}
		}

		public ParameterList Parameters
		{
			get { return parameters; }
		}

		public IEnumerable<StarData> Generate(Random rng, StarPositions starPositions)
		{
			int colorI = 0;
			foreach (var position in starPositions.Stars)
				yield return new StarData(starTypes[colorI++ % starTypes.Length].Hue, 1, "Test", position, 0);
		}
	}
}
