using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Stareater.Utils;

namespace Stareater.Players
{
	public static class PlayerAssets
	{
		public static Color[] Colors { get; private set; }
		public static Dictionary<string, IOffscreenPlayerFactory> AIDefinitions { get; private set; }
		public static Organization[] Organizations { get; private set; }

		public static void ColorLoader(IEnumerable<TracableStream> dataSources)
		{
			var colorList = new List<Color>();
			foreach(var source in dataSources)
			{
				var parser = new IkonParser(source.Stream);
				IkonArray colorsData;

				try
				{
					colorsData = parser.ParseNext().To<IkonComposite>()[ColorsKey].To<IkonArray>();
				} 
				catch (IOException e)
				{
					throw new IOException(source.SourceInfo, e);
				}
				catch(FormatException e)
				{
					throw new FormatException(source.SourceInfo, e);
				}

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

		public static void AILoader(IEnumerable<IOffscreenPlayerFactory> aiFactories)
		{
			AIDefinitions = aiFactories.ToDictionary(x => x.Id);
        }

		public static void OrganizationsLoader(IEnumerable<TracableStream> dataSources)
		{
			var list = new List<Organization>();
			foreach (var source in dataSources)
			{
				var parser = new IkonParser(source.Stream); //TODO(v0.7) use extended data parser
				try
				{
					foreach (var item in parser.ParseAll())
					{
						var data = item.Value.To<IkonComposite>();
						list.Add(new Organization(
							data[OrganizationNameKey].To<string>(),
							data[OrganizationDescriptionKey].To<string>(),
							data[OrganizationAffinitiesKey].To<string[]>()
						));
					}
				} 
				catch (IOException e)
				{
					throw new IOException(source.SourceInfo, e);
				}
				catch(FormatException e)
				{
					throw new FormatException(source.SourceInfo, e);
				}
			}

			list.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
			Organizations = list.ToArray();
		}

		public static bool IsLoaded
		{
			get { return Colors != null && AIDefinitions != null && Organizations != null; }
		}

		internal static Organization RandomOrganization(Random rng)
		{
			return Organizations[rng.Next(Organizations.Length)];
		}

		#region Attribute keys
		private const string ColorsKey = "Colors";
		private const string OrganizationNameKey = "name";
		private const string OrganizationDescriptionKey = "description";
		private const string OrganizationAffinitiesKey = "affinities";
		#endregion
	}
}
