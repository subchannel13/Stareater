using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace Stareater.GLData
{
	static class TextureUtils
	{
		private static readonly Dictionary<int, Vector2> textureSizes = new Dictionary<int, Vector2>();

		public static int CreateTexture(ColorMap image)
		{
			int textureId = GL.GenTexture();

			var maxLevel = (int)Math.Round(Math.Log(Math.Min(image.Width, image.Height), 2));

			ShaderLibrary.PrintGlErrors("CreateTexture generate ID");
			GL.BindTexture(TextureTarget.Texture2D, textureId);
			ShaderLibrary.PrintGlErrors("CreateTexture bind");
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, maxLevel);
			ShaderLibrary.PrintGlErrors("CreateTexture parameters");

			GL.TexImage2D(
				TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
				0, PixelFormat.Bgra, PixelType.UnsignedByte, image.RawPixels()
			);
			ShaderLibrary.PrintGlErrors("CreateTexture put data");

			int level = 1;
			foreach (var mipmap in makeMipmaps(image))
			{
				GL.TexImage2D(
					TextureTarget.Texture2D, level, PixelInternalFormat.Rgba, mipmap.Width, mipmap.Height,
					0, PixelFormat.Bgra, PixelType.UnsignedByte, mipmap.RawPixels()
				);
				ShaderLibrary.PrintGlErrors("CreateTexture put mipmap data level " + level);
				level++;
			}

			textureSizes[textureId] = new Vector2(image.Width, image.Height);
			
			return textureId;
		}

		public static void DeleteTexture(int textureId)
		{
			GL.DeleteTexture(textureId);
			ShaderLibrary.PrintGlErrors("DeleteTexture");
			textureSizes.Remove(textureId);
		}

		public static void UpdateTexture(int textureId, ColorMap image)
		{
			GL.BindTexture(TextureTarget.Texture2D, textureId);
			ShaderLibrary.PrintGlErrors("UpdateTexture bind");

			GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, image.Width, image.Height,
				PixelFormat.Bgra, PixelType.UnsignedByte, image.RawPixels());
			ShaderLibrary.PrintGlErrors("UpdateTexture put data");

			int level = 1;
			foreach (var mipmap in makeMipmaps(image))
			{
				GL.TexSubImage2D(
					TextureTarget.Texture2D, level, 0, 0, mipmap.Width, mipmap.Height,
					PixelFormat.Bgra, PixelType.UnsignedByte, mipmap.RawPixels()
				);
				ShaderLibrary.PrintGlErrors("CreateTexture put mipmap data level " + level);
				level++;
			}

			textureSizes[textureId] = new Vector2(image.Width, image.Height);
			ShaderLibrary.PrintGlErrors("UpdateTexture generate mipmaps");
		}

		public static Vector2 TextureSize(int textureId)
		{
			return textureSizes[textureId];
		}

		private static IEnumerable<ColorMap> makeMipmaps(ColorMap image)
		{
			var lastLevel = image;
			while (lastLevel.Width > 1 && lastLevel.Height >= 1)
			{
				var levelImage = new ColorMap(lastLevel.Width / 2, lastLevel.Height / 2);
				for (int y = 0; y < levelImage.Height; y++)
					for (int x = 0; x < levelImage.Width; x++)
					{
						var colorSum = new Vector4();
						var alphaSum = 0f;
						foreach (var color in lastLevel.Subregion(2 * x, 2 * y, 2 * x + 1, 2 * y + 1))
						{
							colorSum += new Vector4(color.R, color.G, color.B, color.A) * color.A;
							alphaSum += color.A;
						}

						if (alphaSum > 0)
						{
							colorSum /= alphaSum;
							levelImage[x, y] = new Color4(colorSum.X, colorSum.Y, colorSum.Z, colorSum.W);
						}
					}

				lastLevel = levelImage;
				yield return levelImage;
			}
		}
	}
}
