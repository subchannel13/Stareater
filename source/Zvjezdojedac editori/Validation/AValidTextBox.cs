using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Prototip;

namespace Zvjezdojedac_editori.Validation
{
	public abstract class AValidTextBox : AValid
	{
		public TextBox txtInput { get; private set; }
		public Label lblReport { get; private set; }

		public AValidTextBox(TextBox txtInput, Label lblReport)
		{
			this.txtInput = txtInput;
			this.lblReport = lblReport;

			txtInput.TextChanged += txtInput_TextChanged;
		}

		public override void setWarning()
		{
			if (lblReport != null)
				lblReport.Visible = !valid();
		}

		private void txtInput_TextChanged(object sender, EventArgs e)
		{
			if (changeAlerter != null)
				changeAlerter(txtInput);
		}
	}
}
