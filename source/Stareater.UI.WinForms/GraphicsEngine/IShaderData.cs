using System;
using Stareater.GLData;

namespace Stareater.GraphicsEngine
{
	interface IShaderData
	{
		AGlProgram ForProgram { get; }
		int VertexDataSize { get; }

		IDrawable MakeDrawable(VertexArray vao, int objectIndex);
	}
}
