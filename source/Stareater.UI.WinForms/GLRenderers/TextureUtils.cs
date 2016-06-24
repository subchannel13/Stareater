using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLRenderers
{
	static class TextureUtils
	{
		public static Vector2[] SpriteQuad 
		{ 
			get
			{
				return new Vector2[] {
					new Vector2(-0.5f, -0.5f),
					new Vector2(0.5f, -0.5f),
					new Vector2(0.5f, 0.5f),
					new Vector2(-0.5f, 0.5f),
				};
			}
		}
		
		public static int CreateTexture(Bitmap image)
		{
			int textureId = GL.GenTexture();
			UpdateTexture(textureId, image);

			return textureId;
		}
		
		public static void DeleteTexture(int textureId)
		{
			GL.DeleteTexture(textureId);
		}

		public static void UpdateTexture(int textureId, Bitmap image)
		{
			System.Drawing.Imaging.BitmapData textureData =
				image.LockBits(
					new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					image.PixelFormat
				);

			GL.BindTexture(TextureTarget.Texture2D, textureId);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
				0, PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);

			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			image.UnlockBits(textureData);
		}
		
		public static void DrawSprite(TextureInfo textureInfo)
		{
			GL.BindTexture(TextureTarget.Texture2D, textureInfo.TextureId);
			GL.Begin(PrimitiveType.Quads);

			for(int i = 0; i < SpriteQuad.Length; i++) {
				GL.TexCoord2(textureInfo.TextureCoords[i]);
				GL.Vertex2(SpriteQuad[i]);
			}
			
			GL.End();
		}
		
		public static void DrawSprite(TextureInfo textureInfo, float zOffset)
		{
			GL.BindTexture(TextureTarget.Texture2D, textureInfo.TextureId);
			GL.Begin(PrimitiveType.Quads);

			for(int i = 0; i < SpriteQuad.Length; i++) {
				GL.TexCoord2(textureInfo.TextureCoords[i]);
				GL.Vertex3(SpriteQuad[i].X, SpriteQuad[i].Y, zOffset);
			}
			
			GL.End();
		}
	}
}
