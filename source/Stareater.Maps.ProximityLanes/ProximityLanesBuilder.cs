using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;
using Stareater.AppData;

namespace Stareater.Maps.ProximityLanes
{
	public class ProximityLanesBuilder : IStarConnector
	{
		const string LanguageContext = "ProximityLanes";

		ParameterList parameters = new ParameterList(new ParameterBase[]{
			new SelectorParameter(LanguageContext, "degree", new Dictionary<int, string>()
			{
				{0, "few"},
				{1, "average"},
				{2, "many"}
			}, 1),
		});

		public string Name
		{
			get { return Settings.Get.Language[LanguageContext]["name"]; }
		}

		public ParameterList Parameters
		{
			get { return parameters; }
		}
	}
}
