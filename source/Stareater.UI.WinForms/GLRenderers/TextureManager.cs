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

		public static new TextureManager Get
		{
			get
			{
				if (instance == null)
					instance = new TextureManager();
				return instance;
			}
		}
		#endregion

		const string GalaxyTexturePath = "./images/galaxy textures.png";

		public int GalaxyTextureId;

		private TextureManager()
		{ }

		public void Load()
		{
			if (System.IO.File.Exists(GalaxyTexturePath)) {
				//make a bitmap out of the file on the disk
				Bitmap textureBitmap = new Bitmap(GalaxyTexturePath);
				//get the data out of the bitmap
				System.Drawing.Imaging.BitmapData textureData =
				textureBitmap.LockBits(
						new System.Drawing.Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height),
						System.Drawing.Imaging.ImageLockMode.ReadOnly,
						System.Drawing.Imaging.PixelFormat.Format32bppArgb
					);

				//Code to get the data to the OpenGL Driver

				//generate one texture and put its ID number into the "Texture" variable
				GalaxyTextureId = GL.GenTexture();
				//tell OpenGL that this is a 2D texture
				GL.BindTexture(TextureTarget.Texture2D, GalaxyTextureId);

				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, textureBitmap.Width, textureBitmap.Height,
					0, PixelFormat.Rgba, PixelType.UnsignedByte, textureData.Scan0);

				//the following code sets certian parameters for the texture
				GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

				//load the data by telling OpenGL to build mipmaps out of the bitmap data
				//GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

				//free the bitmap data (we dont need it anymore because it has been passed to the OpenGL driver
				textureBitmap.UnlockBits(textureData);
			}
		}

		internal void Unload()
		{
			GL.DeleteTexture(GalaxyTextureId);
		}
	}
}
