namespace Stareater.Utils
{
	public class Move<T>
	{
		public T FromNode { get; private set; }
		public T ToNode { get; private set; }

		public Move(T fromNode, T toNode)
		{
			this.FromNode = fromNode;
			this.ToNode = toNode;
		}
	}
}
