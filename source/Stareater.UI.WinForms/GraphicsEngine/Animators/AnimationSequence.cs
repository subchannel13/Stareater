using System.Collections.Generic;

namespace Stareater.GraphicsEngine.Animators
{
	class AnimationSequence : IAnimator
	{
		private Queue<IAnimator> sequence;

		public AnimationSequence(params IAnimator[] sequence)
		{
			this.sequence = new Queue<IAnimator>(sequence);
		}

		public void OnUpdate(double deltaTime)
		{
			while (this.sequence.Count > 0)
			{
				var topElement = this.sequence.Peek();
				topElement.OnUpdate(deltaTime);

				if (topElement.Finished)
					this.sequence.Dequeue();
				else
					break;
			}
		}

		public void FastForward()
		{
			while (this.sequence.Count > 0)
				this.sequence.Dequeue().FastForward();
        }

		public bool Finished
		{
			get
			{
				return this.sequence.Count == 0;
            }
		}
	}
}
