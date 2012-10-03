using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Stareater.AppData;
using Stareater.Localization;

namespace Stareater.GUI
{
	public partial class FormMainMenu : Form
	{
		Font initialFont;

		public FormMainMenu()
		{
			InitializeComponent();
			this.initialFont = Font;

			setLanguage(SettingsWinforms.Get.Language);
		}

		private void setLanguage(Language newLanguage)
		{
			Context context = newLanguage["FormMainMenu"];

			this.Text = context["FormTitle"];
			titleLabel.Text = context["Title"];

			newGameButton.Text = context["NewGame"];
			loadGameButton.Text = context["Load"];
			saveGameButton.Text = context["Save"];
			settingsButton.Text = context["Settings"];
			exitButton.Text = context["Quit"];
		}
	}
}
