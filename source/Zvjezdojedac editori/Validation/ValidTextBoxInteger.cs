using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Prototip;

namespace Zvjezdojedac_editori.Validation
{
	public class ValidTextBoxInteger : AValidTextBox
	{
		public ValidTextBoxInteger(TextBox txtInput, Label lblReport)
			: base(txtInput, lblReport)
		{	}

		public override bool valid()
		{
			long x;
			return long.TryParse(txtInput.Text, out x);
		}
	}
}
