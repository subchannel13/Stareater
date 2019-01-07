using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Stareater.GraphicsEngine.GuiElements;

namespace Stareater.GLData.SpriteShader
{
	//TODO(v0.8) remove VertexData prefixes
	static class SpriteHelpers
	{
		public static IEnumerable<Vector2> PathRectVertexData(Vector2 fromPosition, Vector2 toPosition, float width, TextureInfo textureinfo)
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

		public static IEnumerable<Vector2> TexturedRectVertexData(Vector2 center, float width, float height, TextureInfo textureinfo)
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
		
		public static IEnumerable<float> TexturedVertexData(float x, float y, float tx, float ty)
		{
			yield return x; 
			yield return y;
			yield return tx;
			yield return ty;
		}
		
		public static IEnumerable<float> UnitRectVertexData(TextureInfo textureinfo)
		{
			return TexturedRectVertexData(new Vector2(0, 0), 1, 1, textureinfo).
				SelectMany(v => new[] { v.X, v.Y });
		}

		public static IEnumerable<float> GuiBackgroundVertexData(BackgroundTexture texture, float width, float height)
		{
			var innerWidth = width - texture.PaddingLeft - texture.PaddingRight;
			var innerHeight = height - texture.PaddingTop - texture.PaddingBottom;

			return TexturedRectVertexData(new Vector2(-width / 2 + texture.PaddingLeft / 2, 0), texture.PaddingLeft, height, texture.LeftTexture).
				Concat(TexturedRectVertexData(new Vector2(width / 2 - texture.PaddingRight / 2, 0), texture.PaddingRight, height, texture.RightTexture)).
				Concat(TexturedRectVertexData(new Vector2(0, height / 2 - texture.PaddingTop / 2), innerWidth, texture.PaddingTop, texture.TopTexture)).
				Concat(TexturedRectVertexData(new Vector2(0, -height / 2 + texture.PaddingTop / 2), innerWidth, texture.PaddingBottom, texture.BottomTexture)).
				Concat(TexturedRectVertexData(new Vector2(0, 0), innerWidth, innerHeight, texture.CenterTexture)).
				SelectMany(v => new[] { v.X, v.Y });
		}
	}
}
