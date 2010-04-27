using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Prototip;

namespace Zvjezdojedac_editori
{
	public partial class ValidatorForm : Form
	{
		public enum InputType
		{
			IntegerNum,
			RealNum,
			Forumla
		}
		public class Validation
		{
			public TextBox txtInput { get; private set; }
			public InputType type { get; private set; }
			public Label lblReport { get; private set; }

			public Validation(TextBox txtInput, InputType type, Label lblReport)
			{
				this.lblReport = lblReport;
				this.txtInput = txtInput;
				this.type = type;
			}
		}

		private HashSet<Validation> validations = new HashSet<Validation>();
		private int change = 0;
		protected int changeDelay { get; set; }

		public ValidatorForm()
		{
			InitializeComponent();

			changeDelay = 300;
		}

		protected virtual void addoditionalChangeHandle()
		{
		}

		protected void addValidation(Validation validation)
		{
			validations.Add(validation);
			validation.txtInput.TextChanged += txtInput_TextChanged;
		}

		private bool check(Validation validation)
		{
			bool ok = true;

			if (validation.type == InputType.Forumla)
				ok = Formula.ValjanaFormula(validation.txtInput.Text);
			else if (validation.type == InputType.IntegerNum)
			{
				long tl;
				ok = long.TryParse(validation.txtInput.Text, out tl);
			}
			else if (validation.type == InputType.RealNum)
			{
				double td;
				ok = double.TryParse(validation.txtInput.Text, NumberStyles.AllowDecimalPoint, Podaci.DecimalnaTocka, out td);
			}

			return ok;
		}

		protected void postChange()
		{
			change = changeDelay;
		}

		protected virtual bool valid()
		{
			foreach (Validation val in validations)
				if (!check(val))
					return false;
			
			return true;
		}

		private void txtInput_TextChanged(object sender, EventArgs e)
		{
			postChange();
		}

		private void tmrDelayCheck_Tick(object sender, EventArgs e)
		{
			if (change > 0)
			{
				change -= tmrDelayCheck.Interval;
				if (change <= 0)
				{
					foreach (Validation val in validations)
						val.lblReport.Visible = !check(val);

					addoditionalChangeHandle();
				}
			}
		}
	}
}
