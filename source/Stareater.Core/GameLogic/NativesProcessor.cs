using System.Linq;
using Stareater.GameData.Databases;
using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Utils.StateEngine;

namespace Stareater.GameLogic
{
	class NativesProcessor
	{
		private const string CatalyzerId = "catalyzer";
		private const long MaxCatalyzers = 5;

		[StateProperty]
		public Player OrganellePlayer { get; private set; }
		
		public NativesProcessor(Player organellePlayer)
		{
			this.OrganellePlayer = organellePlayer;
		}

		private NativesProcessor()
		{ }

		public void Initialize(StatesDB states, StaticsDB statics, TemporaryDB derivates)
		{
			this.OrganellePlayer.Intelligence.Initialize(states);
			foreach(var star in states.Stars)
				this.OrganellePlayer.Intelligence.StarFullyVisited(star, 0);

			foreach(var designData in statics.NativeDesigns)
				makeDesign(statics, states, designData.Key, designData.Value, derivates[this.OrganellePlayer]);
		}

		public void ProcessPrecombat(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			var catalizers = states.Fleets.OwnedBy[this.OrganellePlayer].SelectMany(x => x.Ships).Where(x => x.Design.IdCode == CatalyzerId).Sum(x => x.Quantity);
			if (catalizers < MaxCatalyzers)
			{
				var nativeDesign = states.Designs.OwnedBy[this.OrganellePlayer].First(x => x.IdCode == CatalyzerId);
				derivates[this.OrganellePlayer].SpawnShip(states.StareaterBrain, nativeDesign, 1, 0, new AMission[0], states);
			}
		}
		
		private void makeDesign(StaticsDB statics, StatesDB states, string id, PredefinedDesign designData, PlayerProcessor playerProc)
		{
			var design = states.Designs.FirstOrDefault(x => x.IdCode == id);
			if (design == null)
			{
				var armor = new Component<ArmorType>(statics.Armors[designData.Armor.IdCode], designData.Armor.Level);
				var hull = new Component<HullType>(statics.Hulls[designData.Hull.IdCode], designData.Hull.Level);
				var reactor = new Component<ReactorType>(statics.Reactors[designData.Reactor.IdCode], designData.Reactor.Level);
				var sensor = new Component<SensorType>(statics.Sensors[designData.Sensors.IdCode], designData.Sensors.Level);
				var thruster = new Component<ThrusterType>(statics.Thrusters[designData.Thrusters.IdCode], designData.Thrusters.Level);

				var isDrive = designData.IsDrive != null ? new Component<IsDriveType>(statics.IsDrives[designData.IsDrive.IdCode], designData.IsDrive.Level) : null;
				var shield = designData.Shield != null ? new Component<ShieldType>(statics.Shields[designData.Shield.IdCode], designData.Shield.Level) : null;
				var equipment = designData.MissionEquipment.Select(
					x => new Component<MissionEquipmentType>(statics.MissionEquipment[x.IdCode], x.Level, x.Amount)
				).ToList();
				var specials = designData.SpecialEquipment.Select(
					x => new Component<SpecialEquipmentType>(statics.SpecialEquipment[x.IdCode], x.Level, x.Amount)
				).ToList();

				design = new Design(
					id, playerProc.Player, false, true, designData.Name, designData.HullImageIndex,
					armor, hull, isDrive, reactor, sensor, shield, equipment, specials, thruster
				);
				
				design.CalcHash(statics);
				states.Designs.Add(design);
			}
			
			playerProc.Analyze(design, statics);
		}
	}
}
