using OpenTK;

namespace Stareater.GLRenderers
{
	interface IDrawable
	{
		void Draw(Matrix4 view);
	}
}
