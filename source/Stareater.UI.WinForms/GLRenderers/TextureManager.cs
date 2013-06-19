using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Stareater.GLRenderers
{
	class TextureManager
	{
		#region Singleton
		static TextureManager instance = null;

		public static TextureManager Get
		{
			get
			{
				if (instance == null)
					instance = new TextureManager();
				return instance;
			}
		}
		#endregion

		public int GalaxyTextureId;
		public int FontTextureId;

		private TextureManager()
		{ }

		public void Load(TextureContext context, Bitmap image)
		{
			switch(context)
			{
				case TextureContext.Font:
					FontTextureId = CreateTexture(image);
					break;
				case TextureContext.GalaxyMap:
					GalaxyTextureId = CreateTexture(image);
					break;
				default:
					throw new ArgumentOutOfRangeException("context");
			}
		}

		public void Unload(TextureContext context)
		{
			switch(context)
			{
				case TextureContext.Font:
					GL.DeleteTexture(FontTextureId);
					FontTextureId = 0;
					break;
				case TextureContext.GalaxyMap:
					GL.DeleteTexture(GalaxyTextureId);
					GalaxyTextureId = 0;
					break;
				default:
					throw new ArgumentOutOfRangeException("context");
			}
		}
		
		public void UnloadAll()
		{
			int[] allIds = new int[] {
				FontTextureId,
				GalaxyTextureId,
			};
			GL.DeleteTextures(allIds.Length, allIds);
			
			FontTextureId = 0;
			GalaxyTextureId = 0;
		}
		
		private static int CreateTexture(Bitmap image)
		{
			System.Drawing.Imaging.BitmapData textureData =
				image.LockBits(
					new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb
				);

			int textureId = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
				0, PixelFormat.Rgba, PixelType.UnsignedByte, textureData.Scan0);

			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			image.UnlockBits(textureData);
			
			return textureId;
		}
	}
}
