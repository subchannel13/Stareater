namespace Stareater.GraphicsEngine.Animators
{
	class AnimationDelay : IAnimator
	{
		private double countdown;

		public AnimationDelay(double delay)
		{
			this.countdown = delay;
		}

		public void OnUpdate(double deltaTime)
		{
			this.countdown -= deltaTime;
		}

		public bool Finished
		{
			get
			{
				return countdown <= 0;
            }
		}
	}
}
