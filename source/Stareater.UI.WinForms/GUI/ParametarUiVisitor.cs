using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Stareater.Utils.PluginParameters;

namespace Stareater.GUI
{
	class ParametarUiVisitor : IParameterVisitor
	{
		private IEnumerable<AParameterBase> parameters;
		private Action changeListener;
		private Control result = null;
		
		public ParametarUiVisitor(IEnumerable<AParameterBase> parameters, Action changeListener)
		{
			this.changeListener = changeListener;
			this.parameters = parameters;
		}
		
		public IEnumerable<Control> MakeUi()
		{
			foreach(var parameter in this.parameters)
			{
				this.result = null;
				parameter.Accept(this);
				yield return this.result;
			}
		}
		
		#region IParameterVisitor implementation
		public void Visit(ContinuousRangeParameter parameter)
		{
			var parameterControl = new MapParameterRealRange();
			parameterControl.SetData(parameter, this.changeListener);
			this.result = parameterControl;
		}
		
		public void Visit(DiscreteRangeParameter parameter)
		{
			var parameterControl = new MapParameterIntegerRange();
			parameterControl.SetData(parameter, this.changeListener);
			this.result = parameterControl;
		}
		
		public void Visit(SelectorParameter parameter)
		{
			var parameterControl = new MapParameterSelector();
			parameterControl.SetData(parameter, this.changeListener);
			this.result = parameterControl;
		}
		#endregion
	}
}
