using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stareater.TextureAtlas
{
	public class PartitionNode
	{
		private const int NotSet = -1;
		
		Size size;
		bool isDividerVertical;
		int dividerPosition;
		
		int itemIndex;
		PartitionNode leftChild;
		PartitionNode rightChild;

		public PartitionNode(Size size)
		{
			this.dividerPosition = NotSet;
			this.itemIndex = NotSet;
			this.size = size;
		}

		public Point? Add(int itemIndex, Size itemSize)
		{
			if (size.Width < itemSize.Width || size.Height < itemSize.Height || this.itemIndex != NotSet)
				return null;

			Point? result;

			if (dividerPosition != NotSet) {
				result = leftChild.Add(itemIndex, itemSize);
				if (result != null)
				    return result;

				result = rightChild.Add(itemIndex, itemSize);
				if (result != null)
					if (isDividerVertical)
						return result.Value + new Size(dividerPosition, 0);
					else
						return result.Value + new Size(0, dividerPosition);
				else
					return null;
			}
			else if (size.Width > itemSize.Width)
			{
				isDividerVertical = true;
				dividerPosition = itemSize.Width;

				leftChild = new PartitionNode(new Size(itemSize.Width, this.size.Height));
				rightChild = new PartitionNode(new Size(this.size.Width - itemSize.Width, this.size.Height));

				result = leftChild.Add(itemIndex, itemSize);
				if (result == null)
					throw new Exception("Something is wrong with universe...");
				
				return result;
			}
			else if (size.Height > itemSize.Height)
			{
				isDividerVertical = false;
				dividerPosition = itemSize.Height;

				leftChild = new PartitionNode(new Size(this.size.Width, itemSize.Height));
				rightChild = new PartitionNode(new Size(this.size.Width, this.size.Height - itemSize.Height));

				result = leftChild.Add(itemIndex, itemSize);
				if (result == null)
					throw new Exception("Something is wrong with universe...");

				return result;
			}
			
			this.itemIndex = itemIndex;
			return new Point();
		}
	}
}
