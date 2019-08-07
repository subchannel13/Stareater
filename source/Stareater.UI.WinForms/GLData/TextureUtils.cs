using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Stareater.GLData
{
	static class TextureUtils
	{
		private static Dictionary<int, Vector2> textureSizes = new Dictionary<int, Vector2>();

		public static int CreateTexture(ColorMap image)
		{
			int textureId = GL.GenTexture();
			ShaderLibrary.PrintGlErrors("CreateTexture generate ID");
			GL.BindTexture(TextureTarget.Texture2D, textureId);
			ShaderLibrary.PrintGlErrors("CreateTexture bind");
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
			ShaderLibrary.PrintGlErrors("CreateTexture parameters");

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
				0, PixelFormat.Bgra, PixelType.UnsignedByte, image.RawPixels());
			ShaderLibrary.PrintGlErrors("CreateTexture put data");

			textureSizes[textureId] = new Vector2(image.Width, image.Height);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D); //TODO(v0.9) make custom mipmap algorithm
			ShaderLibrary.PrintGlErrors("CreateTexture generate mipmaps");

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

			textureSizes[textureId] = new Vector2(image.Width, image.Height);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D); //TODO(v0.9) make custom mipmap algorithm
			ShaderLibrary.PrintGlErrors("UpdateTexture generate mipmaps");
		}

		public static Vector2 TextureSize(int textureId)
		{
			return textureSizes[textureId];
		}
	}
}
