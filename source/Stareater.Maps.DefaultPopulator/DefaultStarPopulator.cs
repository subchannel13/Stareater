using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;
using Stareater.Utils.PluginParameters;

namespace Stareater.Maps.DefaultPopulator
{
	public class DefaultStarPopulator : IStarPopulator
	{
		const string LanguageContext = "DefaultPopulator";

		ParameterList parameters;
		private SelectorParameter climateParameter;

		public DefaultStarPopulator()
		{
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
	}
}
