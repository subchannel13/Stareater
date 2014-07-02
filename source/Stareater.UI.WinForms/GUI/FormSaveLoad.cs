using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Data;
using Stareater.Localization;

namespace Stareater.GUI
{
	public partial class FormSaveLoad : Form
	{
		public const string LanguageContext = "FormSaveLoad";
		
		private SavesController controller;
		private Label noSavedMessage = null;
		
		internal MainMenuResult Result { get; private set; }
		
		public FormSaveLoad()
		{
			InitializeComponent();
		}
		
		public FormSaveLoad(SavesController controller) : this()
		{
			this.controller = controller;
			
			if (controller.CanSave)
				addSavedGame(null);
			
			foreach (var data in controller.Games)
				addSavedGame(data);
			
			updateNoSaveMessage();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void addSavedGame(SavedGameData gameData)
		{
			var itemView = new SavedGame();
			itemView.Data = gameData;
			
			gameList.Controls.Add(itemView);
		}
		
		private void updateNoSaveMessage()
		{
			if (noSavedMessage == null && controller.Games.Count() == 0)
			{
				Context context = SettingsWinforms.Get.Language[FormSaveLoad.LanguageContext];
				
				noSavedMessage = new Label();
				noSavedMessage.Size = new Size(298, 23);
				noSavedMessage.TextAlign = ContentAlignment.TopCenter;
				noSavedMessage.Text = context["NoSaves"].Text();
				
				gameList.Controls.Add(noSavedMessage);
			}
		}
		
		void SaveButtonClick(object sender, EventArgs e)
		{
			this.Result = MainMenuResult.SaveGame;
			this.DialogResult = DialogResult.OK;
		}
		
		void LoadButtonClick(object sender, EventArgs e)
		{
			this.Result = MainMenuResult.LoadGame;
			this.DialogResult = DialogResult.OK;
		}
	}
}
