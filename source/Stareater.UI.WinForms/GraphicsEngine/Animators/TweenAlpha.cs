using Stareater.GLData.SpriteShader;
using System.Drawing;

namespace Stareater.GraphicsEngine.Animators
{
	class TweenAlpha : IAnimator
	{
		private PolygonData target;
		private double finalValue;
		private double changeSpeed;

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

			var oldData = this.target.ShaderData as SpriteData;
			var oldColor = Color.FromArgb(oldData.Color.ToArgb());
			this.target.UpdateDrawable(new SpriteData(
				oldData.LocalTransform,
				oldData.TextureId,
				Color.FromArgb((int)(this.currentValue * 255), oldColor)
			));
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
