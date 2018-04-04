using Stareater.Utils.Collections;

namespace Stareater.GraphicsEngine.Animators
{
	class ParallelAnimation : IAnimator
	{
		private PendableSet<IAnimator> animations;

		public ParallelAnimation(params IAnimator[] animations)
		{
			this.animations = new PendableSet<IAnimator>(animations);
		}

		public void OnUpdate(double deltaTime)
		{
			foreach(var animation in this.animations)
			{
				animation.OnUpdate(deltaTime);

				if (animation.Finished)
					this.animations.PendRemove(animation);
			}

			this.animations.ApplyPending();
		}

		public bool Finished
		{
			get
			{
				return animations.Count == 0;
			}
		}
	}
}
