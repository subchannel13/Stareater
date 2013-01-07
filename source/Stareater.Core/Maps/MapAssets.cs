using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Stareater.Utils;
using Ikon.Ston.Values;

namespace Stareater.Maps
{
	public class MapAssets
	{
		private const string StartConditionsFilePath = "./data/start_conditions.txt";
		private const string MapsFolder = "./maps/";

		#region Attribute keys
		const string StartingConditionsKey = "StartinConditions";
		const string ColoniesKey = "colonies";
		const string PopulationKey = "population";
		const string InfrastructureKey = "infrastructure";
		const string NameKey = "nameKey";
		#endregion

		public static StartingConditions[] Starts { get; private set; }
		public static IMapFactory[] MapFactories { get; private set; }

		public static IEnumerable<double> StartConditionsLoader()
		{
			List<StartingConditions> conditionList = new List<StartingConditions>();
			using (Ikon.Ston.Parser parser = new Ikon.Ston.Parser(new StreamReader(StartConditionsFilePath))) {
				var conditions = parser.ParseAll();
				yield return 0.5;

				foreach (double p in Methods.ProgressReportHelper(0.5, 0.5, conditions.Count, () =>
				{
					var conditionData = conditions.Dequeue() as ObjectValue;
					conditionList.Add(new StartingConditions(
						(conditionData[PopulationKey] as NumericValue).GetLong,
						(conditionData[ColoniesKey] as NumericValue).GetInt,
						(conditionData[InfrastructureKey] as NumericValue).GetLong,
						(conditionData[NameKey] as TextValue).GetText
					));
				}))
					yield return p;
			}

			Starts = conditionList.ToArray();
			yield return 1;
		}

		public static IEnumerable<double> MapsLoader()
		{
			List<FileInfo> dllFiles = new List<FileInfo>(new DirectoryInfo(MapsFolder).EnumerateFiles("*.dll"));
			yield return 0.1;

			List<IMapFactory> factoryList = new List<IMapFactory>();
			Type targetType = typeof(IMapFactory);
			foreach (double p in Methods.ProgressReportHelper(0.1, 0.8, dllFiles, (dllFile) =>
			{
				foreach (var type in Assembly.LoadFile(dllFile.FullName).GetTypes())
					if (targetType.IsAssignableFrom(type))
						factoryList.Add(Activator.CreateInstance(type) as IMapFactory);
			}))
				yield return p;

			MapFactories = factoryList.ToArray();
			yield return 1;
		}

		public static bool IsLoaded
		{
			get { return Starts != null && MapFactories != null; }
		}
	}
}
