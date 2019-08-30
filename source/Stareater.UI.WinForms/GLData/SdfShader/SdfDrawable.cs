using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GraphicsEngine;

namespace Stareater.GLData.SdfShader
{
	class SdfDrawable : IDrawable
	{
		private readonly VertexArray vao;
		private readonly int objectIndex;
		private SdfData objectData;

		public SdfDrawable(VertexArray vao, int objectIndex, SdfData objectData)
		{
			this.vao = vao;
			this.objectIndex = objectIndex;
			this.objectData = objectData;
		}

		public void Draw(Matrix4 view, float z, Matrix4 viewportTransform)
		{
			var program = ShaderLibrary.Sdf;
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
			GL.Uniform1(program.ZId, z);
			GL.Uniform4(program.ColorId, this.objectData.Color);
			GL.Uniform1(program.SmoothDistId, this.objectData.SmoothDist);

			GL.DrawArrays(PrimitiveType.Triangles, vao.ObjectStart(this.objectIndex), vao.ObjectSize(this.objectIndex));
			ShaderLibrary.PrintGlErrors("Draw sprites");

			if (this.objectData.ClipArea != null)
				GL.Disable(EnableCap.ScissorTest);
		}

		public void Update(IShaderData shaderUniforms)
		{
			this.objectData = shaderUniforms as SdfData;
		}
	}
}
