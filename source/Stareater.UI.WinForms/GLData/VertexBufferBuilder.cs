using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class VertexBufferBuilder
	{
		private List<float> vertices = new List<float>();
		private List<int> objectStarts = new List<int>();
		private List<int> objectSizes = new List<int>();
	
		private int vertexSize;
		private int objectSize = 0;
		
		public VertexBufferBuilder(int vertexSize)
		{
			this.vertexSize = vertexSize;
		}
		
		public void BeginObject()
		{
			this.objectSize = 0;
			this.objectStarts.Add(this.vertices.Count);
		}
		
		public void EndObject()
		{
			this.objectSizes.Add(this.objectSize);
		}
		
		public static int Vao;
		
		public VertexBuffer GenBuffer()
		{
			//int vao;
			int vbo;

			Vao = GL.GenVertexArray();
			GL.BindVertexArray(Vao);
			
			vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.vertices.Count * sizeof (float)), this.vertices.ToArray(), BufferUsageHint.StaticDraw);
		
			var program = ShaderLibrary.Sprite;
			GL.VertexAttribPointer(program.LocalPositionId, 2, VertexAttribPointerType.Float, false, SpriteGlProgram.VertexSize, 0);
			GL.EnableVertexAttribArray(program.LocalPositionId);
			GL.VertexAttribPointer(program.TexturePositionId, 2, VertexAttribPointerType.Float, false, SpriteGlProgram.VertexSize, 2 * sizeof(float));
			GL.EnableVertexAttribArray(program.TexturePositionId);
			
			return new VertexBuffer(vbo, this.objectStarts, this.objectSizes);
		}
		
		public void AddTexturedRect(Vector2D center, Vector2D width, Vector2D height, Vector2D[] textureCoords)
		{
			this.add(center - width / 2 + height /2);
			this.add(textureCoords[0]);
			
			this.add(center + width / 2 + height /2);
			this.vertices.Add((float)textureCoords[1].X);
			this.vertices.Add((float)textureCoords[0].Y);
		
			this.add(center + width / 2 - height /2);
			this.add(textureCoords[1]);
		
			
			this.add(center + width / 2 - height /2);
			this.add(textureCoords[1]);
		
			this.add(center - width / 2 - height /2);
			this.vertices.Add((float)textureCoords[0].X);
			this.vertices.Add((float)textureCoords[1].Y);
		
			this.add(center - width / 2 + height /2);
			this.add(textureCoords[0]);
		
			this.objectSize += 6;
		}
		
		public void AddTexturedRect(float x0, float y0, float w, float h, float tx0, float ty0, float tx1, float ty1)
		{
			this.vertices.Add(x0 - w / 2); 
			this.vertices.Add(y0 + h / 2);
			this.vertices.Add(tx0);
			this.vertices.Add(ty0);
		
			this.vertices.Add(x0 + w / 2); 
			this.vertices.Add(y0 + h / 2);
			this.vertices.Add(tx1);
			this.vertices.Add(ty0);
		
			this.vertices.Add(x0 + w / 2); 
			this.vertices.Add(y0 - h / 2);
			this.vertices.Add(tx1);
			this.vertices.Add(ty1);
		
		
			this.vertices.Add(x0 + w / 2); 
			this.vertices.Add(y0 - h / 2);
			this.vertices.Add(tx1);
			this.vertices.Add(ty1);
		
			this.vertices.Add(x0 - w / 2); 
			this.vertices.Add(y0 - h / 2);
			this.vertices.Add(tx0);
			this.vertices.Add(ty1);
		
			this.vertices.Add(x0 - w / 2); 
			this.vertices.Add(y0 + h / 2);
			this.vertices.Add(tx0);
			this.vertices.Add(ty0);
		
			this.objectSize += 6;
		}
		
		private void add(Vector2D v)
		{
			this.vertices.Add((float)v.X);
			this.vertices.Add((float)v.Y);
		}
	}
}
