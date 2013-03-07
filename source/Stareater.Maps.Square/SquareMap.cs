using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;
using Stareater.Utils.PluginParameters;
using System.IO;
using Ikadn;
using Ikadn.Ikon.Values;

namespace Stareater.Maps.Square
{
	public class SquareMap : IStarPositioner
	{
		const string ParametersFile = "square_map.txt";
		const string LanguageContext = "SquareMap";
		const string SizeKey = "Size";

		private SelectorParameter sizeParameter;
		private RangeParameter<double> displacementParameter;
		private ParameterList parameters;
		private SizeOption[] sizeOptions;

		public SquareMap()
		{
			ValueQueue data;
			using (var parser = new Ikadn.Ikon.Parser(new StreamReader(MapAssets.MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			sizeParameter = loadSizes(data);
			displacementParameter = new RangeParameter<double>(LanguageContext, "displacement", 0, 0.5, 0.25, displacementPercentage);
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
	}
}
