using System;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Ships.Missions
{
	class StationaryMission : AMission
	{
		public StarData Location { get; private set; }
		
		public StationaryMission(StarData location)
		{
			this.Location = location;
		}
		
		public override MissionType Type {
			get {
				return MissionType.Stationary;
			}
		}
		
		public override bool Equals(object obj)
		{
			var other = obj as StationaryMission;
			return other != null && this.Location == other.Location;
		}

		public override int GetHashCode()
		{
			return Location.GetHashCode();
		}
		
		public override AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			return new StationaryMission(galaxyRemap.Stars[this.Location]);
		}
		
		public override Ikadn.IkadnBaseObject Save(ObjectIndexer indexer)
		{
			var saveData = new IkonComposite(MissionTag);
			
			saveData.Add(LocationKey, new IkonInteger(indexer.IndexOf(this.Location)));
			
			return saveData;
		}
		
		public static AMission Load(Ikadn.IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			var dataStruct = rawData as IkonComposite;
			return new StationaryMission(deindexer.Get<StarData>(dataStruct[LocationKey].To<int>()));
		}
		
		#region Saving keys
		public const string MissionTag = "Stay";
		private const string LocationKey = "where";
 		#endregion
	}
}
