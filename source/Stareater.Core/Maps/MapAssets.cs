using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Stareater.Utils;
using Ikadn.Ikon.Types;
using Ikadn.Ikon;

namespace Stareater.Maps
{
	public class MapAssets
	{
		private const string StartConditionsFilePath = "./data/start_conditions.txt";
		public const string MapsFolder = "./maps/";

		#region Attribute keys
		const string StartingConditionsKey = "StartinConditions";
		#endregion

		public static StartingConditions[] Starts { get; private set; }
		public static IStarPositioner[] StarPositioners { get; private set; }
		public static IStarConnector[] StarConnectors { get; private set; }
		public static IStarPopulator[] StarPopulators { get; private set; }

		public static IEnumerable<double> StartConditionsLoader()
		{
			List<StartingConditions> conditionList = new List<StartingConditions>();
			using (var parser = new IkonParser(new StreamReader(StartConditionsFilePath))) {
				var conditions = parser.ParseAll();
				yield return 0.5;

				foreach (double p in Methods.ProgressReportHelper(0.5, 0.5, conditions.Count))
				{
					conditionList.Add(new StartingConditions(conditions.Dequeue().To<IkonComposite>()));
					yield return p;
				}
			}

			Starts = conditionList.ToArray();
			yield return 1;
		}

		public static IEnumerable<double> PositionersLoader()
		{
			List<FileInfo> dllFiles = new List<FileInfo>(new DirectoryInfo(MapsFolder).EnumerateFiles("*.dll"));
			yield return 0.1;

			List<IStarPositioner> factoryList = new List<IStarPositioner>();
			foreach (var p in Methods.ProgressReportHelper(0.1, 0.8, dllFiles))
			{
				factoryList.AddRange(Methods.LoadFromDLL<IStarPositioner>(p.Data.FullName));
				yield return p.Percentage;
			}

			StarPositioners = factoryList.ToArray();
			yield return 1;
		}

		public static IEnumerable<double> ConnectorsLoader()
		{
			List<FileInfo> dllFiles = new List<FileInfo>(new DirectoryInfo(MapsFolder).EnumerateFiles("*.dll"));
			yield return 0.1;

			List<IStarConnector> factoryList = new List<IStarConnector>();
			foreach (var p in Methods.ProgressReportHelper(0.1, 0.8, dllFiles))
			{
				factoryList.AddRange(Methods.LoadFromDLL<IStarConnector>(p.Data.FullName));
				yield return p.Percentage;
			}
			
			StarConnectors = factoryList.ToArray();
			yield return 1;
		}

		public static IEnumerable<double> PopulatorsLoader()
		{
			List<FileInfo> dllFiles = new List<FileInfo>(new DirectoryInfo(MapsFolder).EnumerateFiles("*.dll"));
			yield return 0.1;

			List<IStarPopulator> factoryList = new List<IStarPopulator>();
			foreach (var p in Methods.ProgressReportHelper(0.1, 0.8, dllFiles))
			{
				factoryList.AddRange(Methods.LoadFromDLL<IStarPopulator>(p.Data.FullName));
				yield return p.Percentage;
			}

			StarPopulators = factoryList.ToArray();
			yield return 1;
		}

		public static bool IsLoaded
		{
			get { return Starts != null && StarPositioners != null; }
		}
	}
}
