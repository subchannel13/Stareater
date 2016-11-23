using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GLData;

namespace Stareater.GLRenderers
{
	class TextDrawable
	{
		public SpriteGlProgram.ObjectData ObjectData { get; private set; }
		private VertexArray vao = null;
		private float textAdjustment;
		private string lastText = null;
		
		public TextDrawable(SpriteGlProgram.ObjectData objectData, float textAdjustment)
		{
			this.ObjectData = objectData;
			this.textAdjustment = textAdjustment;
		}
		
		public void Draw(Matrix4 view, string text)
		{
			if (vao == null || text != this.lastText) 
				updateVao(text);
			
			var program = ShaderLibrary.Sprite;
			GL.UseProgram(program.ProgramId);
			this.vao.Bind();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);
			
			var mvp = this.ObjectData.LocalTransform * view;
			GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
			GL.BindTexture(TextureTarget.Texture2D, this.ObjectData.TextureId);
			GL.Uniform1(program.ZId, this.ObjectData.Z);
			GL.Uniform4(program.ColorId, this.ObjectData.Color);
		
			GL.DrawArrays(BeginMode.Triangles, vao.ObjectStart(0), vao.ObjectSize(0));
			ShaderLibrary.PrintGlErrors("Draw sprites");
		}

		private void updateVao(string text)
		{
			var vaoBuilder = new VertexArrayBuilder();
			vaoBuilder.BeginObject();
			TextRenderUtil.Get.BufferText(
				text, 
				this.textAdjustment,
				Matrix4.Identity,
				vaoBuilder
			);
			vaoBuilder.EndObject();
			
			if (this.vao == null)
				this.vao = vaoBuilder.Generate(ShaderLibrary.Sprite);
			else
				vaoBuilder.Update(this.vao);
		}
	}
}
