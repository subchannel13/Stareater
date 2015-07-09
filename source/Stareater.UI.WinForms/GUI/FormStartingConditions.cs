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
using Stareater.Galaxy;
using Stareater.Controllers;
using Stareater.Utils.NumberFormatters;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public partial class FormStartingConditions : Form
	{
		private static readonly Color validColor = SystemColors.Window;
		private static readonly Color invalidColor = Color.LightPink;

		public FormStartingConditions()
		{
			InitializeComponent();

			setLanguage();
		}

		public void Initialize(StartingConditions condition)
		{
			coloniesSelector.Maximum = StarData.MaxPlanets;
			coloniesSelector.Value = condition.Colonies;

			var numberFormat = new ThousandsFormatter(condition.Population, condition.Infrastructure);
			populationInput.Text = numberFormat.Format(condition.Population);
			infrastructureInput.Text = numberFormat.Format(condition.Infrastructure);
		}

		private void setLanguage()
		{
			Context context = SettingsWinforms.Get.Language["FormStartingConditions"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["FormTitle"].Text();

			coloniesSelector.Text = context["coloniesSelector"].Text();
			populationLabel.Text = context["populationLabel"].Text();
			infrastructureLabel.Text = context["infrastructureLabel"].Text();

			acceptButton.Text = SettingsWinforms.Get.Language["General"]["DialogAccept"].Text();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		public bool IsValid
		{
			get
			{
				return NumberInput.DecodeQuantity(populationInput.Text) != null && NumberInput.DecodeQuantity(infrastructureInput.Text) != null;
			}
		}

		public StartingConditions GetResult()
		{
			return new StartingConditions(
				NumberInput.DecodeQuantity(populationInput.Text).Value,
				(int)coloniesSelector.Value,
				NumberInput.DecodeQuantity(infrastructureInput.Text).Value,
				NewGameController.CustomStartNameKey);
		}

		private void populationInput_TextChanged(object sender, EventArgs e)
		{
			populationInput.BackColor = NumberInput.DecodeQuantity(populationInput.Text) != null ? validColor : invalidColor;
		}

		private void infrastructureInput_TextChanged(object sender, EventArgs e)
		{
			infrastructureInput.BackColor = NumberInput.DecodeQuantity(infrastructureInput.Text) != null ? validColor : invalidColor;
		}

		private void acceptButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
	}
}
