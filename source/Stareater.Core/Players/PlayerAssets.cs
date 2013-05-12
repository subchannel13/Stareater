using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Stareater.Utils;
using System.Reflection;
using Ikadn.Ikon.Values;

namespace Stareater.Players
{
	public static class PlayerAssets
	{
		private const string DataFilePath = "./data/player_data.txt";
		private const string AIsFolder = "./players/";

		#region Attribute keys
		const string ColorsKey = "Colors";
		#endregion

		public static Color[] Colors { get; private set; }
		public static IOffscreenPlayerFactory[] AIDefinitions { get; private set; }

		public static IEnumerable<double> ColorLoader()
		{
			List<Color> colorList = new List<Color>();
			using (var parser = new Ikadn.Ikon.Parser(new StreamReader(DataFilePath))) {
				var data = parser.ParseNext() as ObjectValue;
				yield return 0.5;

				var colors = data[ColorsKey].To<ArrayValue>();
				foreach (var p in Methods.ProgressReportHelper(0.5, 0.5, colors))
				{
					var colorData = p.Data.To<ArrayValue>();
					colorList.Add(Color.FromArgb(
						(colorData[0] as NumericValue).To<int>(),
						(colorData[1] as NumericValue).To<int>(),
						(colorData[2] as NumericValue).To<int>()
						));
					yield return p.Percentage;
				}
					
			}

			Colors = colorList.ToArray();
			yield return 1;
		}

		public static IEnumerable<double> AILoader()
		{
			List<FileInfo> dllFiles = new List<FileInfo>(new DirectoryInfo(AIsFolder).EnumerateFiles("*.dll"));
			yield return 0.1;

			List<IOffscreenPlayerFactory> aiList = new List<IOffscreenPlayerFactory>();
			foreach (var p in Methods.ProgressReportHelper(0.1, 0.8, dllFiles))
			{
				aiList.AddRange(Methods.LoadFromDLL<IOffscreenPlayerFactory>(p.Data.FullName));
				yield return p.Percentage;
			}
				

			AIDefinitions = aiList.ToArray();
			yield return 1;
		}

		public static bool IsLoaded
		{
			get { return Colors != null && AIDefinitions != null; }
		}
	}
}
