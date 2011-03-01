using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Prototip;

namespace Zvjezdojedac_editori.Validation
{
	public class ValidTextBoxSetUniqeness : AValidTextBox
	{
		protected HashSet<string> set;

		public ValidTextBoxSetUniqeness(TextBox txtInput, Label lblReport,
			HashSet<string> set)
			: base(txtInput, lblReport)
		{
			this.set = set;
		}

		public override bool valid()
		{
			return !set.Contains(txtInput.Text.Trim());
		}
	}
}
