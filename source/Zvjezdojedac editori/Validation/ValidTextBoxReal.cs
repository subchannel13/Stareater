using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Prototip;

namespace Zvjezdojedac_editori.Validation
{
	public class ValidTextBoxReal : AValidTextBox
	{
		public ValidTextBoxReal(TextBox txtInput, Label lblReport)
			: base(txtInput, lblReport)
		{	}

		public override bool valid()
		{
			double x;
			return double.TryParse(txtInput.Text, NumberStyles.AllowDecimalPoint, PodaciAlat.DecimalnaTocka, out x);
		}
	}
}
