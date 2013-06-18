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

			acceptButton.Text = context["acceptButton"].Text();
			cancelButton.Text = context["cancelButton"].Text();
		}

		private static long? decodeNumber(string text)
		{
			long result = -1;

			text = text.Trim();
			double? prefixedValue = ThousandsFormatter.TryParse(text);
			if (prefixedValue.HasValue)
				result = (long)prefixedValue.Value;
			else if (text.ToLower().Contains("e")) {
				double resultScientific;
				if (double.TryParse(text, out resultScientific))
					result = (long)resultScientific;
				else
					return null;
			}
			else if (!long.TryParse(text, out result))
					return null;

			return (result < 0) ? null : new long?(result);
		}

		public bool IsValid
		{
			get
			{
				return decodeNumber(populationInput.Text) != null && decodeNumber(infrastructureInput.Text) != null;
			}
		}

		public StartingConditions GetResult()
		{
			return new StartingConditions(
				decodeNumber(populationInput.Text).Value,
				(int)coloniesSelector.Value,
				decodeNumber(infrastructureInput.Text).Value,
				NewGameController.CustomStartNameKey);
		}

		private void populationInput_TextChanged(object sender, EventArgs e)
		{
			populationInput.BackColor = decodeNumber(populationInput.Text) != null ? validColor : invalidColor;
		}

		private void infrastructureInput_TextChanged(object sender, EventArgs e)
		{
			infrastructureInput.BackColor = decodeNumber(infrastructureInput.Text) != null ? validColor : invalidColor;
		}

		private void acceptButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}
