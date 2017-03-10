using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.Localization;
using System.IO;
using Stareater.Players;

namespace Stareater.AppData
{
	static class LoadingMethods
	{
		#region Localization
		private const string LanguagesFolder = "./languages/";
		private const string DefaultLangSufix = "(default)";
		private static string DefaultLangCode = null;

		public static void InitializeLocalization()
		{
			Language currentLanguage = null;
			Language defaultLanguage = null;
			var infos = new List<LanguageInfo>();

			foreach (var folder in new DirectoryInfo(LanguagesFolder).EnumerateDirectories())
			{
				string code = folder.Name;

				if (folder.Name.EndsWith(DefaultLangSufix, StringComparison.InvariantCultureIgnoreCase))
				{
					code = code.Remove(code.Length - DefaultLangSufix.Length);
					DefaultLangCode = code;
				}
				
				Language lang = LoadLanguage(code);

				if (code == DefaultLangCode)
					defaultLanguage = lang;
				if (code == SettingsWinforms.Get.LanguageId)
					currentLanguage = lang;

				
				infos.Add(new LanguageInfo(code, lang["General"]["LanguageName"].Text()));
			}

			if (SettingsWinforms.Get.LanguageId == null)
				currentLanguage = defaultLanguage;

			LocalizationManifest.Initialize(infos, defaultLanguage, currentLanguage);
		}

		public static Language LoadLanguage(string langCode)
		{
			var folderSufix = langCode == DefaultLangCode ? DefaultLangSufix : "";

			return new Language(
				langCode,
				dataStreams(new DirectoryInfo(LanguagesFolder + langCode + folderSufix).EnumerateFiles())
			);
		}
		#endregion

		#region Player assets
		private const string AIsFolder = "./players/";
		private static readonly string[] OrganizationFiles = { "./data/organizations.txt" };
		private static readonly string[] PlayersColorFiles = { "./data/playerData.txt" };
		
		public static void LoadAis()
		{
			PlayerAssets.AILoader(loadFromDLLs<IOffscreenPlayerFactory>(AIsFolder));
		}
		
		public static void LoadOrganizations()
		{
			PlayerAssets.OrganizationsLoader(dataStreams(OrganizationFiles.Select(x => new FileInfo(x))));
		}
		
		public static void LoadPlayerColors()
		{
			PlayerAssets.ColorLoader(dataStreams(PlayersColorFiles.Select(x => new FileInfo(x))));
		}
		#endregion
		
		#region Map assets
		private static readonly string[] StartConditionsFiles = { "./data/startConditions.txt" };
		public const string MapsFolder = "./maps/";
		
		public static void LoadStarConnectors()
		{
			MapAssets.ConnectorsLoader(loadFromDLLs<IStarConnector>(MapsFolder));
		}
		
		public static void LoadStarPopulators()
		{
			MapAssets.PopulatorsLoader(loadFromDLLs<IStarPopulator>(MapsFolder));
		}
		
		public static void LoadStarPositioners()
		{
			MapAssets.PositionersLoader(loadFromDLLs<IStarPositioner>(MapsFolder));
		}
		
		public static void LoadStartConditions()
		{
			MapAssets.StartConditionsLoader(dataStreams(StartConditionsFiles.Select(x => new FileInfo(x))));
		}
		#endregion
		
		#region Game data
		private static readonly string StaticDataFolder = "./data/statics/";
		
		public static IEnumerable<TextReader> GameDataSources()
		{
			return dataStreams(new DirectoryInfo(StaticDataFolder).EnumerateFiles());
		}
		#endregion
		
		private static IEnumerable<TextReader> dataStreams(IEnumerable<FileInfo> files)
		{
			foreach (var file in files)
			{
				var stream = new StreamReader(file.FullName);
				yield return stream;
				stream.Close();
			}
		}
		
		private static IEnumerable<T> loadFromDLLs<T>(string folderPath)
		{
			var dllFiles = new List<FileInfo>(new DirectoryInfo(folderPath).EnumerateFiles("*.dll"));
			Type targetType = typeof(T);
			
			foreach (var file in dllFiles)
				foreach (var type in Assembly.UnsafeLoadFrom(file.FullName).GetTypes()) //TODO(later) consider more secure approach
					if (targetType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
						yield return (T)Activator.CreateInstance(type);
		}
	}
}
