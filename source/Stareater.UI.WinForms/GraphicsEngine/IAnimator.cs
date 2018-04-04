namespace Stareater.GraphicsEngine
{
	interface IAnimator
	{
		void OnUpdate(double deltaTime);

		bool Finished { get; }
	}
}
