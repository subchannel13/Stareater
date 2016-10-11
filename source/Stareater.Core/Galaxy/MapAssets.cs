using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Stareater.Galaxy.Builders;

namespace Stareater.Galaxy
{
	public static class MapAssets
	{
		

		#region Attribute keys
		const string StartingConditionsKey = "StartinConditions";
		#endregion

		public static StartingConditions[] Starts { get; private set; }
		public static IStarPositioner[] StarPositioners { get; private set; }
		public static IStarConnector[] StarConnectors { get; private set; }
		public static IStarPopulator[] StarPopulators { get; private set; }

		public static void StartConditionsLoader(IEnumerable<TextReader> dataSources)
		{
			var conditionList = new List<StartingConditions>();
			foreach(var source in dataSources)
			{
				var parser = new IkonParser(source);
				foreach (var item in parser.ParseAll())
					conditionList.Add(new StartingConditions(item.Value.To<IkonComposite>()));
			}

			Starts = conditionList.ToArray();
		}

		public static void PositionersLoader(IEnumerable<IStarPositioner> factoryList)
		{
			StarPositioners = factoryList.ToArray();
		}

		public static void ConnectorsLoader(IEnumerable<IStarConnector> factoryList)
		{
			StarConnectors = factoryList.ToArray();
		}

		public static void PopulatorsLoader(IEnumerable<IStarPopulator> factoryList)
		{
			StarPopulators = factoryList.ToArray();
		}

		public static bool IsLoaded
		{
			get { return Starts != null && StarPositioners != null && StarConnectors != null && StarPopulators != null; }
		}
	}
}
