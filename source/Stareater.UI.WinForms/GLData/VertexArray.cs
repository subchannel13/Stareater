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
			this.vao = vao;
			this.vbo = vbo;
			this.objectStart = objectStart.ToArray();
			this.objectSize = objectSize.ToArray();
		}
		
		public void Bind()
		{
			GL.BindVertexArray(this.vao);
		}
		
		public void BindVbo()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);
		}
		
		public void Delete()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DeleteBuffer(this.vbo);
			
			GL.BindVertexArray(0);
			GL.DeleteVertexArray(this.vao);
		}

		public int ObjectStart(int index)
		{
			return this.objectStart[index];
		}
		
		public int ObjectSize(int index)
		{
			return this.objectSize[index];
		}
		
		public void Update(IEnumerable<int> objectStart, IEnumerable<int> objectSize)
		{
			this.objectStart = objectStart.ToArray();
			this.objectSize = objectSize.ToArray();
		}
	}
}
