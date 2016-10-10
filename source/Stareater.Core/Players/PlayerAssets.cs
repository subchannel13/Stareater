using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using Stareater.Utils;
using Ikadn.Ikon.Types;
using Ikadn.Ikon;

namespace Stareater.Players
{
	public static class PlayerAssets
	{
		private const string AIsFolder = "./players/";

		#region Attribute keys
		const string ColorsKey = "Colors";
		#endregion

		public static Color[] Colors { get; private set; }
		public static IOffscreenPlayerFactory[] AIDefinitions { get; private set; }
		//TODO(v0.6) move organization list here

		public static void ColorLoader(IEnumerable<TextReader> dataSources)
		{
			var colorList = new List<Color>();
			foreach(var source in dataSources)
			{
				var parser = new IkonParser(source);
				var colorsData = parser.ParseNext().To<IkonComposite>()[ColorsKey].To<IkonArray>();
				foreach(var item in colorsData)
				{
					var colorData = item.To<IkonArray>();
					colorList.Add(Color.FromArgb(
						colorData[0].To<int>(),
						colorData[1].To<int>(),
						colorData[2].To<int>()
						));
				}
			}

			Colors = colorList.ToArray();
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
