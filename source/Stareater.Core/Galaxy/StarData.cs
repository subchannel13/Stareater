using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Galaxy
{
	public class StarData
	{
		public const int MaxPlanets = 8;

		public Color Color { get; private set; }
		public float ImageSizeScale { get; private set; }
		public string Name { get; private set; }

		public Vector2D Position { get; private set; }
		public double Radiation { get; private set; }

		public StarData(Color color, float imageSizeScale, string name, Vector2D position, double radiation)
		{
			this.Color = color;
			this.ImageSizeScale = imageSizeScale;
			this.Name = name;
			this.Position = position;
			this.Radiation = radiation;
		}
	}
}
