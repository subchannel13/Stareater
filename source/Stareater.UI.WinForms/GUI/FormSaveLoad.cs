using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Localization;

namespace Stareater.GUI
{
	public sealed partial class FormSaveLoad : Form
	{
		public const string LanguageContext = "FormSaveLoad";

		private GameController gameController;
        private SavesController saveController;
		private Label noSavedMessage = null;
		private SavedGameItemView selectedGame = null;
		
		internal MainMenuResult Result { get; private set; }
		
		public FormSaveLoad()
		{
			InitializeComponent();
		}
		
		public FormSaveLoad(SavesController saveController, GameController gameController) : this()
		{
			this.Text = LocalizationManifest.Get.CurrentLanguage[FormSaveLoad.LanguageContext]["FormTitle"].Text();
			this.Font = SettingsWinforms.Get.FormFont;
			this.gameController = gameController;
			this.saveController = saveController;
			
			if (saveController.CanSave) {
				addSavedGame(null);
				this.saveButton.Enabled = true;
			}
			
			foreach (var data in saveController.Games)
				addSavedGame(data);
			
			updateNoSaveMessage();
		}
		
		public SavedGameInfo SelectedGameData
		{
			get { return this.selectedGame.Data; }
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void addSavedGame(SavedGameInfo gameData)
		{
			var itemView = new SavedGameItemView();
			itemView.Data = gameData;
			
			gameList.Controls.Add(itemView);
		}
		
		private void updateNoSaveMessage()
		{
			if (noSavedMessage == null && !saveController.Games.Any())
			{
				Context context = LocalizationManifest.Get.CurrentLanguage[FormSaveLoad.LanguageContext];
				
				noSavedMessage = new Label();
				noSavedMessage.Size = new Size(298, 23);
				noSavedMessage.TextAlign = ContentAlignment.TopCenter;
				noSavedMessage.Text = context["NoSaves"].Text();
				
				gameList.Controls.Add(noSavedMessage);
			}
		}
		
		private void saveButton_Click(object sender, EventArgs e)
		{
			var preview = new SavePreviewGenerator(this.gameController);

            if (this.selectedGame.Data == null)
				this.saveController.NewSave(this.selectedGame.GameName, preview.Make());
			else
				this.saveController.OverwriteSave(this.selectedGame.Data, this.selectedGame.GameName, preview.Make());

			this.Result = MainMenuResult.SaveGame;
			this.DialogResult = DialogResult.OK;
		}
		
		private void loadButton_Click(object sender, EventArgs e)
		{
			this.Result = MainMenuResult.LoadGame;
			this.DialogResult = DialogResult.OK;
		}
		
		private void gameList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.loadButton.Enabled &= !(this.gameList.SelectedItem is Label);
			
			if (this.gameList.SelectedItem == null || !(this.gameList.SelectedItem is SavedGameItemView) || this.gameList.SelectedItem == this.selectedGame)
				return;
			
			if (this.selectedGame != null)
				this.selectedGame.Deselect();
			
			this.selectedGame = this.gameList.SelectedItem as SavedGameItemView;
			this.selectedGame.OnSelect();
			
			this.loadButton.Enabled = (this.selectedGame.Data != null);
		}
	}
}
