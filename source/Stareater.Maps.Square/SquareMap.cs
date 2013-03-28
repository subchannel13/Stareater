using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;
using Stareater.Utils.PluginParameters;
using System.IO;
using Ikadn;
using Ikadn.Ikon.Values;
using Stareater.Utils;

namespace Stareater.Maps.Square
{
	public class SquareMap : IStarPositioner
	{
		const string ParametersFile = "square_map.txt";
		const string LanguageContext = "SquareMap";
		const string ConstantsKey = "Constants";
		const string SizeKey = "Size";
		const string StarDistanceKey = "starDistance";
		const string DefaultDisplacementKey = "defaultDisplacement";
		const string HomeSystemDistance = "homeSystemDistance";

		private SelectorParameter sizeParameter;
		private RangeParameter<double> displacementParameter;
		private ParameterList parameters;
		private SizeOption[] sizeOptions;

		private double starDistance = 1;
		private double homeSystemDistance = 0.5;

		public SquareMap()
		{
			ValueQueue data;
			using (var parser = new Ikadn.Ikon.Parser(new StreamReader(MapAssets.MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			var constants = data.Dequeue(ConstantsKey).To<ObjectValue>();
			this.starDistance = constants[StarDistanceKey].To<double>();
			this.homeSystemDistance = constants[HomeSystemDistance].To<double>();

			sizeParameter = loadSizes(data);
			displacementParameter = new RangeParameter<double>(LanguageContext, "displacement", 0, 0.5, constants[DefaultDisplacementKey].To<double>(), displacementPercentage);
			this.parameters = new ParameterList(new ParameterBase[]{
				sizeParameter,
				displacementParameter,
			});
		}

		private SelectorParameter loadSizes(ValueQueue data)
		{
			this.sizeOptions = new SizeOption[data.CountOf(SizeKey)];
			var parameterOptions = new Dictionary<int, string>();
			for (int i = 0; i < sizeOptions.Length; i++) {
				sizeOptions[i] = new SizeOption(data.Dequeue(SizeKey).To<ObjectValue>());
				parameterOptions.Add(i, sizeOptions[i].Name);
			}

			return new SelectorParameter(LanguageContext, SizeKey, parameterOptions, (int)Math.Ceiling(parameterOptions.Count / 2.0));
		}

		static string displacementPercentage(double displacement)
		{
			return (2 * displacement * 100).ToString("0") + "%";
		}

		public string Name
		{
			get { return Settings.Get.Language[LanguageContext]["name"].Text(); }
		}

		public string Description
		{
			get { return Settings.Get.Language[LanguageContext]["description"].Text(Math.Pow(sizeOptions[sizeParameter.Value].Size, 2)); }
		}

		public ParameterList Parameters
		{
			get
			{
				return parameters;
			}
		}


		public StarPositions Generate(Random rng, int playerCount)
		{
			var positions = new List<Point2d>();
			int size = sizeOptions[sizeParameter.Value].Size;
			double displacement = displacementParameter.Value;

			for (double y = 0; y < size; y++)
				for (double x = 0; x < size; x++)
					positions.Add(new Point2d(
						(x + displacementParameter.Value * (2 * rng.NextDouble() - 1)) * starDistance - size / 2.0,
						(y + displacementParameter.Value * (2 * rng.NextDouble() - 1)) * starDistance - size / 2.0
						));

			var homeSystems = new List<Point2d>();
			double phi = 0.5 * rng.NextDouble() * Math.PI;
			double deltaPhi = Math.PI * 2.0 / playerCount;
			double radius = starDistance * (size - 1) / 2.0;

			for (double player = 0; player < playerCount; player++) {
				Point2d desiredPoint = new Point2d(
					radius * Math.Cos(phi + player * deltaPhi),
					radius * Math.Sin(phi + player * deltaPhi)
				);

				homeSystems.Add(positions.Aggregate((starA, starB) =>
					desiredPoint.Distance(starA) < desiredPoint.Distance(starB) ? starA : starB));
			}

			return new StarPositions(positions, homeSystems);
		}
	}
}
