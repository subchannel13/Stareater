using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;
using Stareater.Utils.PluginParameters;

namespace Stareater.Maps.Square
{
	public class SquareMap : IStarPositioner
	{
		const string LanguageContext = "SquareMap";

		ParameterList parameters = new ParameterList(new ParameterBase[]{
			new SelectorParameter(LanguageContext, "size", new Dictionary<int, string>()
			{
				{0, "miniatureSize"},
				{1, "smallSize"}
			}, 1),
			new RangeParameter<double>(LanguageContext, "displacement", 0, 0.5, 0.25, displacementPercentage),
		});

		public static string displacementPercentage(double displacement)
		{
			return (2 * displacement * 100).ToString("0") + "%";
		}

		public string Name
		{
			get { return Settings.Get.Language[LanguageContext]["name"]; }
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
