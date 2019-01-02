using OpenTK;

namespace Stareater.GraphicsEngine
{
	interface IDrawable
	{
		void Draw(Matrix4 view, float z, Matrix4 viewportTransform);
		void Update(IShaderData shaderUniforms);
	}
}
