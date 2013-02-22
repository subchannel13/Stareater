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

		ParameterList parameters = new ParameterList(new ParameterBase[]{
			new SelectorParameter(LanguageContext, "hospitality", new Dictionary<int, string>()
			{
				{0, "hostileClimate"},
				{1, "normalClimate"},
				{2, "paradiseClimate"},
			}, 1),
		});

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
