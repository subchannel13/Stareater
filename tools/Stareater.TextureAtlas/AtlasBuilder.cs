using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stareater.TextureAtlas
{
	class AtlasBuilder
	{
		private KeyValuePair<string, Size>[] sizes;
		private Size bucketSize;
		private int margin;

		public AtlasBuilder(KeyValuePair<string, Size>[] sizes, int margin, Size bucketSize)
		{
			this.bucketSize = bucketSize;
			this.sizes = sizes;
			this.margin = margin;

			Size marginPoint = new Size(2 * margin, 2 * margin);
			for (int i = 0; i < sizes.Length; i++)
				this.sizes[i] = new KeyValuePair<string, Size>(this.sizes[i].Key, sizes[i].Value + marginPoint);
		}

		public IEnumerable<KeyValuePair<string, Rectangle>> Build()
		{
			PartitionNode bucket = new PartitionNode(bucketSize);

			Point? itemPosition;
			for (int i = 0; i < sizes.Length; i++) {
				itemPosition = bucket.Add(i, sizes[i].Value);
				if (itemPosition == null)
					throw new Exception("Bucket is too small");

				yield return new KeyValuePair<string, Rectangle>(
					sizes[i].Key,
					new Rectangle(
						itemPosition.Value + new Size(margin, margin),
						sizes[i].Value - new Size(2 * margin, 2 * margin)
					)
				);
			}
		}
	}
}
