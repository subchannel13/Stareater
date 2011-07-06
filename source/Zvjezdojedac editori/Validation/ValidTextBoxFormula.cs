using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Zvjezdojedac;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac_editori.Validation
{
	public class ValidTextBoxFormula : AValidTextBox
	{
		protected HashSet<string> acceptableVariables = null;

		public ValidTextBoxFormula(TextBox txtInput, Label lblReport)
			: base(txtInput, lblReport)
		{	}

		public ValidTextBoxFormula(TextBox txtInput, Label lblReport,
			HashSet<string> acceptableVariables)
			: base(txtInput, lblReport)
		{
			this.acceptableVariables = new HashSet<string>(acceptableVariables);
		}

		public ValidTextBoxFormula(TextBox txtInput, Label lblReport,
			string[] acceptableVariables)
			: this(txtInput, lblReport, new HashSet<string>(acceptableVariables))
		{	}

		public override bool valid()
		{
			if (!Formula.ValjanaFormula(txtInput.Text)) return false;			
			
			Formula formula = Formula.IzStringa(txtInput.Text);
			List<string> varijable = new List<string>();
			foreach (Formula.Varijabla var in formula.popisVarijabli())
				varijable.Add(var.ime);

			if (acceptableVariables != null)
				if (!acceptableVariables.IsSupersetOf(varijable))
					return false;

			return true;
		}
	}
}
