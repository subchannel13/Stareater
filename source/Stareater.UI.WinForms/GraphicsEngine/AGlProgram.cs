using System;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GraphicsEngine
{
	abstract class AGlProgram
	{
		public int ProgramId { get; protected set; }
		
		public abstract void SetupAttributes();
		
		protected static int LoadShader(ShaderType type, string source)
		{
			var shader = GL.CreateShader(type);
			if (shader == 0)
			{
				System.Diagnostics.Trace.WriteLine("Error creating shader");
				return 0;
			}
		
			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);
			int compiled;
			GL.GetShader(shader, ShaderParameter.CompileStatus, out compiled);
			
			if (compiled == 0)
			{
				System.Diagnostics.Trace.WriteLine("Shader compilation error");
				System.Diagnostics.Trace.WriteLine("Reason: " + GL.GetShaderInfoLog(shader));
				GL.DeleteShader(shader);

				return 0;
			}
		
			return shader;
		}
		
		protected static int BuildProgram(int vertexShader, int fragmentShader)
		{
			var programObject = GL.CreateProgram();
			GL.AttachShader(programObject, vertexShader);
			GL.AttachShader(programObject, fragmentShader);
			GL.BindFragDataLocation(programObject, 0, "outputF");
			GL.LinkProgram(programObject);
		
			int linked;
			GL.GetProgram(programObject, GetProgramParameterName.LinkStatus, out linked);
			if (linked == 0)
			{
				System.Diagnostics.Trace.WriteLine("Program link error");
				System.Diagnostics.Trace.WriteLine("Reason: " + GL.GetProgramInfoLog(programObject));
				GL.DeleteProgram(programObject);
				
				return 0;
			}
			
			return programObject;
		}
	}
}
