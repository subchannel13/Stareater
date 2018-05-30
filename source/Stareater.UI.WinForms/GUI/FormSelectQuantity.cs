using System;
using System.Drawing;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Localization;
using Stareater.Utils;
using Stareater.Utils.NumberFormatters;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public partial class FormSelectQuantity : Form
	{
		private static readonly Color validColor = SystemColors.Window;
		private static readonly Color invalidColor = Color.LightPink;
		
		private bool ignoreEvents = false;
		private readonly long maximum;
		
		public FormSelectQuantity()
		{
			InitializeComponent();
			
			setLanguage();
		}
		
		public FormSelectQuantity(long max, long current) : this()
		{
			this.maximum = max;
			
			var formatter = new ThousandsFormatter();
			this.quantitySlider.Maximum = max > this.quantitySlider.Maximum ? this.quantitySlider.Maximum : (int)max;
			this.quantityInput.Text = formatter.Format(current);
		}
		
		public long SelectedValue 
		{ 
			get
			{
				return NumberInput.DecodeQuantity(this.quantityInput.Text) ?? this.maximum;
			}
		}
		
		private void setLanguage()
		{
			Context context = LocalizationManifest.Get.CurrentLanguage["FormMain"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["SelectQuantityTitle"].Text();

			this.acceptButton.Text = LocalizationManifest.Get.CurrentLanguage["General"]["DialogAccept"].Text();
		}
		
		private void quantitySlider_Scroll(object sender, EventArgs e)
		{
			if (ignoreEvents) return;
			
			ignoreEvents = true;
			
			if (this.maximum == this.quantitySlider.Maximum)
				this.quantityInput.Text = this.quantitySlider.Value.ToString();
			else
			{
				var formatter = new ThousandsFormatter();
				this.quantityInput.Text = formatter.Format(Math.Round(this.maximum * (this.quantitySlider.Value / (double) this.quantitySlider.Maximum)));
			}
			ignoreEvents = false;
		}
		
		private void quantityInput_TextChanged(object sender, EventArgs e)
		{
			if (ignoreEvents) return;
			
			ignoreEvents = true;
			
			var quantity = NumberInput.DecodeQuantity(quantityInput.Text);
			if (quantity.HasValue && (quantity.Value < 0 || quantity.Value > maximum))
			    quantity = null;
			
			quantityInput.BackColor = quantity.HasValue ? validColor : invalidColor;
			if (quantity.HasValue)
			{
				this.quantitySlider.Value = (int)((quantity.Value / (double) maximum) * this.quantitySlider.Maximum);
			}
			ignoreEvents = false;
		}
		
		private void acceptButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
	}
}
