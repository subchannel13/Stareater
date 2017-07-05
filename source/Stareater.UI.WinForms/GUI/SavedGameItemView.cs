using System;
using System.Windows.Forms;

using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.Properties;
using Stareater.Utils.Collections;

namespace Stareater.GUI
{
	public partial class SavedGameItemView : UserControl
	{
		private SavedGameInfo gameData;
		
		public SavedGameItemView()
		{
			InitializeComponent();
		}
		
		public SavedGameInfo Data 
		{
			get
			{
				return this.gameData;
			}
			set
			{
				this.gameData = value;
				
				Context context = LocalizationManifest.Get.CurrentLanguage[FormSaveLoad.LanguageContext];
				
				if (value != null)
				{
					this.gameName.Text = gameData.Title;
					this.turnText.Text = context["Turn"].Text(null, new TextVar("turn", gameData.Turn.ToString()).Get);
				}
				else
				{
					this.preview.Image = Resources.newSave;
					this.gameName.Text = context["NewSave"].Text();
					this.turnText.Text = "";
				}
			}
		}

		public string GameName
		{
			get { return this.gameName.Text; }
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
