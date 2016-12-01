using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GLData;

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

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			image.UnlockBits(textureData);
		}
		
		public static void DrawSprite(SpriteInfo spriteInfo, Matrix4 view, Matrix4 transform, float z, Color color)
		{
			var program = ShaderLibrary.Sprite;
			GL.UseProgram(program.ProgramId);
			spriteInfo.SpriteSheet.Bind();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);
			
			var mvp = transform * view;
			GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
			GL.BindTexture(TextureTarget.Texture2D, spriteInfo.Texture.Id);
			GL.Uniform1(program.ZId, z);
			GL.Uniform4(program.ColorId, color);
		
			GL.DrawArrays(BeginMode.Triangles, spriteInfo.SpriteSheet.ObjectStart(spriteInfo.SpriteIndex), spriteInfo.SpriteSheet.ObjectSize(spriteInfo.SpriteIndex));
			ShaderLibrary.PrintGlErrors("Draw sprites");
		}
	}
}
