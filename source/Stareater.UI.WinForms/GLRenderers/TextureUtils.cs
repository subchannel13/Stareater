using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Stareater.GLRenderers
{
	class TextureUtils
	{
		#region Singleton
		static TextureUtils instance = null;

		public static TextureUtils Get
		{
			get
			{
				if (instance == null)
					instance = new TextureUtils();
				return instance;
			}
		}
		#endregion

		public Vector2[] SpriteQuad { get; private set; }
		
		private TextureUtils()
		{
			SpriteQuad = new Vector2[] {
				new Vector2(-0.5f, -0.5f),
				new Vector2(0.5f, -0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(-0.5f, 0.5f),
			};
		}
		
		public int CreateTexture(Bitmap image)
		{
			int textureId = GL.GenTexture();
			UpdateTexture(textureId, image);

			return textureId;
		}
		
		public void DeleteTexture(int textureId)
		{
			GL.DeleteTexture(textureId);
		}

		public void UpdateTexture(int textureId, Bitmap image)
		{
			System.Drawing.Imaging.BitmapData textureData =
				image.LockBits(
					new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb
				);

			GL.BindTexture(TextureTarget.Texture2D, textureId);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
				0, PixelFormat.Rgba, PixelType.UnsignedByte, textureData.Scan0);

			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			image.UnlockBits(textureData);
		}
		
		public void DrawSprite(TextureInfo textureInfo)
		{
			GL.BindTexture(TextureTarget.Texture2D, textureInfo.TextureId);
			GL.Begin(BeginMode.Quads);

			for(int i = 0; i < SpriteQuad.Length; i++) {
				GL.TexCoord2(textureInfo.TextureCoords[i]);
				GL.Vertex2(SpriteQuad[i]);
			}
			
			GL.End();
		}
		
		public void DrawSprite(TextureInfo textureInfo, float zOffset)
		{
			GL.BindTexture(TextureTarget.Texture2D, textureInfo.TextureId);
			GL.Begin(BeginMode.Quads);

			for(int i = 0; i < SpriteQuad.Length; i++) {
				GL.TexCoord2(textureInfo.TextureCoords[i]);
				GL.Vertex3(SpriteQuad[i].X, SpriteQuad[i].Y, zOffset);
			}
			
			GL.End();
		}
	}
}
