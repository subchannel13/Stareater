using System;
using System.Collections.Generic;
using System.Linq;
using Ikadn.Ikon.Types;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Ships.Missions
{
	class MoveMission : AMission
	{
		public IEnumerable<Vector2D> Waypoints { get; private set; }
		
		public MoveMission(IEnumerable<Vector2D> waypoints)
		{
			this.Waypoints = waypoints;
		}
		
		public override MissionType Type {
			get {
				return MissionType.Move;
			}
		}
		
		public override AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			return new MoveMission(this.Waypoints);
		}
		
		public override Ikadn.IkadnBaseObject Save(ObjectIndexer indexer)
		{
			var saveData = new IkonComposite(MissionTag);
			var waypointData = new IkonArray();
			
			foreach (var point in this.Waypoints)
				waypointData.Add(new IkonArray(new Ikadn.IkadnBaseObject[] {
					new IkonFloat(point.X), new IkonFloat(point.Y)
				}));
			
			saveData.Add(WaypointsKey, waypointData);
			
			return saveData;
		}
		
		public static AMission Load(Ikadn.IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			var dataStruct = rawData as IkonComposite;
			var waypoints = dataStruct[WaypointsKey].To<IEnumerable<IList<Ikadn.IkadnBaseObject>>>().
				Select(x => new Vector2D(x[0].To<double>(), x[1].To<double>())).
				ToArray();
			
			return new MoveMission(waypoints);
		}
		
		#region Saving keys
		public const string MissionTag = "Move";
		private const string WaypointsKey = "path";
 		#endregion
	}
}
