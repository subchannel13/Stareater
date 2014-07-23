 
using Ikadn.Ikon.Types;
using System;
using System.Linq;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy 
{
	public partial class Planet 
	{
		public StarData Star { get; private set; }
		public int Position { get; private set; }
		public PlanetType Type { get; private set; }
		public double Size { get; private set; }
		public double MineralsSurface { get; private set; }
		public double MineralsDeep { get; private set; }

		public Planet(StarData star, int position, PlanetType type, double size, double mineralsSurface, double mineralsDeep) 
		{
			this.Star = star;
			this.Position = position;
			this.Type = type;
			this.Size = size;
			this.MineralsSurface = mineralsSurface;
			this.MineralsDeep = mineralsDeep;
 
		} 


		internal Planet Copy(GalaxyRemap galaxyRemap) 
		{
			return new Planet(galaxyRemap.Stars[this.Star], this.Position, this.Type, this.Size, this.MineralsSurface, this.MineralsDeep);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			IkonComposite data = new IkonComposite(TableTag);
			
			data.Add(StarKey, new IkonInteger(indexer.IndexOf(this.Star)));

			data.Add(PositionKey, new IkonInteger(this.Position));

			data.Add(TypeKey, new IkonComposite(this.Type.ToString()));

			data.Add(SizeKey, new IkonFloat(this.Size));

			data.Add(MineralsSurfaceKey, new IkonFloat(this.MineralsSurface));

			data.Add(MineralsDeepKey, new IkonFloat(this.MineralsDeep));
 

			return data;
		}

		private const string TableTag = "Planet"; 
		private const string StarKey = "star";
		private const string PositionKey = "position";
		private const string TypeKey = "type";
		private const string SizeKey = "size";
		private const string MineralsSurfaceKey = "mineralsSurface";
		private const string MineralsDeepKey = "mineralsDeep";
 
		#endregion
	}
}
