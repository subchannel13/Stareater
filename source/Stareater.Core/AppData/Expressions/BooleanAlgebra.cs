using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class ConjunctionSequence : IExpressionNode
	{
		ICollection<IExpressionNode> sequence;

		public ConjunctionSequence(ICollection<IExpressionNode> sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.isConstant);

			if (sequence.Count == 1)
				return sequence.First();
			else if (constCount == sequence.Count)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				var grouping = sequence.GroupBy(x => x.isConstant).ToDictionary(x => x.Key);

				if (grouping[true].Any(x => x.Evaluate(null) < 0))
					return new Constant(-1);

				return new ConjunctionSequence(grouping[false].ToArray());
			}
			else
				return this;
		}

		public bool isConstant
		{
			get { return sequence.All(x => x.isConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.All(x => x.Evaluate(null) >= 0) ? 1 : -1;
		}
	}

	class DisjunctionSequence : IExpressionNode
	{
		ICollection<IExpressionNode> sequence;

		public DisjunctionSequence(ICollection<IExpressionNode> sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.isConstant);

			if (sequence.Count == 1)
				return sequence.First();
			else if (constCount == sequence.Count)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				var grouping = sequence.GroupBy(x => x.isConstant).ToDictionary(x => x.Key);

				if (grouping[true].Any(x => x.Evaluate(null) >= 0))
					return new Constant(1);

				return new DisjunctionSequence(grouping[false].ToArray());
			}
			else
				return this;
		}

		public bool isConstant
		{
			get { return sequence.All(x => x.isConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Any(x => x.Evaluate(null) >= 0) ? 1 : -1;
		}
	}

	class XorSequence : IExpressionNode
	{
		ICollection<IExpressionNode> sequence;

		public XorSequence(ICollection<IExpressionNode> sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.isConstant);

			if (sequence.Count == 1)
				return sequence.First();
			else if (constCount == sequence.Count)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				var grouping = sequence.GroupBy(x => x.isConstant).ToDictionary(x => x.Key);
				List<IExpressionNode> newSequence = new List<IExpressionNode>();
		
				newSequence.Add(new Constant((grouping[true].Count() - grouping[true].Count(x => x.Evaluate(null) >= 0) % 2 != 0) ? 1 : -1));
				newSequence.AddRange(grouping[false]);

				return new XorSequence(newSequence.ToArray());
			}
			else
				return this;
		}

		public bool isConstant
		{
			get { return sequence.All(x => x.isConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			int truths = sequence.Count(x => x.Evaluate(null) >= 0);
			return ((sequence.Count - truths) % 2 != 0) ? 1 : -1;
		}
	}
}
