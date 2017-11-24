using System;
using System.Collections.Generic;
using System.Drawing;
using Ikadn.Ikon.Types;

namespace Stareater.GLData
{
	class AtlasBuilder
	{
		private readonly Dictionary<string, Rectangle> sizes;
		private readonly PartitionNode bucket;
		private readonly int margin;

		public AtlasBuilder(IkonComposite sizes, int margin, Size bucketSize)
		{
			this.bucket = new PartitionNode(bucketSize);
			this.sizes = new Dictionary<string, Rectangle>();
			this.margin = margin;

			var marginPoint = new Size(2 * margin, 2 * margin);
			foreach(var name in sizes.Keys)
			{
				var sizeData = sizes[name].To<IkonArray[]>();
				var height = (sizeData[2].To<double[]>()[1] - sizeData[0].To<double[]>()[1]) * bucketSize.Height;
				var width = (sizeData[2].To<double[]>()[0] - sizeData[0].To<double[]>()[0]) * bucketSize.Width;
				var size = new Size((int)Math.Round(width) + 2 * margin, (int)Math.Round(height) + 2 * margin);
				var itemPosition = bucket.Add(size);
				
				if (!itemPosition.HasValue)
					throw new Exception("Bucket is too small");
				this.sizes[name] = new Rectangle(itemPosition.Value, size);
			}
		}

		public Rectangle Add(Size itemSize)
		{
			itemSize = itemSize + new Size(2 * margin, 2 * margin);
			var itemPosition = bucket.Add(itemSize);
			if (itemPosition == null)
				throw new Exception("Bucket is too small");

			return new Rectangle(
				itemPosition.Value + new Size(margin, margin),
				itemSize - new Size(2 * margin, 2 * margin)
			);
		}
		
		class PartitionNode
		{
			private const int NotSet = -1;
			
			Size size;
			bool isDividerVertical;
			int dividerPosition;
			
			bool filledUp = false;
			PartitionNode leftChild;
			PartitionNode rightChild;
	
			public PartitionNode(Size size)
			{
				this.dividerPosition = NotSet;
				this.size = size;
			}
	
			public Point? Add(Size itemSize)
			{
				if (size.Width < itemSize.Width || size.Height < itemSize.Height || this.filledUp)
					return null;
	
				Point? result;
	
				if (dividerPosition != NotSet) {
					result = leftChild.Add(itemSize);
					if (result != null)
					    return result;
	
					result = rightChild.Add(itemSize);
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
	
					result = leftChild.Add(itemSize);
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
	
					result = leftChild.Add(itemSize);
					if (result == null)
						throw new Exception("Something is wrong with universe...");
	
					return result;
				}
				
				this.filledUp = true;
				return new Point();
			}
		}
	}
}
