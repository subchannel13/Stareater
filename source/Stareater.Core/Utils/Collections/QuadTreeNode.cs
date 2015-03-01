using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Utils.Collections
{
	class QuadTreeNode<T>
	{
		public QuadTreeNode(Vector2D topRight, Vector2D bottomLeft)
		{
			this.topRight = topRight;
			this.bottomLeft = bottomLeft;
		}

		private List<QuadTreeElement<T>> contents = new List<QuadTreeElement<T>>();
		private List<QuadTreeNode<T>> nodes = new List<QuadTreeNode<T>>(4);

		private Vector2D topRight;
		private Vector2D bottomLeft;

		public bool IsEmpty
		{
			get { return contents.Count == 0 && nodes.Count == 0; }
		}

		public IEnumerable<QuadTreeElement<T>> SubTreeContents
		{
			get
			{
				foreach (var node in nodes)
					foreach (var item in node.SubTreeContents)
						yield return item;

				foreach (var item in this.contents)
					yield return item;
			}
		}

		public IEnumerable<T> Query(Vector2D queryTopRight, Vector2D queryBottomLeft)
		{
			foreach (var item in this.contents)
				if (!Methods.IsRectOutside(queryTopRight, queryBottomLeft, item.TopRight, item.BottomLeft))
					yield return item.Data;

			foreach (QuadTreeNode<T> node in nodes) {
				if (node.IsEmpty)
					continue;

				if (Methods.IsRectEnveloped(node.topRight, node.bottomLeft, queryTopRight, queryBottomLeft)) {
					foreach (var item in node.Query(queryTopRight, queryBottomLeft))
						yield return item;
					break;
				}

				if (Methods.IsRectEnveloped(queryTopRight, queryBottomLeft, node.topRight, node.bottomLeft))
				    foreach (var item in node.SubTreeContents)
						yield return item.Data;
				else if (!Methods.IsRectOutside(node.topRight, node.bottomLeft, queryTopRight, queryBottomLeft))
					foreach (var item in node.Query(queryTopRight, queryBottomLeft))
						yield return item;
			}
		}

		public bool Insert(QuadTreeElement<T> item, float minSize)
		{
			if (!Methods.IsRectEnveloped(topRight, bottomLeft, item.TopRight, item.BottomLeft))
			    return false;
			
			if (nodes.Count == 0)
				CreateSubNodes(minSize);

			foreach (QuadTreeNode<T> node in nodes)
				if (Methods.IsRectEnveloped(node.topRight, node.bottomLeft, item.TopRight, item.BottomLeft))
					if (node.Insert(item, minSize)) {
						return true;
					}
					else
						return false;

			this.contents.Add(item);
			return true;
		}
		
		private void CreateSubNodes(float minSize)
		{
			var halfSize = new Vector2D((topRight.X - bottomLeft.X) / 2f, (topRight.Y - bottomLeft.Y) / 2f);
			
			if (halfSize.X * 2 < minSize || halfSize.Y * 2 < minSize)
				return;

			var left = new Vector2D(-halfSize.X, 0);
			var down = new Vector2D(0, -halfSize.Y);
				
			nodes.Add(new QuadTreeNode<T>(topRight, topRight - halfSize));
			nodes.Add(new QuadTreeNode<T>(topRight + left, topRight - halfSize + left));
			nodes.Add(new QuadTreeNode<T>(topRight + down, topRight - halfSize + down));
			nodes.Add(new QuadTreeNode<T>(topRight - halfSize, bottomLeft));
		}

		public bool Remove(QuadTreeElement<T> item)
		{
			if (!Methods.IsRectEnveloped(this.topRight, this.bottomLeft, item.TopRight, item.BottomLeft))
				return false;

			if (this.contents.Remove(item))
				return true;

			foreach (QuadTreeNode<T> node in nodes)
				if (Methods.IsRectEnveloped(node.topRight, node.bottomLeft, item.TopRight, item.BottomLeft))
					if (node.Remove(item))
						return true;

			return false;
		}

		public void Clear()
		{
			this.contents.Clear();
			this.nodes.Clear();
		}
	}
}
