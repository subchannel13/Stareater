namespace Stareater.GraphicsEngine
{
	interface IAnimator
	{
		void OnUpdate(double deltaTime);
		void FastForward();

		bool Finished { get; }
	}
}
