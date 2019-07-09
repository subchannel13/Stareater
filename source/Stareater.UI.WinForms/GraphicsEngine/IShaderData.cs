using Stareater.GLData;
using System.Drawing;

namespace Stareater.GraphicsEngine
{
	interface IShaderData
	{
		AGlProgram ForProgram { get; }
		int VertexDataSize { get; }

		float Alpha { get; set; }
		IDrawable MakeDrawable(VertexArray vao, int objectIndex);
	}
}
