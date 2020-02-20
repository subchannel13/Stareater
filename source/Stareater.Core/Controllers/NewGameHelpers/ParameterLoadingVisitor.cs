using System;
using System.Collections.Generic;
using System.Linq;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Galaxy.Builders;
using Stareater.Utils.PluginParameters;

namespace Stareater.Controllers.NewGameHelpers
{
	internal class ParameterLoadingVisitor : IParameterVisitor
	{
		private readonly Queue<IkadnBaseObject> data;
		
		public ParameterLoadingVisitor(Queue<IkadnBaseObject> data)
		{
			this.data = data;
		}
		
		public static T Load<T>(IkonArray data, IEnumerable<T> generators) where T : IMapBuilderPiece
		{
			if (data == null)
				return generators.First();
			
			try
			{
				var dataQueue = new Queue<IkadnBaseObject>(data as IEnumerable<IkadnBaseObject>);
				var generatorCode = dataQueue.Dequeue().To<string>();
				var generator = generators.FirstOrDefault(x => x.Code == generatorCode);
				var loader = new ParameterLoadingVisitor(dataQueue);
				
				foreach(var parameter in generator.Parameters)
					parameter.Accept(loader);
				
				return generator;
			}
			catch(ArgumentNullException)
			{
				return generators.First();
			}
		}
		
		#region IParameterVisitor implementation
		public void Visit(ContinuousRangeParameter parameter)
		{
			parameter.Value = this.data.Dequeue().To<double>();
		}
		
		public void Visit(DiscreteRangeParameter parameter)
		{
			parameter.Value = this.data.Dequeue().To<int>();
		}
		
		public void Visit(SelectorParameter parameter)
		{
			parameter.Value = this.data.Dequeue().To<int>();
		}
		#endregion
	}
}
