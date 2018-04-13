using System;

namespace Stareater.GraphicsEngine.Animators
{
	class CallbackAnimation : IAnimator
	{
		private Action callback;

		public CallbackAnimation(Action callback)
		{
			this.callback = callback;
		}

		public void OnUpdate(double deltaTime)
		{
			if (this.callback != null)
				this.callback();

			this.callback = null;
		}

		public void FastForward()
		{
			this.OnUpdate(0);
		}

		public bool Finished
		{
			get
			{
				return this.callback == null;
			}
		}
	}
}
