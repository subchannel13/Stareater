using OpenTK;
using Stareater.GLData;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.GraphicsEngine.GuiElements
{
	class BackgroundTexture
	{
		private static readonly Vector2[] Deltas = new Vector2[]
		{
			new Vector2(-1, -1),
			new Vector2(0, -1),
			new Vector2(0, 0),
			new Vector2(-1, 0),
		};

		public TextureInfo Sprite { get; private set; }
		public float Padding { get; private set; }
		
		private readonly Vector2[] cuts;

		public BackgroundTexture(TextureInfo sprite, int padding)
		{
			this.Sprite = sprite;
			this.Padding = padding;

			this.cuts = padding > 0 ? 
				new Vector2[]
				{
						new Vector2(-0.5f, 0),
						new Vector2(-0.5f, 1),
						new Vector2(0.5f, -1),
						new Vector2(0.5f, 0),
				} :
				new Vector2[]
				{
					new Vector2(-0.5f, 0),
					new Vector2(0.5f, 0),
				};
		}

		public IEnumerable<Vector2> SlicePolygon(float width, float height)
		{
			return makeGrid(width, height, this.Padding, this.Padding);
		}

		public IEnumerable<Vector2> SliceTexture()
		{
			var max = this.Sprite.Coordinates[2];
			var min = this.Sprite.Coordinates[0];
			var center = (max + min) / 2;
			var size = max - min;
			var altasSize = TextureUtils.TextureSize(this.Sprite.Id);

			return makeGrid(size.X, size.Y, this.Padding / altasSize.X, this.Padding / altasSize.Y).
				Select(p => p + center);
		}

		private IEnumerable<Vector2> makeGrid(float width, float height, float paddingX, float paddingY)
		{
			var widthVector = new Vector2(width, paddingX);
			var heightVector = new Vector2(height, paddingY);

			for (int y = 1; y < cuts.Length; y++)
				for (int x = 1; x < cuts.Length; x++)
					foreach(var delta in Deltas)
						yield return new Vector2(
							Vector2.Dot(cuts[x + (int)delta.X], widthVector), 
							Vector2.Dot(cuts[y + (int)delta.Y], heightVector)
						);
		}
	}
}
