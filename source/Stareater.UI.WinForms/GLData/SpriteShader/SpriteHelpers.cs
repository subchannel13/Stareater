using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Stareater.GLData.SpriteShader
{
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
	}
}
