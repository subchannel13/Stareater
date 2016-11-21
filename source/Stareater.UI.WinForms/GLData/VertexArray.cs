using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class VertexArray
	{
		private int vao;
		private int vbo;
		private int[] objectStart;
		private int[] objectSize;
		
		public VertexArray(int vao, int vbo, IEnumerable<int> objectStart, IEnumerable<int> objectSize)
		{
			this.vao = vbo;
			this.vbo = vbo;
			this.objectStart = objectStart.ToArray();
			this.objectSize = objectSize.ToArray();
		}
		
		public void Bind()
		{
			GL.BindVertexArray(this.vao);
		}
		
		public int ObjectStart(int index)
		{
			return this.objectStart[index];
		}
		
		public int ObjectSize(int index)
		{
			return this.objectSize[index];
		}
	}
}
