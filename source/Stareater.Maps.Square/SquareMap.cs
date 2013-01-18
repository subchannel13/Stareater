using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;

namespace Stareater.Maps.Square
{
	public class SquareMap : IMapFactory
	{
		const string LanguageContext = "SquareMap";

		public string Name
		{
			get { return Settings.Get.Language[LanguageContext]["name"]; }
		}

		public IEnumerable<MapFactoryParameterInfo> Parameters()
		{
			yield return new MapFactoryParameterInfo(LanguageContext, "size", new Dictionary<int, string>()
			{
				{6, "miniatureSize"},
				{7, "smallSize"}
			});
		}
	}
}
