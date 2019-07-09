namespace Stareater.GraphicsEngine.Animators
{
	class TweenAlpha : IAnimator
	{
		private readonly PolygonData target;
		private readonly double finalValue;
		private readonly double changeSpeed;

		private double currentValue;

		public TweenAlpha(PolygonData target, double startingValue, double finalValue, double changeSpeed)
		{
			this.target = target;
			this.currentValue = startingValue;
			this.finalValue = finalValue;
			this.changeSpeed = changeSpeed;
		}

		public void OnUpdate(double deltaTime)
		{
			this.currentValue += deltaTime * this.changeSpeed;

			if (this.Finished)
				this.currentValue = this.finalValue;

			this.target.ShaderData.Alpha = (float)this.currentValue;
		}

		public void FastForward()
		{
			this.currentValue = this.finalValue;
			this.OnUpdate(0);
		}

		public bool Finished
		{
			get
			{
				return (this.changeSpeed > 0) ?
					this.currentValue >= this.finalValue :
					this.currentValue <= this.finalValue;
			}
		}
	}
}
