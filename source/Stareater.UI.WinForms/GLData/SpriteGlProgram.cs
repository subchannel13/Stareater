using System;
using System.IO;
using System.Reflection;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class SpriteGlProgram
	{
		private int vertexShaderId;
		private int fragmentShaderId;
		private int programId;
	
		public void Load()
		{
			string vertexShaderSource;
			string fragmentShaderSource;
			var bla = Assembly.GetExecutingAssembly().GetManifestResourceNames();
			
			using(var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SpriteVertexShader")))
				vertexShaderSource = stream.ReadToEnd();
			
			using(var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SpriteFragmentShader")))
				fragmentShaderSource = stream.ReadToEnd();
			
			this.vertexShaderId = loadShader(ShaderType.VertexShader, vertexShaderSource);
			this.fragmentShaderId = loadShader(ShaderType.FragmentShader, fragmentShaderSource);
			this.programId = buildProgram(this.vertexShaderId, this.fragmentShaderId);
			
			ErrorCode err;
		    while ((err = GL.GetError()) != ErrorCode.NoError) {
		        System.Diagnostics.Trace.WriteLine("OpenGL error: \n" + err);
		    }
		}
		
		private int loadShader(ShaderType type, string source)
		{
			var shader = GL.CreateShader(type);
			if (shader == 0)
			{
				System.Diagnostics.Trace.WriteLine("Error creating shader\n");
				return 0;
			}
		
			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);
			int compiled;
			GL.GetShader(shader, ShaderParameter.CompileStatus, out compiled);
			
			if (compiled == 0)
			{
				System.Diagnostics.Trace.WriteLine("Shader compilation error\n");
				System.Diagnostics.Trace.WriteLine("Reason: " + GL.GetShaderInfoLog(shader));
				GL.DeleteShader(shader);

				return 0;
			}
		
			return shader;
		}
		
		private int buildProgram(int vertexShader, int fragmentShader)
		{
			var programObject = GL.CreateProgram();
			GL.AttachShader(programObject, vertexShader);
			GL.AttachShader(programObject, fragmentShader);
			GL.LinkProgram(programObject);
		
			int linked;
			GL.GetProgram(programObject, ProgramParameter.LinkStatus, out linked);
			if (linked == 0)
			{
				System.Diagnostics.Trace.WriteLine("Program link error");
				GL.DeleteProgram(programObject);
				
				return 0;
			}
			
			return programObject;
		}
	}
}
