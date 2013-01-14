using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Localization;
using Stareater.AppData;
using Stareater.Maps;

namespace Stareater.GUI
{
	public partial class FormStartingConditions : Form
	{
		public FormStartingConditions()
		{
			InitializeComponent();

			setLanguage();
		}

		public void Initialize(StartingConditions condition)
		{
		}

		private void setLanguage()
		{
			Context context = SettingsWinforms.Get.Language["FormStartingConditions"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["FormTitle"];
		}
	}
}
