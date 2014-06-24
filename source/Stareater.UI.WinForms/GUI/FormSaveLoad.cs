using System;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.Controllers.Data;

namespace Stareater.GUI
{
	public partial class FormSaveLoad : Form
	{
		public const string LanguageContext = "FormSaveLoad";
		
		private SavesController controller;
		
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
