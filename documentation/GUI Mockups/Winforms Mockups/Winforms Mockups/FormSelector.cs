
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Winforms_Mockups
{
	/// <summary>
	/// Description of FormSelector.
	/// </summary>
	public partial class FormSelector : Form
	{
		public FormSelector()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void FormSelectorLoad(object sender, EventArgs e)
		{
			Type targetType = typeof(Form);
			foreach (var type in Assembly.GetEntryAssembly().GetTypes())
				if (targetType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface && targetType != type)
					formList.Items.Add(type);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void showForm(Type formType)
		{
			using (Form form = (Form)Activator.CreateInstance(formType)) 
			{
				form.ShowDialog();
			}
		}
		
		void FormListSelectedIndexChanged(object sender, EventArgs e)
		{
			if (formList.SelectedItem == null)
				return;
			
			Type formType = (Type)formList.SelectedItem;
			Settings.Default.LastForm = formType.FullName;
			Settings.Default.Save();
			showForm(formType);
		}
		
		void DelayedRunTick(object sender, EventArgs e)
		{
			delayedRun.Stop();
			
			if (!string.IsNullOrWhiteSpace(Settings.Default.LastForm))
				showForm(Assembly.GetEntryAssembly().GetType(Settings.Default.LastForm));
		}
	}
}
