using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace Zvjezdojedac_editori.Validation
{
	/// <summary>
	/// Provides methods for validating.
	/// </summary>
	public abstract class AValid
	{
		public delegate void ChangeAlerter(Control controlChanged);

		protected ChangeAlerter changeAlerter = null;

		/// <summary>
		/// Checks for validity.
		/// </summary>
		/// <returns>True if valid, false otherwise.</returns>
		public abstract bool valid();

		/// <summary>
		/// Set warning sign if invalid, remove if valid.
		/// </summary>
		public abstract void setWarning();

		public void setChangeAlerter(ChangeAlerter changeAlerter)
		{
			this.changeAlerter = changeAlerter;
		}
	}
}
