using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Prototip;
using Zvjezdojedac_editori.Validation;

namespace Zvjezdojedac_editori
{
	public partial class ValidatorForm : Form
	{
		private HashSet<AValid> validations = new HashSet<AValid>();
		private HashSet<Control> changedControles = new HashSet<Control>();
		private int change = 0;
		protected int changeDelay { get; set; }

		public ValidatorForm()
		{
			InitializeComponent();

			changeDelay = 300;
		}

		/// <summary>
		/// Adds validation constraint.
		/// </summary>
		/// <param name="validation">Constraint</param>
		protected void addValidation(AValid validation)
		{
			validations.Add(validation);
			validation.setChangeAlerter(postChange);
		}

		/// <summary>
		/// Signals that change occured and that validation 
		/// GUI might need refreshing.
		/// </summary>
		protected void postChange(Control controlChanged)
		{
			change = changeDelay;
			changedControles.Add(controlChanged);
		}

		/// <summary>
		/// Checks validity of form.
		/// </summary>
		/// <returns>True if all constrains are satisfied.</returns>
		protected virtual bool valid()
		{
			foreach (AValid val in validations)
				if (!val.valid())
					return false;
			
			return true;
		}

		protected virtual void changeOccured(HashSet<Control> changedControles)
		{
		}

		private void tmrDelayCheck_Tick(object sender, EventArgs e)
		{
			if (change > 0)
			{
				change -= tmrDelayCheck.Interval;
				if (change <= 0)
				{
					foreach (AValid val in validations)
						val.setWarning();
					
					changeOccured(changedControles);
					changedControles.Clear();
				}
			}
		}
	}
}
