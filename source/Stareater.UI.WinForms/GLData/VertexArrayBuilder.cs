using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;
using OpenTK.Graphics.OpenGL;
using Stareater.GLRenderers;

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
		}
		
		public void AddOrbitVertex(float x, float y)
		{
			this.vertices.Add(x); 
			this.vertices.Add(y);
			this.vertices.Add(x);
			this.vertices.Add(y);
		
			this.objectSize++;
		}
		
		public void AddPathRect(Vector2D fromPosition, Vector2D toPosition, double width, TextureInfo textureinfo)
		{
			var center = (fromPosition + toPosition) / 2;
			var length = toPosition - fromPosition;
			var direction = new Vector2D(length.X, length.Y);
			direction.Normalize();
			var widthDir = new Vector2D(-direction.Y, direction.X) * width;
			
			this.add(center - length / 2 + widthDir /2);
			this.vertices.Add(textureinfo.Coordinates[3].X);
			this.vertices.Add(textureinfo.Coordinates[3].Y);
			
			this.add(center + length / 2 + widthDir /2);
			this.vertices.Add(textureinfo.Coordinates[2].X);
			this.vertices.Add(textureinfo.Coordinates[2].Y);
		
			this.add(center + length / 2 - widthDir /2);
			this.vertices.Add(textureinfo.Coordinates[1].X);
			this.vertices.Add(textureinfo.Coordinates[1].Y);
		
			
			this.add(center + length / 2 - widthDir /2);
			this.vertices.Add(textureinfo.Coordinates[1].X);
			this.vertices.Add(textureinfo.Coordinates[1].Y);
		
			this.add(center - length / 2 - widthDir /2);
			this.vertices.Add(textureinfo.Coordinates[0].X);
			this.vertices.Add(textureinfo.Coordinates[0].Y);
		
			this.add(center - length / 2 + widthDir /2);
			this.vertices.Add(textureinfo.Coordinates[3].X);
			this.vertices.Add(textureinfo.Coordinates[3].Y);
		
			this.objectSize += 6;
		}
		
		public void AddTexturedRect(Vector2D center, int width, int height, TextureInfo textureinfo)
		{
			var widthDir = new Vector2D(width, 0);
			var heightDir = new Vector2D(0, height);
			
			this.add(center - widthDir / 2 + heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[3].X);
			this.vertices.Add(textureinfo.Coordinates[3].Y);
			
			this.add(center + widthDir / 2 + heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[2].X);
			this.vertices.Add(textureinfo.Coordinates[2].Y);
		
			this.add(center + widthDir / 2 - heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[1].X);
			this.vertices.Add(textureinfo.Coordinates[1].Y);
		
			
			this.add(center + widthDir / 2 - heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[1].X);
			this.vertices.Add(textureinfo.Coordinates[1].Y);
		
			this.add(center - widthDir / 2 - heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[0].X);
			this.vertices.Add(textureinfo.Coordinates[0].Y);
		
			this.add(center - widthDir / 2 + heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[3].X);
			this.vertices.Add(textureinfo.Coordinates[3].Y);
		
			this.objectSize += 6;
		}
		
		public void AddTexturedRect(TextureInfo textureinfo)
		{
			var widthDir = new Vector2D(1, 0);
			var heightDir = new Vector2D(0, 1);
			
			this.add(-widthDir / 2 + heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[3].X);
			this.vertices.Add(textureinfo.Coordinates[3].Y);
			
			this.add(widthDir / 2 + heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[2].X);
			this.vertices.Add(textureinfo.Coordinates[2].Y);
		
			this.add(widthDir / 2 - heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[1].X);
			this.vertices.Add(textureinfo.Coordinates[1].Y);
		
			
			this.add(widthDir / 2 - heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[1].X);
			this.vertices.Add(textureinfo.Coordinates[1].Y);
		
			this.add(-widthDir / 2 - heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[0].X);
			this.vertices.Add(textureinfo.Coordinates[0].Y);
		
			this.add(-widthDir / 2 + heightDir /2);
			this.vertices.Add(textureinfo.Coordinates[3].X);
			this.vertices.Add(textureinfo.Coordinates[3].Y);
		
			this.objectSize += 6;
		}
		
		public void AddTexturedVertex(float x, float y, float tx, float ty)
		{
			this.vertices.Add(x); 
			this.vertices.Add(y);
			this.vertices.Add(tx);
			this.vertices.Add(ty);
		
			this.objectSize++;
		}
		
		private void add(Vector2D v)
		{
			this.vertices.Add((float)v.X);
			this.vertices.Add((float)v.Y);
		}
	}
}
