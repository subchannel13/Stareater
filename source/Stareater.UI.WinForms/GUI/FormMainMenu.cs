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
	public partial class FormMainMenu : AForm
	{
		Font initialFont;

		public FormMainMenu()
		{
			InitializeComponent();
			this.initialFont = Font;
			
			OnLanguageChanged(SettingsWinforms.Get.Language);
		}

		public override void OnLanguageChanged(Language newLanguage)
		{
			/*private void postaviJezik()
		
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormMain];

			btnNovaIgra.Text = jezik["NOVA_IGRA"].tekst(null);
			btnPostavke.Text = jezik["POSTAVKE"].tekst(null);
			btnUcitaj.Text = jezik["UCITAJ"].tekst(null);
			btnUgasi.Text = jezik["UGASI"].tekst(null);
		*/
		}
	}
}
