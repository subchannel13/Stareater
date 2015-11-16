 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.GameData;

namespace Stareater.Galaxy 
{
	public class Wormhole 
	{
		public StarData FromStar { get; private set; }
		public StarData ToStar { get; private set; }

		public Wormhole(StarData fromStar, StarData toStar) 
		{
			this.FromStar = fromStar;
			this.ToStar = toStar;
 
			 
		} 


		private Wormhole(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var fromStarSave = rawData[FromStarKey];
			this.FromStar = deindexer.Get<StarData>(fromStarSave.To<int>());

			var toStarSave = rawData[ToStarKey];
			this.ToStar = deindexer.Get<StarData>(toStarSave.To<int>());
 
			 
		}

		internal Wormhole Copy(GalaxyRemap galaxyRemap) 
		{
			return new Wormhole(galaxyRemap.Stars[this.FromStar], galaxyRemap.Stars[this.ToStar]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(FromStarKey, new IkonInteger(indexer.IndexOf(this.FromStar)));

			data.Add(ToStarKey, new IkonInteger(indexer.IndexOf(this.ToStar)));
			return data;
 
		}

		public static Wormhole Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new Wormhole(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "Wormhole";
		private const string FromStarKey = "from";
		private const string ToStarKey = "to";
 
		#endregion

 
	}
}
