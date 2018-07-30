namespace Stareater.Utils
{
	public class Vertex<T>
	{
		public T Data { get; private set; }

		internal Vertex(T data)
		{
			this.Data = data;
		}
	}
}
