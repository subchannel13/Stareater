using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Stareater.GLData
{
	class CharacterSdfDrawer : ICharacterDrawer
	{
		private const int Padding = 4;

		private readonly AtlasBuilder atlas;
		private readonly Font font;
		private readonly ColorMap texture;
		private readonly Bitmap fakeBitmap;
		private readonly Graphics fakeCanvas;

		public CharacterSdfDrawer(AtlasBuilder atlas, ColorMap texture, Font font)
		{
			this.atlas = atlas;
			this.font = font;
			this.texture = texture;
			this.fakeBitmap = new Bitmap(1, 1);
			this.fakeCanvas = Graphics.FromImage(this.fakeBitmap);
		}

		public void Dispose()
		{
			fakeCanvas.Dispose();
			fakeBitmap.Dispose();
		}

		public CharTextureInfo Draw(char c)
		{
			var text = c.ToString();
			var path = new GraphicsPath();
			path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), StringFormat.GenericTypographic);
			path.Flatten();

			var contures = getContures(path).ToList();
			path.Dispose();

			var measuredSize = new SizeF(
				this.fakeCanvas.MeasureString(text, this.font, int.MaxValue, StringFormat.GenericTypographic).Width + Padding * 2,
				TextRenderer.MeasureText(this.fakeCanvas, text, this.font, new Size(int.MaxValue, int.MaxValue)).Height + Padding * 2
			);
			var rect = this.atlas.Add(measuredSize);
			
			int width = rect.Size.Width;
			int height = rect.Size.Height;
			var distField = genSdf(contures, width, height);

			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
					this.texture[rect.X + x, rect.Y + y] = Color.FromArgb((int)(distField[y, x] * 255), 255, 255, 255);

			return new CharTextureInfo(
				rect,
				this.texture.Width, this.texture.Height,
				width / (float)(width - 2 * Padding), height / (float)(height - 2 * Padding),
				0.5f, -0.5f
			);
		}

		private static IEnumerable<GlyphContour> getContures(GraphicsPath path)
		{
			var pathPoints = path.PathPoints;
            var pathTypes = path.PathTypes.Select(x => x & (byte)PathPointType.PathTypeMask).ToList();
            var contoureRanges = new List<KeyValuePair<int, int>>();
            var start = 0;
            var count = 0;

            for (int i = 0; i < pathPoints.Length; i++)
            {
                if (pathTypes[i] == (byte)PathPointType.Start)
                {
                    contoureRanges.Add(new KeyValuePair<int, int>(start, count));
                    start = i;
                    count = 0;
                }

                count++;
            }
            contoureRanges.Add(new KeyValuePair<int, int>(start, count));
         
            foreach (var group in contoureRanges.Where(x => x.Value > 1))
            {
                var strokes = new List<PointF[]>();
                for (int i = 1; i < group.Value; i++)
                    strokes.Add(new[] { pathPoints[group.Key + i - 1], pathPoints[group.Key + i] });

                strokes.Add(new[] { pathPoints[group.Key + group.Value - 1], pathPoints[group.Key] });
                yield return new GlyphContour(strokes);
            }
		}

		private static double[,] genSdf(List<GlyphContour> contures, int width, int height)
		{
			var distField = new double[height, width];

			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					var fromP = new Vector2(x - Padding, y - Padding);
					var minDist = Math.Min(contures.Min(shape => shape.Distance(fromP)), Padding);
					if (contures.Sum(shape => shape.RayHits(fromP)) % 2 != 0)
						minDist *= -1;

					distField[y, x] = -minDist / Padding / 2 + 0.5;
				}

			return distField;
		}

	}
}
