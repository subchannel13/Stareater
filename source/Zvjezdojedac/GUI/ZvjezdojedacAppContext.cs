using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Zvjezdojedac.GUI
{
	class ZvjezdojedacAppContext : ApplicationContext
	{
		private static ZvjezdojedacAppContext instance = null;
		public static ZvjezdojedacAppContext GetInstance
		{
			get {
				if (instance == null)
					instance = new ZvjezdojedacAppContext();

				return instance;
			}
		}

		private Stack<Form> formStack = new Stack<Form>();

		private ZvjezdojedacAppContext()
			:base()
		{ }


		public void PushNextForm(Form form)
		{
			formStack.Push(MainForm);
			//formStack.Push(form);
			MainForm.Hide();

			MainForm = form;
			MainForm.Show();
		}

		protected override void OnMainFormClosed(object sender, EventArgs e)
		{
			if (formStack.Count > 0) {
				MainForm = formStack.Pop();
				MainForm.Show();
			}
			else
				base.OnMainFormClosed(sender, e);
		}
	}
}
