using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers.Data;
using Stareater.Localization;
using Stareater.Properties;
using Stareater.Utils.Collections;

namespace Stareater.GUI
{
	public partial class SavedGameItemView : UserControl
	{
		private SavedGameData gameData;
		
		public SavedGameItemView()
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
					this.preview.Image = Resources.newSave;
					this.gameName.Text = "";
					this.turnText.Text = context["NewSave"].Text();
				}
			}
		}
		
		public void OnSelect()
		{
			this.gameName.Enabled = true;
			this.gameName.Focus();
		}
		
		public void Deselect()
		{
			this.gameName.Enabled = false;
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
