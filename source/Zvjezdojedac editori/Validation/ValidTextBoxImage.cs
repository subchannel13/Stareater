using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Zvjezdojedac;

namespace Zvjezdojedac_editori.Validation
{
	public class ValidTextBoxImage : AValidTextBox
	{
		protected Size size;

		private Image tmpImage = null;

		public ValidTextBoxImage(TextBox txtInput, Label lblReport,
			Size size)
			: base(txtInput, lblReport)
		{
			this.size = size;
		}

		public override bool valid()
		{
			try
			{
				tmpImage = Image.FromFile(txtInput.Text.Trim());
			}
			catch
			{
				return false;
			}

			if (tmpImage.Size != size)
				return false;

			return true;
		}
	}
}
