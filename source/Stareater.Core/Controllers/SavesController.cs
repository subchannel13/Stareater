using System;
using System.Collections.Generic;
using Stareater.Controllers.Data;
using System.IO;
using Stareater.AppData;
using Ikadn;
using Ikadn.Ikon.Types;

namespace Stareater.Controllers
{
	public class SavesController
	{
		private const string SaveFolderName = "/SavedGames/";
		private const string SaveNamePrefix = "game";
		private const string SaveNameExtension = ".save";

		private const string SaveGameTitleKey = "title";

		private GameController gameController;
		private int nextSaveNumber;
		
		public SavesController(GameController gameController)
		{
			this.gameController = gameController;
			nextSaveNumber = 1; //TODO(v0.5) calculate from files (max found +1)
		}

		#region Indicators
		private static string SaveFolderPath
		{
			get
			{
				return AssetController.Get.FileStorageRootPath + SaveFolderName;
			}
		}

		public bool CanSave
		{
			get { return gameController.State == GameState.Running; }
		}
		
		public IEnumerable<SavedGameData> Games
		{
			get
			{
				return SavedGames.Games;
			}
		}
		#endregion

		#region Saving / Loading
		public void Save(SavedGameData savedGameData)
		{
			string fileName = SaveNamePrefix + this.nextSaveNumber + SaveNameExtension;

			FileInfo saveFile = new FileInfo(SaveFolderPath + fileName);
			saveFile.Directory.Create();

			using (var output = new StreamWriter(saveFile.OpenWrite())) {
				var gameData = gameController.GameInstance.Save();
				gameData.Add(SaveGameTitleKey, new IkonText(savedGameData.Title));
				
				IkadnWriter writer = new IkadnWriter(output);
				gameData.Compose(writer);
			}

			this.nextSaveNumber++;
		}

		public void Load(SavedGameData savedGameData)
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
		#endregion
	}
}
