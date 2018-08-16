using System.Collections;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	public class QuadTree<T> : ICollection<QuadTreeElement<T>>
	{
		private const float InitialSize = 4;
		private const float MinSize = 1.0f / 8;

		private QuadTreeNode<T> root;
		private readonly Dictionary<T, QuadTreeElement<T>> boundedElements = new Dictionary<T, QuadTreeElement<T>>();

		private Vector2D topRight;
		private Vector2D bottomLeft;

		private float minNodeSize { get; set; }

		public QuadTree()
			: this(
				new Vector2D(0, 0),
				new Vector2D(InitialSize, InitialSize),
				MinSize)
		{ }

		public QuadTree(Vector2D center, Vector2D size, float minSize)
		{
			this.topRight = center + size / 2;
			this.bottomLeft = center - size / 2;
			this.minNodeSize = minSize;
			this.root = new QuadTreeNode<T>(topRight, bottomLeft);
		}

		public IEnumerable<T> Query(Vector2D center, Vector2D size)
		{
			return root.Query(center + size / 2, center - size / 2);
		}

		public void Add(T newItem, double centerX, double centerY, double sizeX, double sizeY)
		{
			this.Add(new QuadTreeElement<T>(
				newItem, 
				new Vector2D(centerX + sizeX / 2, centerY + sizeY / 2),
				new Vector2D(centerX - sizeX / 2, centerY - sizeY / 2)
			));
		}
		
		public void Add(T newItem, Vector2D center, Vector2D size)
		{
			this.Add(new QuadTreeElement<T>(newItem, center + size / 2, center - size / 2));
		}

		public void Add(QuadTreeElement<T> item)
		{
			boundedElements.Add(item.Data, item);

			if (!root.Insert(item, this.minNodeSize))
			{
				var halfSize = (topRight - bottomLeft) / 2;

				while (!Methods.IsRectEnveloped(this.topRight, this.bottomLeft, item.TopRight, item.BottomLeft))
				{
					this.topRight += halfSize;
					this.bottomLeft -= halfSize;
					halfSize *= 2;
				}

				var oldRoot = this.root;
				this.root = new QuadTreeNode<T>(topRight, bottomLeft);
				this.root.Insert(item, this.minNodeSize);

				foreach (var oldItem in oldRoot.SubTreeContents)
					root.Insert(oldItem, this.minNodeSize);
			}
		}

		public void Clear()
		{
			root.Clear();
			boundedElements.Clear();
		}

		public bool Contains(QuadTreeElement<T> item)
		{
			return this.boundedElements.ContainsKey(item.Data) &&
				item.Equals(this.boundedElements[item.Data]);
		}

		public int Count
		{
			get { return this.boundedElements.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			if (!boundedElements.ContainsKey(item))
				return false;

			var boundedItem = boundedElements[item];
			boundedElements.Remove(item);
			return root.Remove(boundedItem);
		}

		public bool Remove(QuadTreeElement<T> item)
		{
			if (!boundedElements.ContainsKey(item.Data))
				return false;

			boundedElements.Remove(item.Data);
			return root.Remove(item);
		}

		public void CopyTo(QuadTreeElement<T>[] array, int arrayIndex)
		{
			boundedElements.Values.CopyTo(array, arrayIndex);
		}

		public IEnumerable<T> GetAll()
		{
			foreach (var element in boundedElements.Keys)
				yield return element;
		}

		public IEnumerator<QuadTreeElement<T>> GetEnumerator()
		{
			return boundedElements.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return boundedElements.Values.GetEnumerator();
		}
	}
}
