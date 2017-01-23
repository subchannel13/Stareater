using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Utils.Collections
{
	public class QuadTree<T>
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
			this.Add(newItem, new Vector2D(centerX, centerY), new Vector2D(sizeX, sizeY));
		}
		
		public void Add(T newItem, Vector2D center, Vector2D size)
		{
			var boundedItem = new QuadTreeElement<T>(newItem, center + size / 2, center - size / 2);
			boundedElements.Add(newItem, boundedItem);

			if (!root.Insert(boundedItem, this.minNodeSize))
			{
				var halfSize = (topRight - bottomLeft) / 2;

				while (!Methods.IsRectEnveloped(this.topRight, this.bottomLeft, boundedItem.TopRight, boundedItem.BottomLeft))
				{
					this.topRight += halfSize;
					this.bottomLeft -= halfSize;
					halfSize *= 2;
				}

				var oldRoot = this.root;
				this.root = new QuadTreeNode<T>(topRight, bottomLeft);
				this.root.Insert(boundedItem, this.minNodeSize);

				foreach (var oldItem in oldRoot.SubTreeContents)
					root.Insert(oldItem, this.minNodeSize);
			}
		}

		public void Clear()
		{
			root.Clear();
			boundedElements.Clear();
		}

		public bool Remove(T item)
		{
			if (!boundedElements.ContainsKey(item))
				return false;

			var boundedItem = boundedElements[item];
			boundedElements.Remove(item);
			return root.Remove(boundedItem);
		}

		public IEnumerable<T> GetAll()
		{
			foreach (var element in boundedElements.Keys)
				yield return element;
		}
	}
}
