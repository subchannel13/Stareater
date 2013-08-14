using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class LinearSegmentsFunction : IExpressionNode
	{
		IExpressionNode indexNode;
		IList<IExpressionNode> segmentPoints;

		public LinearSegmentsFunction(IExpressionNode index, IList<IExpressionNode> segmentPoints)
		{
			this.indexNode = index;
			this.segmentPoints = segmentPoints;
		}

		public IExpressionNode Simplified()
		{
			if (indexNode.isConstant) {
				double t = indexNode.Evaluate(null);
				int leftIndex = leftPointIndex(t);

				if (segmentPoints[leftIndex].isConstant && segmentPoints[leftIndex + 1].isConstant)
					return new Constant(this.Evaluate(null));
			}

			return this;
		}

		public bool isConstant
		{
			get { return indexNode.isConstant && segmentPoints.All(point => point.isConstant); }
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
			else if (t >= segmentPoints.Count - 1)
				return segmentPoints.Count - 2;

			return (int)Math.Floor(t);
		}
	}
}
