using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Stareater.Utils;
using Stareater.GameData.Reading;
using Stareater.Controllers.Views;

namespace Stareater.Players
{
	public static class PlayerAssets
	{
		internal static Organization[] OrganizationsRaw { get; private set; }

		public static Color[] Colors { get; private set; }
		public static Dictionary<string, IOffscreenPlayerFactory> AIDefinitions { get; private set; }

		public static IEnumerable<OrganizationInfo> Organizations
		{
			get { return OrganizationsRaw.Select(x => new OrganizationInfo(x)).ToList(); }
		}

		public static void ColorLoader(IEnumerable<TracableStream> dataSources)
		{
			var colorList = new List<Color>();
			foreach(var source in dataSources)
				using (var parser = new IkonParser(source.Stream))
				{
					IkonArray colorsData;

					try
					{
						colorsData = parser.ParseNext().To<IkonComposite>()[ColorsKey].To<IkonArray>();
					}
					catch (IOException e)
					{
						throw new IOException(source.SourceInfo, e);
					}
					catch (FormatException e)
					{
						throw new FormatException(source.SourceInfo, e);
					}

					foreach (var item in colorsData)
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

		public static void AILoader(IEnumerable<IOffscreenPlayerFactory> aiFactories)
		{
			AIDefinitions = aiFactories.ToDictionary(x => x.Id);
        }

		public static void OrganizationsLoader(IEnumerable<TracableStream> dataSources)
		{
			var list = new List<Organization>();
			foreach (var source in dataSources)
				using (var parser = new Parser(source.Stream))
				{
					try
					{
						foreach (var item in parser.ParseAll())
						{
							var data = item.Value.To<IkonComposite>();
							list.Add(new Organization(
								data[Stareater.GameData.Databases.StaticsDB.GeneralCodeKey].To<string>(),
								data[OrganizationLangCodeKey].To<string>(),
								data[OrganizationAffinitiesKey].To<string[]>()
							));
						}
					}
					catch (IOException e)
					{
						throw new IOException(source.SourceInfo, e);
					}
					catch (FormatException e)
					{
						throw new FormatException(source.SourceInfo, e);
					}
				}

			OrganizationsRaw = list.ToArray();
		}

		public static bool IsLoaded
		{
			get { return Colors != null && AIDefinitions != null && OrganizationsRaw != null; }
		}

		#region Attribute keys
		private const string ColorsKey = "Colors";
		private const string OrganizationLangCodeKey = "langCode";
		private const string OrganizationAffinitiesKey = "affinities";
		#endregion
	}
}
