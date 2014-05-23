 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using NGenerics.DataStructures.Mathematical;
using Stareater.Localization.StarNames;

namespace Stareater.Galaxy 
{
	public partial class StarData 
	{
		public Color Color;
		public float ImageSizeScale;
		public IStarName Name;
		public Vector2D Position;
		public double Radiation;

		public StarData(Color color, float imageSizeScale, IStarName name, Vector2D position, double radiation) 
		{
			this.Color = color;
			this.ImageSizeScale = imageSizeScale;
			this.Name = name;
			this.Position = position;
			this.Radiation = radiation;
		}
		
		internal StarData Copy()
		{
			return new StarData(this.Color, this.ImageSizeScale, this.Name, this.Position, this.Radiation);
		}
	}
}
