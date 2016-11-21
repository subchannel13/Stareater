using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class SpriteDrawable
	{
		private readonly VertexArray vao;
		private readonly IEnumerable<SpriteGlProgram.ObjectData> objects;
		
		public SpriteDrawable(VertexArray vao, IEnumerable<SpriteGlProgram.ObjectData> objects)
		{
			this.vao = vao;
			this.objects = objects;
		}
		
		public void Draw(Matrix4 view)
		{
			var program = ShaderLibrary.Sprite;
			GL.UseProgram(program.ProgramId);
			vao.Bind();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);
			
			int objectIndex = 0;
			foreach(var batch in this.objects)
			{
				var mvp = view * batch.LocalTransform;
				GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
				GL.BindTexture(TextureTarget.Texture2D, batch.TextureId);
				GL.Uniform1(program.ZId, batch.Z);
				GL.Uniform4(program.ColorId, batch.Color);
			
				GL.DrawArrays(BeginMode.Triangles, vao.ObjectStart(objectIndex), vao.ObjectSize(objectIndex));
				ShaderLibrary.PrintGlErrors("Draw sprites");
				objectIndex++;
			}
		}
	}
}
