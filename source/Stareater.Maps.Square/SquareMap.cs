using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;
using Stareater.Utils.PluginParameters;
using Ikon;
using System.IO;
using Ikon.Ston.Values;

namespace Stareater.Maps.Square
{
	public class SquareMap : IStarPositioner
	{
		const string ParametersFile = "square_map.txt";
		const string LanguageContext = "SquareMap";
		const string SizeKey = "Size";

		private ParameterList parameters;
		private SizeOption[] sizeOptions;

		public SquareMap()
		{
			ValueQueue data;
			using (Ikon.Parser parser = new Ikon.Ston.Parser(new StreamReader(MapAssets.MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			this.parameters = new ParameterList(new ParameterBase[]{
				loadSizes(data),
				new RangeParameter<double>(LanguageContext, "displacement", 0, 0.5, 0.25, displacementPercentage),
			});
		}

		private ParameterBase loadSizes(ValueQueue data)
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
			get { return null; }
		}

		public ParameterList Parameters
		{
			get
			{
				return parameters;
			}
		}
	}
}
