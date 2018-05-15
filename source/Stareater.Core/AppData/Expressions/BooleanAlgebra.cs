using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class ConjunctionSequence : IExpressionNode
	{
		IExpressionNode[] sequence;

		public ConjunctionSequence(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.IsConstant);

			if (constCount == sequence.Length)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				var grouping = sequence.GroupBy(x => x.IsConstant).ToDictionary(x => x.Key);

				if (grouping[true].Any(x => x.Evaluate(null) < 0))
					return new Constant(-1);

				return new ConjunctionSequence(grouping[false].ToArray());
			}
			else
				return this;
		}

		public bool IsConstant
		{
			get { return sequence.All(x => x.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.All(x => x.Evaluate(variables) >= 0) ? 1 : -1;
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				foreach(var node in sequence)
					foreach(var variable in node.Variables)
						yield return variable;
			}
		}
	}

	class DisjunctionSequence : IExpressionNode
	{
		IExpressionNode[] sequence;

		public DisjunctionSequence(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.IsConstant);

			if (constCount == sequence.Length)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				var grouping = sequence.GroupBy(x => x.IsConstant).ToDictionary(x => x.Key);

				if (grouping[true].Any(x => x.Evaluate(null) >= 0))
					return new Constant(1);

				return new DisjunctionSequence(grouping[false].ToArray());
			}
			else
				return this;
		}

		public bool IsConstant
		{
			get { return sequence.All(x => x.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Any(x => x.Evaluate(variables) >= 0) ? 1 : -1;
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				foreach(var node in sequence)
					foreach(var variable in node.Variables)
						yield return variable;
			}
		}
	}

	class XorSequence : IExpressionNode
	{
		IExpressionNode[] sequence;

		public XorSequence(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.IsConstant);

			if (constCount == sequence.Length)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				List<IExpressionNode> newSequence = new List<IExpressionNode>();
				
				var grouping = sequence.GroupBy(x => x.IsConstant).ToDictionary(x => x.Key);
				int truths = grouping[true].Count(x => x.Evaluate(null) >= 0);
				
				if (truths > 1)
					return new Constant(-1);
				if (truths == 1)
					newSequence.Add(new Constant(1));
				
				newSequence.AddRange(grouping[false]);

				return new XorSequence(newSequence.ToArray());
			}
			else
				return this;
		}

		public bool IsConstant
		{
			get { return sequence.All(x => x.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Aggregate(-1.0, (last, current) => (ToBoolean.Normalize(current.Evaluate(variables)) != last) ? 1 : -1);
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				foreach(var node in sequence)
					foreach(var variable in node.Variables)
						yield return variable;
			}
		}
	}
}
