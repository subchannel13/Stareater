using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GraphicsEngine;

namespace Stareater.GLData.SpriteShader
{
	class SpriteDrawable : IDrawable
	{
		private readonly VertexArray vao;
		private readonly int objectIndex;
		private SpriteData objectData;
		private TextureMinFilter minFilter;
		private TextureMagFilter magFilter;

		public SpriteDrawable(VertexArray vao, int objectIndex, SpriteData objectData)
		{
			this.vao = vao;
			this.objectIndex = objectIndex;
			this.objectData = objectData;
			this.updateFilters();
		}

		public void Draw(Matrix4 view, float z, Matrix4 viewportTransform)
		{
			var program = ShaderLibrary.Sprite;
			GL.UseProgram(program.ProgramId);
			this.vao.Bind(); //TODO(v0.8) set program and bind VAO outside
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);

			if (this.objectData.ClipArea != null)
			{
				var area = this.objectData.ClipArea.ScissorRectangle(view * viewportTransform);
				GL.Enable(EnableCap.ScissorTest);
				GL.Scissor(area.X, area.Y, area.Width, area.Height);
			}

			var mvp = this.objectData.LocalTransform * view;
			GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
			GL.BindTexture(TextureTarget.Texture2D, this.objectData.TextureId);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)this.minFilter);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)this.magFilter);
			GL.Uniform1(program.ZId, z);
			GL.Uniform4(program.ColorId, this.objectData.Color);

			GL.DrawArrays(PrimitiveType.Triangles, vao.ObjectStart(this.objectIndex), vao.ObjectSize(this.objectIndex));
			ShaderLibrary.PrintGlErrors("Draw sprites");

			if (this.objectData.ClipArea != null)
				GL.Disable(EnableCap.ScissorTest);
		}
		
		public void Update(IShaderData shaderUniforms)
		{
			this.objectData = (SpriteData)shaderUniforms;
			this.updateFilters();
		}

		private void updateFilters()
		{
			if (this.objectData.LinearFiltering)
			{
				this.minFilter = TextureMinFilter.Linear;
				this.magFilter = TextureMagFilter.Linear;
			}
			else
			{
				this.minFilter = TextureMinFilter.Nearest;
				this.magFilter = TextureMagFilter.Nearest;
			}
		}
	}
}
