using System;
using Ikadn.Ikon.Types;
using Stareater.Galaxy.Builders;
using Stareater.Utils.PluginParameters;

namespace Stareater.Controllers.NewGameHelpers
{
	internal class ParameterSavingVisitor : IParameterVisitor
	{
		IkonArray data = null;
		
		public IkonArray Save(IMapBuilderPiece generator)
		{
			this.data = new IkonArray();
			data.Add(new IkonText(generator.Code));
			
			foreach(var parameter in generator.Parameters)
				parameter.Accept(this);
			
			return this.data;
		}
		
		#region IParameterVisitor implementation
		public void Visit(ContinuousRangeParameter parameter)
		{
			this.data.Add(new IkonFloat(parameter.Value));
		}
		
		public void Visit(DiscreteRangeParameter parameter)
		{
			this.data.Add(new IkonInteger(parameter.Value));
		}
		
		public void Visit(SelectorParameter parameter)
		{
			this.data.Add(new IkonInteger(parameter.Value));
		}
		#endregion
		
	}
}
