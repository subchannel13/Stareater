using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class LinearSegmentsFunction : IExpressionNode
	{
		IExpressionNode indexNode;
		IExpressionNode[] segmentPoints;

		public LinearSegmentsFunction(IExpressionNode index, IExpressionNode[] segmentPoints)
		{
			this.indexNode = index;
			this.segmentPoints = segmentPoints;
		}

		public IExpressionNode Simplified()
		{
			if (indexNode.IsConstant) {
				double t = indexNode.Evaluate(null);
				int leftIndex = leftPointIndex(t);

				if (segmentPoints[leftIndex].IsConstant && segmentPoints[leftIndex + 1].IsConstant)
					return new Constant(this.Evaluate(null));
			}

			return this;
		}

		public bool IsConstant
		{
			get { return indexNode.IsConstant && segmentPoints.All(point => point.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			double t = indexNode.Evaluate(variables);
			int leftIndex = leftPointIndex(t);

			double leftPoint = segmentPoints[leftIndex].Evaluate(variables);
			double rightPoint = segmentPoints[leftIndex + 1].Evaluate(variables);

			return leftPoint + (t - leftIndex) * (rightPoint - leftPoint);
		}

		private int leftPointIndex(double t)
		{
			if (t < 0)
				return 0;
			else if (t >= segmentPoints.Length - 1)
				return segmentPoints.Length - 2;

			return (int)Math.Floor(t);
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				foreach(var variable in indexNode.Variables)
					yield return variable;
				
				foreach(var node in segmentPoints)
					foreach(var variable in node.Variables)
						yield return variable;
			}
		}
	}
}
