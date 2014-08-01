 

using Ikadn.Ikon.Types;
using System;
using System.Linq;
using System.Drawing;
using NGenerics.DataStructures.Mathematical;
using Stareater.Localization.StarNames;

namespace Stareater.Galaxy 
{
	public partial class StarData 
	{
		public Color Color { get; private set; }
		public float ImageSizeScale { get; private set; }
		public IStarName Name { get; private set; }
		public Vector2D Position { get; private set; }

		public StarData(Color color, float imageSizeScale, IStarName name, Vector2D position) 
		{
			this.Color = color;
			this.ImageSizeScale = imageSizeScale;
			this.Name = name;
			this.Position = position;
 
		} 


		internal StarData Copy() 
		{
			return new StarData(this.Color, this.ImageSizeScale, this.Name, this.Position);
 
		} 
 

		#region Saving
		public IkonComposite Save() 
		{
			var data = new IkonComposite(TableTag);
			var colorData = new IkonArray();
			colorData.Add(new IkonInteger(this.Color.R));
			colorData.Add(new IkonInteger(this.Color.G));
			colorData.Add(new IkonInteger(this.Color.B));
			data.Add(ColorKey, colorData);

			data.Add(SizeKey, new IkonFloat(this.ImageSizeScale));

			data.Add(NameKey, this.Name.Save());

			var positionData = new IkonArray();
			positionData.Add(new IkonFloat(this.Position.X));
			positionData.Add(new IkonFloat(this.Position.Y));
			data.Add(PositionKey, positionData);
			return data;
 
		}

		private const string TableTag = "StarData";
		private const string ColorKey = "color";
		private const string SizeKey = "size";
		private const string NameKey = "name";
		private const string PositionKey = "pos";
 
		#endregion
	}
}
