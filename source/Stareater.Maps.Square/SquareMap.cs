using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy.Builders;
using Stareater.Localization;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.Square
{
	public class SquareMap : IStarPositioner
	{
		const string ParametersFile = "squareMap.txt";
		const string LanguageContext = "SquareMap";
		const string ConstantsKey = "Constants";
		const string SizeKey = "Size";
		const string StarDistanceKey = "starDistance";
		const string DefaultDisplacementKey = "defaultDisplacement";
		const string HomeSystemDistance = "homeSystemDistance";
		const string EmptyPositionsRatio = "emptyPositionsRatio";

		private SelectorParameter sizeParameter;
		private ContinuousRangeParameter displacementParameter;
		private SizeOption[] sizeOptions;

		private double starDistance = 1;
		private double homeSystemDistance = 0.5;
		private double emptyPositionsRatio = 0.25;

		public void Initialize(string dataPath)
		{
			TaggableQueue<object, IkadnBaseObject> data;
			using (var parser = new IkonParser(new StreamReader(dataPath + ParametersFile)))
				data = parser.ParseAll();

			var constants = data.Dequeue(ConstantsKey).To<IkonComposite>();
			this.starDistance = constants[StarDistanceKey].To<double>();
			this.homeSystemDistance = constants[HomeSystemDistance].To<double>();
			this.emptyPositionsRatio = constants[EmptyPositionsRatio].To<double>();

			this.sizeParameter = loadSizes(data);
			this.displacementParameter = new ContinuousRangeParameter(LanguageContext, "displacement", 0, 0.5, constants[DefaultDisplacementKey].To<double>(), displacementPercentage);
		}

		private SelectorParameter loadSizes(TaggableQueue<object, IkadnBaseObject> data)
		{
			this.sizeOptions = new SizeOption[data.CountOf(SizeKey)];
			var parameterOptions = new Dictionary<int, string>();
			for (int i = 0; i < this.sizeOptions.Length; i++) {
				this.sizeOptions[i] = new SizeOption(data.Dequeue(SizeKey).To<IkonComposite>());
				parameterOptions.Add(i, this.sizeOptions[i].Name);
			}

			return new SelectorParameter(LanguageContext, SizeKey, parameterOptions, (int)Math.Ceiling(parameterOptions.Count / 2.0));
		}

		static string displacementPercentage(double displacement)
		{
			return (2 * displacement * 100).ToString("0") + "%";
		}

		public string Code
		{
			get { return "SquareMap"; }
		}

		public string Name
		{
			get { return LocalizationManifest.Get.CurrentLanguage[LanguageContext]["name"].Text(); }
		}

		public string Description
		{
			get 
			{ 
				return LocalizationManifest.Get.CurrentLanguage[LanguageContext]["description"].Text(null, new TextVar(
					"size", Math.Pow(sizeOptions[sizeParameter.Value].Size, 2).ToString()).Get
				);
			}
		}

		public IEnumerable<AParameterBase> Parameters
		{
			get
			{
				yield return sizeParameter;
				yield return displacementParameter;
			}
		}

		public StarPositions Generate(Random rng, int playerCount)
		{
			int size = sizeOptions[sizeParameter.Value].Size;
			var allPositions = new PickList<Tuple<int, int>>(rng,
				Methods.Range(0, size * size, 1).
				Select(i => new Tuple<int, int>(i % size, i / size)).ToList()
			);

			var emptyPositions = new HashSet<Tuple<int, int>>();
			while (emptyPositions.Count < emptyPositionsRatio * size * size)
				emptyPositions.Add(allPositions.Take());

			var positions = new List<Vector2D>();
			double displacement = displacementParameter.Value;

			for (double y = 0; y < size; y++)
				for (double x = 0; x < size; x++)
					if (!emptyPositions.Contains(new Tuple<int, int>((int)x, (int)y)))
						positions.Add(new Vector2D(
							(x + displacementParameter.Value * (2 * rng.NextDouble() - 1) - size / 2.0) * starDistance,
							(y + displacementParameter.Value * (2 * rng.NextDouble() - 1) - size / 2.0) * starDistance
							));

			var homeSystems = new List<int>();
			double phi = 0.5 * rng.NextDouble() * Math.PI;
			double deltaPhi = Math.PI * 2.0 / playerCount;
			double radius = starDistance * homeSystemDistance * size / 2.0;

			for (double player = 0; player < playerCount; player++) {
				var desiredPoint = new Vector2D(
					radius * Math.Cos(phi + player * deltaPhi),
					radius * Math.Sin(phi + player * deltaPhi)
				);
				desiredPoint *= radius / Math.Max(Math.Abs(desiredPoint.X), Math.Abs(desiredPoint.Y));

				int candidate = 0;
				for (int i = 1; i < positions.Count; i++)
					if ((desiredPoint - positions[candidate]).Magnitude() > (desiredPoint - positions[i]).Magnitude())
						candidate = i;
				homeSystems.Add(candidate);
			}

			var centroid = positions.Aggregate(new Vector2D(0, 0), (subsum, vertex) => subsum + vertex) / positions.Count;
			var centralNode = positions.Aggregate((a, b) => ((a - centroid).Magnitude() < (b - centroid).Magnitude()) ? a : b);

			return new StarPositions(positions, homeSystems, positions.IndexOf(centralNode));
		}
	}
}
