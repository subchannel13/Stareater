using Stareater.GLData.SpriteShader;
using Stareater.Utils;
using System;
using System.Drawing;

namespace Stareater.GraphicsEngine.Animators
{
	class PulsingAlpha : IAnimator
	{
		private double time = 0;
		private double period;
		private double amplitude;

		private PolygonData target;
        private Func<double, double> valueTransform;
		private Color color;

		public PulsingAlpha(PolygonData target, double period, double amplitude, Func<double, double> valueTransform, Color color)
		{
			this.target = target;
			this.period = period;
			this.amplitude = amplitude;
			this.valueTransform = valueTransform;
			this.color = color;
		}

		public void OnUpdate(double deltaTime)
		{
			this.time += deltaTime;

			double phase = Methods.GetPhase(this.time, this.period);
			var alpha = valueTransform(Math.Abs(phase - 0.5) * this.amplitude);

			var oldData = target.ShaderData as SpriteData;
			target.UpdateDrawable(new SpriteData(
				oldData.LocalTransform,
				oldData.TextureId,
				Color.FromArgb((int)(alpha * 255), this.color)
			));
		}
	}
}
