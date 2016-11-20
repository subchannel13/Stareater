using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class SpriteBatchDrawable
	{
		private readonly VertexArray vbo;
		private readonly IEnumerable<SpriteGlProgram.ObjectData> objects;
		
		public SpriteBatchDrawable(VertexArray vbo, IEnumerable<SpriteGlProgram.ObjectData> objects)
		{
			this.vbo = vbo;
			this.objects = objects;
		}
		
		public void Prepare()
		{
			var program = ShaderLibrary.Sprite;
			GL.UseProgram(program.ProgramId);
			//vbo.Bind();
			/*ShaderLibrary.Use(program);
			
			GL.VertexAttribPointer(program.LocalPositionId, 2, VertexAttribPointerType.Float, false, SpriteGlProgram.VertexSize, 0);
			GL.VertexAttribPointer(program.TexturePositionId, 2, VertexAttribPointerType.Float, false, SpriteGlProgram.VertexSize, 2 * sizeof(float));*/

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);
		}
		
		public void Draw(Matrix4 view)
		{
			var program = ShaderLibrary.Sprite;
			int objectIndex = 0;
			foreach(var batch in this.objects)
			{
				var mvp = view * batch.LocalTransform;
				GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
				GL.BindTexture(TextureTarget.Texture2D, batch.TextureId);
				GL.Uniform1(program.ZId, batch.Z);
				GL.Uniform4(program.ColorId, batch.Color);
			
				GL.DrawArrays(BeginMode.Triangles, 0 /*vbo.ObjectStart(objectIndex)*/, 36 /*vbo.ObjectSize(objectIndex)*/);
				ShaderLibrary.PrintGlErrors("Draw sprites");
				objectIndex++;
			}
		}
	}
}
