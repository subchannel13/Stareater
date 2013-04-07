using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;
using Stareater.AppData;
using System.IO;
using Ikadn;
using Ikadn.Ikon.Values;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Maps.ProximityLanes
{
	public class ProximityLanesBuilder : IStarConnector
	{
		const string ParametersFile = "proximity_lanes.txt";

		const string LanguageContext = "ProximityLanes";
		const string DegreeKey = "Degree";

		private SelectorParameter degreesParameter;
		private ParameterList parameters;
		private DegreeOption[] degreeOptions;

		public ProximityLanesBuilder()
		{
			ValueQueue data;
			using (var parser = new Ikadn.Ikon.Parser(new StreamReader(MapAssets.MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			degreesParameter = loadDegrees(data);
			this.parameters = new ParameterList(new ParameterBase[]{
				degreesParameter,
			});
		}

		private SelectorParameter loadDegrees(ValueQueue data)
		{
			this.degreeOptions = new DegreeOption[data.CountOf(DegreeKey)];
			var parameterOptions = new Dictionary<int, string>();
			for (int i = 0; i < degreeOptions.Length; i++) {
				degreeOptions[i] = new DegreeOption(data.Dequeue(DegreeKey).To<ObjectValue>());
				parameterOptions.Add(i, degreeOptions[i].Name);
			}

			return new SelectorParameter(LanguageContext, DegreeKey, parameterOptions, (int)Math.Ceiling(parameterOptions.Count / 2.0));
		}

		public string Name
		{
			get { return Settings.Get.Language[LanguageContext]["name"].Text(); }
		}

		public string Description
		{
			get
			{
				return Settings.Get.Language[LanguageContext]["description"].Text(new Dictionary<string, double>(){
				{"minDegree", degreeOptions[degreesParameter.Value].Min},
				{"maxDegree", degreeOptions[degreesParameter.Value].Max}
				});
			}
		}

		public ParameterList Parameters
		{
			get { return parameters; }
		}


		public IEnumerable<Tuple<Vector2D, Vector2D>> Generate(Random rng, StarPositions starPositions)
		{
			throw new NotImplementedException();
		}
	}
}
