using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Stareater.GameData.Reading;
using Stareater.Controllers.Views;
using Ikadn.Utilities;
using System.Collections.ObjectModel;
using System;

namespace Stareater.Players
{
	public static class PlayerAssets
	{
		internal static Organization[] OrganizationsRaw { get; private set; }

		public static ReadOnlyCollection<Color> Colors { get; private set; }
		public static Dictionary<string, IOffscreenPlayerFactory> AIDefinitions { get; private set; }

		public static IEnumerable<OrganizationInfo> Organizations
		{
			get { return OrganizationsRaw.Select(x => new OrganizationInfo(x)).ToList(); }
		}

		public static void ColorLoader(IEnumerable<NamedStream> dataSources)
		{
			var colorList = new List<Color>();
			using (var parser = new IkonParser(dataSources))
				foreach (var item in parser.ParseNext().To<IkonComposite>()[ColorsKey].To<IkonArray>())
				{
					var colorData = item.To<IkonArray>();
					colorList.Add(Color.FromArgb(
						colorData[0].To<int>(),
						colorData[1].To<int>(),
						colorData[2].To<int>()
						));
				}

			Colors = Array.AsReadOnly(colorList.ToArray());
		}

		public static void AILoader(IEnumerable<IOffscreenPlayerFactory> aiFactories)
		{
			AIDefinitions = aiFactories.ToDictionary(x => x.Id);
        }

		public static void OrganizationsLoader(IEnumerable<NamedStream> dataSources)
		{
			var list = new List<Organization>();
			using (var parser = new Parser(dataSources))
				foreach (var item in parser.ParseAll())
				{
					var data = item.Value.To<IkonComposite>();
					list.Add(new Organization(
						data[Stareater.GameData.Databases.StaticsDB.GeneralCodeKey].To<string>(),
						data[OrganizationLangCodeKey].To<string>(),
						data[OrganizationAffinitiesKey].To<string[]>()
					));
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
