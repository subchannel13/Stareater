using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class VertexBuffer
	{
		private int vbo;
		private int[] objectStart;
		private int[] objectSize;
		
		public VertexBuffer(int vbo, IEnumerable<int> objectStart, IEnumerable<int> objectSize)
		{
			this.vbo = vbo;
			this.objectStart = objectStart.ToArray();
			this.objectSize = objectSize.ToArray();
		}
		
		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
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
