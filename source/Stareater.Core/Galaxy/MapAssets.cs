using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Galaxy.Builders;

namespace Stareater.Galaxy
{
	public static class MapAssets
	{
		public static ReadOnlyCollection<StartingConditions> Starts { get; private set; }
		public static ReadOnlyCollection<IStarPositioner> StarPositioners { get; private set; }
		public static ReadOnlyCollection<IStarConnector> StarConnectors { get; private set; }
		public static ReadOnlyCollection<IStarPopulator> StarPopulators { get; private set; }

		public static void StartConditionsLoader(IEnumerable<NamedStream> dataSources)
		{
			var conditionList = new List<StartingConditions>();
			using (var parser = new IkonParser(dataSources))
				foreach (var item in parser.ParseAll())
				{
					var start = StartingConditions.Load(item.Value.To<IkonComposite>());
					if (start != null)
						conditionList.Add(start);
					else
						throw new FormatException();
				}

			Starts = Array.AsReadOnly(conditionList.ToArray());
		}

		public static void PositionersLoader(IEnumerable<IStarPositioner> factoryList)
		{
			StarPositioners = Array.AsReadOnly(factoryList.ToArray());
		}

		public static void ConnectorsLoader(IEnumerable<IStarConnector> factoryList)
		{
			StarConnectors = Array.AsReadOnly(factoryList.ToArray());
		}

		public static void PopulatorsLoader(IEnumerable<IStarPopulator> factoryList)
		{
			StarPopulators = Array.AsReadOnly(factoryList.ToArray());
		}

		public static bool IsLoaded
		{
			get { return Starts != null && StarPositioners != null && StarConnectors != null && StarPopulators != null; }
		}
	}
}
