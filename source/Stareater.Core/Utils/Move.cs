namespace Stareater.Utils
{
	public class Move<T>
	{
		public T FromNode { get; private set; }
		public T ToNode { get; private set; }
		public double Cost{ get; private set; }

		public Move(T fromNode, T toNode, double cost)
		{
			this.FromNode = fromNode;
			this.ToNode = toNode;
			this.Cost = cost;
		}
	}
}
