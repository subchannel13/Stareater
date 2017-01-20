using System;
using Stareater.GLData;
using Stareater.GLRenderers;

namespace Stareater.GraphicsEngine
{
	interface IShaderData
	{
		AGlProgram ForProgram { get; }
		int VertexDataSize { get; }

		IDrawable MakeDrawable(VertexArray vao, int objectIndex);
	}
}
