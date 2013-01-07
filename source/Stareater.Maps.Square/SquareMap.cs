using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;

namespace Stareater.Maps.Square
{
	public class SquareMap : IMapFactory
	{
		public string Name
		{
			get { return Settings.Get.Language["SquareMap"]["name"]; }
		}

		public IEnumerable<MapFactoryParameterInfo> Parameters()
		{
			yield return new MapFactoryParameterInfo("size", new Dictionary<int, string>()
			{
				{1, "sf"}
			});
		}
	}
}
