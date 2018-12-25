using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Stareater.GraphicsEngine;

namespace Stareater.GLData
{
	class VertexArrayBuilder
	{
		private List<float> vertices = new List<float>();
		private List<int> objectStarts = new List<int>();
		private List<int> objectSizes = new List<int>();
	
		private int objectSize = 0;
		
		public void BeginObject()
		{
			var previous = this.objectStarts.Count - 1;
			this.objectSize = 0;
			this.objectStarts.Add(this.objectStarts.Count == 0 ? 0 : (this.objectStarts[previous] + this.objectSizes[previous]));
		}
		
		public void EndObject()
		{
			this.objectSizes.Add(this.objectSize);
		}
		
		public VertexArray Generate(AGlProgram forProgram)
		{
			var vao = GL.GenVertexArray();
			GL.BindVertexArray(vao);
			
			var vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.vertices.Count * sizeof (float)), this.vertices.ToArray(), BufferUsageHint.StaticDraw);
			forProgram.SetupAttributes();
			GL.BindVertexArray(0);
			ShaderLibrary.PrintGlErrors("Generate VAO");
			
			return new VertexArray(vao, vbo, this.objectStarts, this.objectSizes);
		}

		public void Update(VertexArray vao)
		{
			vao.BindVbo();
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.vertices.Count * sizeof (float)), this.vertices.ToArray(), BufferUsageHint.StaticDraw);
			vao.Update(this.objectStarts, this.objectSizes);
		}

		public void Add(ICollection<float> vertexData, int vertexDataSize)
		{
			this.vertices.AddRange(vertexData);
			this.objectSize += vertexData.Count / vertexDataSize;
		}

		public int Count
		{
			get { return this.objectStarts.Count; }
		}
	}
}
