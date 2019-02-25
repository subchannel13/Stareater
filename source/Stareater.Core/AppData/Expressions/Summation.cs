using System.Collections.Generic;
using System.Linq;

namespace Stareater.AppData.Expressions
{
	class Summation: IExpressionNode
	{
		private readonly IExpressionNode[] sequence;
		private readonly IExpressionNode[] inverseSequence;

		public Summation(IExpressionNode[] sequence, IExpressionNode[] inverseSequence)
		{
			this.sequence = sequence;
			this.inverseSequence = inverseSequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.IsConstant) + inverseSequence.Count(x => x.IsConstant);

			if (sequence.Length == 1 && inverseSequence.Length == 0)
				return sequence.First();
			else if (constCount == sequence.Length + inverseSequence.Length)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1)
				return new Summation(
					sequence.Where(x => !x.IsConstant).Concat(new IExpressionNode[] 
					{
						new Constant(
							sequence.Where(x => x.IsConstant).Select(x => x.Evaluate(null)).Aggregate(0.0, (a, b) => a + b) -
							inverseSequence.Where(x => x.IsConstant).Select(x => x.Evaluate(null)).Aggregate(0.0, (a, b) => a + b)
						)
					}).ToArray(),
					inverseSequence.Where(x => !x.IsConstant).ToArray()
					);
			else
				return this;
		}

		public IExpressionNode Substitute(Dictionary<string, Formula> mapping)
		{
			return new Summation(
				this.sequence.Select(x => x.Substitute(mapping)).ToArray(),
				this.inverseSequence.Select(x => x.Substitute(mapping)).ToArray()
			).Simplified();
		}

		public bool IsConstant
		{
			get { return sequence.All(x => x.IsConstant) && inverseSequence.All(x => x.IsConstant); }
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
