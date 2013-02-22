using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;
using Stareater.AppData;
using System.IO;
using Ikon.Ston;
using Ikon.Ston.Values;
using Ikon;

namespace Stareater.Maps.ProximityLanes
{
	public class ProximityLanesBuilder : IStarConnector
	{
		const string ParametersFile = "proximity_lanes.txt";

		const string LanguageContext = "ProximityLanes";
		const string DegreeKey = "Degree";

		private ParameterList parameters;
		private DegreeOption[] degreeOptions;

		public ProximityLanesBuilder()
		{
			ValueQueue data;
			using (Ikon.Parser parser = new Ikon.Ston.Parser(new StreamReader(MapAssets.MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			this.parameters = new ParameterList(new ParameterBase[]{
				loadDegrees(data),
			});
		}

		private ParameterBase loadDegrees(ValueQueue data)
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
			get { return null; }
		}

		public ParameterList Parameters
		{
			get { return parameters; }
		}
	}
}
