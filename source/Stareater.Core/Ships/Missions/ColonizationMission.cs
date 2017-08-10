using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	[StateType(saveMethod: "Save", saveTag: MissionTag)]
	class ColonizationMission : AMission
	{
		[StateProperty]
		public Planet Target { get; private set; }
		
		public ColonizationMission(Planet target)
		{
			this.Target = target;
		}

		private ColonizationMission()
		{ }

		#region implemented abstract members of AMission
		public override void Accept(IMissionVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			return new ColonizationMission(galaxyRemap.Planets[this.Target]);
		}

		public override IkonBaseObject Save(SaveSession session)
		{
			var saveData = new IkonComposite(MissionTag);
			saveData.Add(TargetKey, session.Serialize(this.Target));

			return saveData;
		}

        public override IkadnBaseObject Save(ObjectIndexer indexer)
		{
			var saveData = new IkonComposite(MissionTag);
			saveData.Add(TargetKey, new IkonInteger(indexer.IndexOf(this.Target)));
			
			return saveData;
		}

		public static AMission Load(IkadnBaseObject rawData, LoadSession session)
		{
			return new ColonizationMission(session.Load<Planet>(rawData.To<IkonComposite>()[TargetKey]));
		}

		public override bool Equals(object obj)
		{
			var other = obj as ColonizationMission;
			return other != null && this.Target == other.Target;
		}
		public override int GetHashCode()
		{
			return this.Target.GetHashCode();
		}
		#endregion
		
		#region Saving keys
		public const string MissionTag = "Colonize";
		public const string TargetKey = "what";
 		#endregion
	}
}
