using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GraphicsEngine;

namespace Stareater.GLData.OrbitShader
{
	class OrbitDrawable : IDrawable
	{
		private readonly VertexArray vao;
		private readonly int objectIndex;
		private OrbitData objectData;
		
		public OrbitDrawable(VertexArray vao, int objectIndex, OrbitData objectData)
		{
			this.objectIndex = objectIndex;
			this.objectData = objectData;
			this.vao = vao;
		}
		
		public void Draw(Matrix4 view, float z, Matrix4 viewportTransform)
		{
			var program = ShaderLibrary.PlanetOrbit;
			GL.UseProgram(program.ProgramId);
			this.vao.Bind(); //TODO(v0.8) set program and bind VAO outside
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);
			GL.BindTexture(TextureTarget.Texture2D, this.objectData.TextureId);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

			var mvp = this.objectData.LocalTransform * view;
			var textureTransform = this.objectData.TextureTransform;
			GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
			GL.Uniform1(program.ZId, z);
			GL.Uniform4(program.ColorId, this.objectData.Color);
			GL.Uniform1(program.MinRId, this.objectData.MinRadius);
			GL.Uniform1(program.MaxRId, this.objectData.MaxRadius);
			GL.UniformMatrix3(program.TextureTransformId, false, ref textureTransform);
		
			GL.DrawArrays(PrimitiveType.Triangles, vao.ObjectStart(this.objectIndex), vao.ObjectSize(this.objectIndex));
			ShaderLibrary.PrintGlErrors("Draw orbits");
		}

		public void Update(IShaderData shaderUniforms)
		{
			this.objectData = shaderUniforms as OrbitData;
		}
	}
}
