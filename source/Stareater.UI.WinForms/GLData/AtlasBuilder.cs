using System;
using System.Drawing;
using Ikadn.Ikon.Types;

namespace Stareater.GLData
{
	class AtlasBuilder
	{
		private readonly PartitionNode bucket;
		private readonly int margin;

		public AtlasBuilder(int margin, Size bucketSize)
		{
			this.bucket = new PartitionNode(bucketSize);
			this.margin = margin;
		}

		public AtlasBuilder(IkonComposite sizes, int margin, Size bucketSize) : this(margin, bucketSize)
		{
			var marginPoint = new Size(2 * margin, 2 * margin);
			foreach(var name in sizes.Keys)
			{
				var sizeData = sizes[name].To<IkonArray[]>();
				var height = (sizeData[2].To<double[]>()[1] - sizeData[0].To<double[]>()[1]) * bucketSize.Height;
				var width = (sizeData[2].To<double[]>()[0] - sizeData[0].To<double[]>()[0]) * bucketSize.Width;
				var size = new Size((int)Math.Round(width) + 2 * margin, (int)Math.Round(height) + 2 * margin);
				var itemPosition = this.bucket.Add(size);
				
				if (!itemPosition.HasValue)
					throw new Exception("Bucket is too small");
			}
		}

		public Rectangle Add(SizeF itemSize)
		{
			return this.Add(new Size((int)Math.Ceiling(itemSize.Width), (int)Math.Ceiling(itemSize.Height)));
		}

		public Rectangle Add(Size itemSize)
		{
			itemSize = itemSize + new Size(2 * this.margin, 2 * this.margin);
			var itemPosition = this.bucket.Add(itemSize);
			if (itemPosition == null)
				throw new Exception("Bucket is too small");

			return new Rectangle(
				itemPosition.Value + new Size(this.margin, this.margin),
				itemSize - new Size(2 * this.margin, 2 * this.margin)
			);
		}
		
		class PartitionNode
		{
			private const int NotSet = -1;

			private readonly Size size;
			private bool isDividerVertical;
			private int dividerPosition;

			private bool filledUp = false;
			private PartitionNode leftChild;
			private PartitionNode rightChild;
	
			public PartitionNode(Size size)
			{
				this.dividerPosition = NotSet;
				this.size = size;
			}
	
			public Point? Add(Size itemSize)
			{
				if (this.size.Width < itemSize.Width || this.size.Height < itemSize.Height || this.filledUp)
					return null;
	
				if (this.dividerPosition != NotSet) {
					var result = this.leftChild.Add(itemSize);
					if (result != null)
					    return result;
	
					result = this.rightChild.Add(itemSize);
					if (result != null)
						if (this.isDividerVertical)
							return result.Value + new Size(this.dividerPosition, 0);
						else
							return result.Value + new Size(0, this.dividerPosition);
					else
						return null;
				}
				else if (this.size.Height > itemSize.Height)
				{
					this.isDividerVertical = false;
					this.dividerPosition = itemSize.Height;

					this.leftChild = new PartitionNode(new Size(this.size.Width, itemSize.Height));
					this.rightChild = new PartitionNode(new Size(this.size.Width, this.size.Height - itemSize.Height));

					var result = this.leftChild.Add(itemSize);
					if (result == null)
						throw new Exception("Something is wrong with universe...");

					return result;
				}
				else if (this.size.Width > itemSize.Width)
				{
					this.isDividerVertical = true;
					this.dividerPosition = itemSize.Width;

					this.leftChild = new PartitionNode(new Size(itemSize.Width, this.size.Height));
					this.rightChild = new PartitionNode(new Size(this.size.Width - itemSize.Width, this.size.Height));
	
					var result = this.leftChild.Add(itemSize);
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
