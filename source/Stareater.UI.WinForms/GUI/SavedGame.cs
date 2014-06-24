using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers.Data;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.GUI
{
	public partial class SavedGame : UserControl
	{
		private SavedGameData gameData;
		
		public SavedGame()
		{
			InitializeComponent();
		}
		
		public SavedGameData Data 
		{
			get
			{
				return this.gameData;
			}
			set
			{
				this.gameData = value;
				
				Context context = SettingsWinforms.Get.Language[FormSaveLoad.LanguageContext];
				
				if (value != null)
				{
					this.gameName.Text = gameData.Title;
					this.turnText.Text = context["Turn"].Text(null, new TextVar("turn", gameData.Turn.ToString()).Get);
				}
				else
				{
					this.gameName.Text = "";
					this.turnText.Text = context["NewSave"].Text();
				}
			}
		}
		
		void PreviewClick(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void GameNameClick(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void TurnTextClick(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
