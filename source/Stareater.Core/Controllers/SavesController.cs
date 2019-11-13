using System;
using System.Linq;
using System.Collections.Generic;
using Stareater.Controllers.Views;
using System.IO;
using Ikadn;
using Ikadn.Ikon.Types;
using Ikadn.Ikon;
using Ikadn.Utilities;

namespace Stareater.Controllers
{
	public class SavesController
	{
		private const string SaveFolderName = "/SavedGames/";
		private const string SaveNamePrefix = "game";
		private const string SaveNameExtension = "save";

		private readonly GameController gameController;
		private int nextSaveNumber;
		private LinkedList<SavedGameInfo> games;
		private readonly string saveFolderPath; //TODO(later) remove file dependency
		
		public SavesController(GameController gameController, string saveFolderRoot)
		{
			this.gameController = gameController;
			this.saveFolderPath = saveFolderRoot  + SaveFolderName;
			checkFiles();
		}

		#region Files
		private void checkFiles()
		{
			var saveFolder = new DirectoryInfo(this.saveFolderPath);
			var saveFiles = new Dictionary<SavedGameInfo, DateTime>();
			var saveNames = new HashSet<string>();

			if (!saveFolder.Exists)
			{
				this.nextSaveNumber = 1;
				this.games = new LinkedList<SavedGameInfo>();
				return;
			}
			
			foreach (var file in saveFolder.EnumerateFiles("*." + SaveNameExtension))
			{
				saveNames.Add(file.Name);

#if DEBUG
				if (file.Length == 0)
				{
					System.Diagnostics.Trace.TraceWarning("Empty save file: " + file.Name);
					continue;
				}
#endif

				using (var parser = new IkonParser(file.OpenText()))
				{
#if !DEBUG
					try
					{
#endif
					var versionData = parser.ParseNext() as IkonArray;
					var title = parser.ParseNext().To<string>();
					var previewData = parser.ParseNext() as IkonBaseObject;
					saveFiles.Add(
						new SavedGameInfo(
							title,
							previewData,
                            file
						),
						file.LastWriteTimeUtc
					);
#if !DEBUG
				}
					catch(IOException)
					{
						//TODO(later) Notify about corrupted save file						
					}
#endif
				}
			}

			for (this.nextSaveNumber = 1;
				saveNames.Contains(SaveNamePrefix + this.nextSaveNumber + "." + SaveNameExtension);
				this.nextSaveNumber++)
				;
			this.games = new LinkedList<SavedGameInfo>(saveFiles.OrderByDescending(x => x.Value).Select(x => x.Key));
		}
#endregion

		#region Indicators
		public bool CanSave
		{
			get { return gameController.State == GameState.Running; }
		}
		
		public IEnumerable<SavedGameInfo> Games
		{
			get
			{
				return this.games;
			}
		}
		#endregion

		#region Saving / Loading
		public void NewSave(string title, IkonBaseObject previewData)
		{
			string fileName = SaveNamePrefix + this.nextSaveNumber + "." + SaveNameExtension;

			var saveFile = new FileInfo(this.saveFolderPath + fileName);
			saveFile.Directory.Create();

			save(saveFile, title, previewData);
			
			this.nextSaveNumber++;
		}

		public void OverwriteSave(SavedGameInfo savedGameData, string title, IkonBaseObject previewData)
		{
			save(savedGameData.FileInfo, title, previewData);
		}

		private void save(FileInfo saveFile, string title, IkonBaseObject previewData)
		{
			using (var output = new StreamWriter(saveFile.Create()))
			{
				var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
				var versionData = new IkonArray
				{
					new IkonInteger(version.Major),
					new IkonInteger(version.Minor),
					new IkonInteger(version.Revision),
					new IkonInteger(version.Build)
				};

				var writer = new IkadnWriter(output);
				versionData.Compose(writer);
				new IkonText(title).Compose(writer);
				previewData.Compose(writer);
				gameController.Save().Compose(writer);
			}
		}
		
		public void Load(SavedGameInfo savedGameData, IEnumerable<NamedStream> staticDataSources)
		{
			IkonComposite saveRawData;
			using (var parser = new IkonParser(savedGameData.FileInfo.OpenText()))
				saveRawData = parser.ParseAll().Dequeue(MainGame.SaveGameTag).To<IkonComposite>();

			this.gameController.LoadGame(GameBuilder.LoadGame(saveRawData, staticDataSources, GameController.GetStateManager()));
		}
		#endregion
	}
}
