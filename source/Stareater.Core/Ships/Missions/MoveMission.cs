using System;
using System.Linq;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Ships.Missions
{
	class MoveMission : AMission
	{
		public StarData Destination { get; private set; }
		public Wormhole UsedWormhole { get; private set; }
		
		public MoveMission(StarData destination, Wormhole usedWormhole)
		{
			this.Destination = destination;
			this.UsedWormhole = usedWormhole;
		}
		
		public override void Accept(IMissionVisitor visitor)
		{
			visitor.Visit(this);
		}
		
		public override bool Equals(object obj)
		{
			var other = obj as MoveMission;
			return other != null && this.Destination == other.Destination && this.UsedWormhole == other.UsedWormhole;
		}
		
		public override int GetHashCode()
		{
			return this.Destination.GetHashCode();
		}
		
		public override AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			return new MoveMission(this.Destination, this.UsedWormhole);
		}
		
		public override Ikadn.IkadnBaseObject Save(ObjectIndexer indexer)
		{
			var saveData = new IkonComposite(MissionTag);
			saveData.Add(DestinationKey, new IkonInteger(indexer.IndexOf(this.Destination)));
			
			if (this.UsedWormhole != null)
				saveData.Add(WormholeKey, new IkonInteger(indexer.IndexOf(this.UsedWormhole)));
			
			return saveData;
		}
		
		public static AMission Load(Ikadn.IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			var dataStruct = rawData as IkonComposite;
			var destination = deindexer.Get<StarData>(dataStruct[DestinationKey].To<int>());
			Wormhole usedWormhole = null;
			
			if (dataStruct.Keys.Contains(WormholeKey))
				usedWormhole = deindexer.Get<Wormhole>(dataStruct[WormholeKey].To<int>());
			
			return new MoveMission(destination, usedWormhole);
		}
		
		#region Saving keys
		public const string MissionTag = "Move";
		private const string DestinationKey = "to";
		private const string WormholeKey = "via";
 		#endregion
	}
}
