using OpenTK;
using Stareater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

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

		public IGalaxySelection Selection { get; set; }

		public PlayerViewpoint(IEnumerable<Vector2> mapPoints, float mapMargins)
		{
			this.boundsMin = new Vector2(
				mapPoints.Min(p => p.X) - mapMargins,
				mapPoints.Min(p => p.Y) - mapMargins
			);
			this.boundsMax = new Vector2(
				mapPoints.Max(p => p.X) + mapMargins,
				mapPoints.Max(p => p.Y) + mapMargins
			);
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
