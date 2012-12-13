using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Ikon.Ston.Values;
using Stareater.Utils;

namespace Stareater.Players
{
	public static class PlayerAssets
	{
		private const string DataFilePath = "./data/player_data.txt";

		#region Attribute keys
		const string ColorsKey = "Colors";
		#endregion

		public static Color[] Colors { get; private set; }

		public static IEnumerable<double> Loader()
		{
			List<Color> colorList = new List<Color>();
			using (Ikon.Ston.Parser parser = new Ikon.Ston.Parser(new StreamReader(DataFilePath))) {
				var data = parser.ParseNext() as ObjectValue;
				yield return 0.5;

				var colors = (data[ColorsKey] as ArrayValue).GetList;
				foreach (double p in Methods.ProgressReportHelper(0.5, 0.5, colors, (colorArray) =>
				{
					var colorData = (colorArray as ArrayValue).GetList;
					colorList.Add(Color.FromArgb(
						(colorData[0] as NumericValue).GetInt,
						(colorData[1] as NumericValue).GetInt,
						(colorData[2] as NumericValue).GetInt
						));
				}))
					yield return p;
			}

			Colors = colorList.ToArray();
			yield return 1;
		}

		public static bool IsLoaded
		{
			get { return Colors != null; }
		}
	}
}
