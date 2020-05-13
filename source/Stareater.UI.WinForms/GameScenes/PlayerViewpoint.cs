using OpenTK;
using Stareater.Utils;
using System;

namespace Stareater.GameScenes
{
	class PlayerViewpoint
	{
		private const double ZoomBase = 1.2;
		private const int MaxZoom = 10;
		private const int MinZoom = -10;

		private Vector2 boundsMin;
		private Vector2 boundsMax;

		private Vector2 perspectiveOffset;
		private int perspectiveZoom = 0;

		public PlayerViewpoint(Vector2 boundsMin, Vector2 boundsMax)
		{
			this.boundsMax = boundsMax;
			this.boundsMin = boundsMin;
			this.ZoomLevel = 2;
		}

		public Vector2 Offset 
		{
			get => this.perspectiveOffset;
			set
			{
				this.perspectiveOffset = new Vector2(
					Methods.Clamp(value.X, this.boundsMin.X, this.boundsMax.X),
					Methods.Clamp(value.Y, this.boundsMin.Y, this.boundsMax.Y)
				);
			}
		}

		public int ZoomLevel 
		{
			get => this.perspectiveZoom;
			set
			{
				this.perspectiveZoom = Methods.Clamp(value, MinZoom, MaxZoom);
			}
		}

		public float ZoomFactor => (float)Math.Pow(ZoomBase, -this.perspectiveZoom);
	}
}
