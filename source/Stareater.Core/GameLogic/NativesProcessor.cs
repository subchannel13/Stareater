using System.Collections.Generic;
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

		[StatePropertyAttribute]
		public Player OrganellePlayer { get; private set; }
		
		[StatePropertyAttribute]
		private Dictionary<string, Design> nativeDesigns { get; set; }

		public NativesProcessor(Player organellePlayer)
		{
			this.nativeDesigns = new Dictionary<string, Design>();
			this.OrganellePlayer = organellePlayer;
		}

		private NativesProcessor()
		{ }

		public void Initialize(StatesDB states, StaticsDB statics, TemporaryDB derivates)
		{
			this.OrganellePlayer.Intelligence.Initialize(states);
			foreach(var star in states.Stars)
				this.OrganellePlayer.Intelligence.StarFullyVisited(star, 0);

			foreach (var designData in statics.NativeDesigns)
			{
				var design = makeDesign(statics, designData.Value, this.OrganellePlayer);
				
				this.nativeDesigns[designData.Key] = design;
				states.Designs.Add(design);
				derivates[this.OrganellePlayer].Analyze(design, statics);
			}
		}

		public void ProcessPrecombat(StatesDB states, TemporaryDB derivates)
		{
			var catalizers = states.Fleets.OwnedBy[this.OrganellePlayer].
				SelectMany(x => x.Ships).
				Where(x => x.Design == this.nativeDesigns[CatalyzerId]).
				Sum(x => x.Quantity);

			if (catalizers < MaxCatalyzers)
				derivates[this.OrganellePlayer].
					SpawnShip(states.StareaterBrain, this.nativeDesigns[CatalyzerId], 1, 0, new AMission[0], states);
		}
		
		private static Design makeDesign(StaticsDB statics, PredefinedDesign designData, Player player)
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

			return new Design(
				player, designData.Name, 0, designData.HullImageIndex, designData.UsesFuel,
				armor, hull, isDrive, reactor, sensor, thruster, shield, equipment, specials
			);
		}
	}
}
