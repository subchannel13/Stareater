using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Stareater.GLData
{
	static class TextureUtils
	{
		private static Dictionary<int, Vector2> textureSizes = new Dictionary<int, Vector2>();

		public static int CreateTexture(Bitmap image)
		{
			int textureId = GL.GenTexture();
			UpdateTexture(textureId, image);

			return textureId;
		}
		
		public static void DeleteTexture(int textureId)
		{
			GL.DeleteTexture(textureId);
			textureSizes.Remove(textureId);
		}

		public static void UpdateTexture(int textureId, Bitmap image)
		{
			var textureData =
				image.LockBits(
					new Rectangle(0, 0, image.Width, image.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					image.PixelFormat
				);

			GL.BindTexture(TextureTarget.Texture2D, textureId);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
				0, PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			image.UnlockBits(textureData);
			textureSizes[textureId] = new Vector2(image.Width, image.Height);
		}

		public static Vector2 TextureSize(int textureId)
		{
			return textureSizes[textureId];
		}
	}
}
