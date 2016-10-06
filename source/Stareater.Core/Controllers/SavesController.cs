using System;
using System.Linq;
using System.Collections.Generic;
using Stareater.Controllers.Views;
using System.IO;
using Ikadn;
using Ikadn.Ikon.Types;
using Ikadn.Ikon;

namespace Stareater.Controllers
{
	public class SavesController
	{
		private const string SaveFolderName = "/SavedGames/";
		private const string SaveNamePrefix = "game";
		private const string SaveNameExtension = "save";

		private const string SaveGameTitleKey = "title";

		private GameController gameController;
		private int nextSaveNumber;
		private LinkedList<SavedGameInfo> games;
		private string saveFolderPath; //TODO(v0.6) remove file dependency
		
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
			
			foreach (var file in saveFolder.EnumerateFiles("*." + SaveNameExtension)) {
				saveNames.Add(file.Name);

				using (var parser = new IkonParser(file.OpenText())) {
#if !DEBUG
					try
					{
#endif						
						var rawData = parser.ParseNext() as IkonComposite;
						saveFiles.Add(
							new SavedGameInfo(
								rawData[SaveGameTitleKey].To<string>(),
								rawData[MainGame.TurnKey].To<int>(),
								rawData,
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
		public void NewSave(string title)
		{
			string fileName = SaveNamePrefix + this.nextSaveNumber + "." + SaveNameExtension;

			var saveFile = new FileInfo(this.saveFolderPath + fileName);
			saveFile.Directory.Create();

			save(saveFile, title);
			
			this.nextSaveNumber++;
		}

		public void OverwriteSave(SavedGameInfo savedGameData, string title)
		{
			save(savedGameData.FileInfo, title);
		}

		private void save(FileInfo saveFile, string title)
		{
			using (var output = new StreamWriter(saveFile.Create())) {
				var gameData = gameController.GameInstance.Save();
				gameData.Add(SaveGameTitleKey, new IkonText(title));

				var writer = new IkadnWriter(output);
				gameData.Compose(writer);
			}
		}
		
		public void Load(SavedGameInfo savedGameData)
		{
			this.gameController.LoadGame(GameBuilder.LoadGame(savedGameData.RawData));
		}
		#endregion
	}
}
