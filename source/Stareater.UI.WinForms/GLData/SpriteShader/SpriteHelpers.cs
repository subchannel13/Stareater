using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Stareater.GraphicsEngine.GuiElements;

namespace Stareater.GLData.SpriteShader
{
	static class SpriteHelpers
	{
		public static IEnumerable<Vector2> PathRect(Vector2 fromPosition, Vector2 toPosition, float width, TextureInfo textureinfo)
		{
			var center = (fromPosition + toPosition) / 2;
			var length = toPosition - fromPosition;
			var direction = new Vector2(length.X, length.Y);
			direction.Normalize();
			var widthDir = new Vector2(-direction.Y, direction.X) * width;

			yield return center - length / 2 + widthDir / 2;
			yield return textureinfo.Coordinates[3];

			yield return center + length / 2 + widthDir / 2;
			yield return textureinfo.Coordinates[2];

			yield return center + length / 2 - widthDir / 2;
			yield return textureinfo.Coordinates[1];


			yield return center + length / 2 - widthDir / 2;
			yield return textureinfo.Coordinates[1];

			yield return center - length / 2 - widthDir / 2;
			yield return textureinfo.Coordinates[0];

			yield return center - length / 2 + widthDir / 2;
			yield return textureinfo.Coordinates[3];
		}

		public static IEnumerable<Vector2> TexturedRect(Vector2 center, float width, float height, TextureInfo textureinfo)
		{
			var widthDir = new Vector2(width, 0);
			var heightDir = new Vector2(0, height);
			
			yield return center - widthDir / 2 + heightDir /2;
			yield return textureinfo.Coordinates[3];
			
			yield return center + widthDir / 2 + heightDir /2;
			yield return textureinfo.Coordinates[2];
		
			yield return center + widthDir / 2 - heightDir /2;
			yield return textureinfo.Coordinates[1];
		
			
			yield return center + widthDir / 2 - heightDir /2;
			yield return textureinfo.Coordinates[1];
		
			yield return center - widthDir / 2 - heightDir /2;
			yield return textureinfo.Coordinates[0];
		
			yield return center - widthDir / 2 + heightDir /2;
			yield return textureinfo.Coordinates[3];
		}
		
		public static IEnumerable<float> TexturedVertex(float x, float y, float tx, float ty)
		{
			yield return x; 
			yield return y;
			yield return tx;
			yield return ty;
		}
		
		public static IEnumerable<float> UnitRect(TextureInfo textureinfo)
		{
			return TexturedRect(new Vector2(0, 0), 1, 1, textureinfo).
				SelectMany(v => new[] { v.X, v.Y });
		}

		public static IEnumerable<float> GuiBackground(BackgroundTexture texture, float width, float height)
		{
			var polygonPoints = texture.SlicePolygon(width, height).ToList();
			var texturePoints = texture.SliceTexture().ToList();
			var points = new List<Vector2>();
			var spriteTextureSize = TextureUtils.TextureSize(texture.Sprite.Id);

			for (int i = 0; i < polygonPoints.Count; i += 4)
			{
				var textureMin = texturePoints[i + 0];
				var textureMax = texturePoints[i + 2];
				var textureSize = textureMax - textureMin;
				var polygonSize = polygonPoints[i + 2] - polygonPoints[i + 0];

				var textureCenter = (textureMax + textureMin) / 2;
				var widthDir = new Vector2(Math.Min(polygonSize.X, textureSize.X * spriteTextureSize.X) / spriteTextureSize.X, 0);
				var heightDir = new Vector2(0, Math.Min(polygonSize.Y, textureSize.Y * spriteTextureSize.Y) / spriteTextureSize.Y);

				points.Add(polygonPoints[i + 3]);
				points.Add(textureCenter - widthDir / 2 + heightDir / 2);

				points.Add(polygonPoints[i + 2]);
				points.Add(textureCenter + widthDir / 2 + heightDir / 2);

				points.Add(polygonPoints[i + 1]);
				points.Add(textureCenter + widthDir / 2 - heightDir / 2);


				points.Add(polygonPoints[i + 1]);
				points.Add(textureCenter + widthDir / 2 - heightDir / 2);

				points.Add(polygonPoints[i + 0]);
				points.Add(textureCenter - widthDir / 2 - heightDir / 2);

				points.Add(polygonPoints[i + 3]);
				points.Add(textureCenter - widthDir / 2 + heightDir / 2);
			}

			return points.SelectMany(v => new[] { v.X, v.Y });
		}
	}
}
