using Stareater.GLData.SpriteShader;
using Stareater.Utils;
using System;
using System.Drawing;

namespace Stareater.GraphicsEngine.Animators
{
	class OscillatingAlpha : IAnimator
	{
		private double time = 0;
		private readonly double period;
		private readonly double amplitude;

		private readonly PolygonData target;
		private readonly Func<double, double> valueTransform;
		private readonly Color color;

		public OscillatingAlpha(PolygonData target, double period, double amplitude, Func<double, double> valueTransform, Color color)
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
			var alpha = this.valueTransform(Math.Abs(phase - 0.5) * this.amplitude);

			var oldData = this.target.ShaderData as SpriteData;
			this.target.UpdateDrawable(new SpriteData(
				oldData.LocalTransform,
				oldData.TextureId,
				Color.FromArgb((int)(alpha * 255), this.color),
				oldData.ClipArea
			));
		}

		public void FastForward()
		{
			throw new InvalidOperationException("Oscillation animator can't be fast forwarded");
		}

		public bool Finished
		{
			get
			{
				return false;
			}
		}
	}
}
