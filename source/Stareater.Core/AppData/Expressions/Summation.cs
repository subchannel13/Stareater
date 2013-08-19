using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class Summation: IExpressionNode
	{
		IExpressionNode[] sequence;
		IExpressionNode[] inverseSequence;

		public Summation(IExpressionNode[] sequence, IExpressionNode[] inverseSequence)
		{
			this.sequence = sequence;
			this.inverseSequence = inverseSequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.isConstant) + inverseSequence.Count(x => x.isConstant);

			if (sequence.Length == 1 && inverseSequence.Length == 0)
				return sequence.First();
			else if (constCount == sequence.Length + inverseSequence.Length)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				List<IExpressionNode> newSequence = new List<IExpressionNode>();
				List<IExpressionNode> newInverseSequence = new List<IExpressionNode>();

				return new Summation(
					sequence.Where(x => !x.isConstant).Concat(new IExpressionNode[] 
					{
						new Constant(
							sequence.Where(x => x.isConstant).Aggregate(0.0, (subSum, element) => subSum + element.Evaluate(null)) -
							inverseSequence.Where(x => x.isConstant).Aggregate(0.0, (subSum, element) => subSum + element.Evaluate(null))
						)
					}).ToArray(),
					inverseSequence.Where(x => !x.isConstant).ToArray()
					);
			}
			else
				return this;
		}

		public bool isConstant
		{
			get { return sequence.All(x => x.isConstant) && inverseSequence.All(x => x.isConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Aggregate(0.0, (subSum, element) => subSum + element.Evaluate(variables)) -
				inverseSequence.Aggregate(0.0, (subSum, element) => subSum + element.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				foreach(var node in sequence)
					foreach(var variable in node.Variables)
						yield return variable;
				
				foreach(var node in inverseSequence)
					foreach(var variable in node.Variables)
						yield return variable;
			}
		}
	}
}
