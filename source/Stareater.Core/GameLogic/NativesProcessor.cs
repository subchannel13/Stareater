using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class NativesProcessor
	{
		private const string CatalyzerId = "catalyzer";
		private const long MaxCatalyzers = 5;

		public Player OrganellePlayer { get; private set; }
		
		public NativesProcessor(Player organellePlayer)
		{
			this.OrganellePlayer = organellePlayer;
		}

		public void Initialize(StatesDB states, StaticsDB statics, TemporaryDB derivates)
		{
			this.OrganellePlayer.Intelligence.Initialize(states.Stars.Select(
					star => new Stareater.Galaxy.Builders.StarSystem(star, states.Planets.At[star].ToArray())
			));
			foreach(var star in states.Stars)
				this.OrganellePlayer.Intelligence.StarFullyVisited(star, 0);
			
			foreach(var template in statics.NativeDesigns)
				makeDesign(statics, states, template.Key, template.Value, derivates.Of(this.OrganellePlayer));
		}
		
		internal NativesProcessor Copy(PlayersRemap playersRemap)
		{
			return new NativesProcessor(playersRemap.Players[this.OrganellePlayer]);
		}

		public void ProcessPrecombat(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			var catalizers = states.Fleets.OwnedBy[this.OrganellePlayer].SelectMany(x => x.Ships).Where(x => x.Design.IdCode == CatalyzerId).Sum(x => x.Quantity);
			if (catalizers < MaxCatalyzers)
			{
				var nativeDesign = states.Designs.OwnedBy[this.OrganellePlayer].First(x => x.IdCode == CatalyzerId);
				
				var star = states.Stars.First();
				derivates.Of(this.OrganellePlayer).SpawnShip(star, nativeDesign, 1, new AMission[0], states);
			}
		}
		
		private void makeDesign(StaticsDB statics, StatesDB states, string id, PredefinedDesign predefDesign, PlayerProcessor playerProc)
		{
			var design = states.Designs.FirstOrDefault(x => x.IdCode == id);
			if (design == null)
			{
				var techLevels = new Var().
					Init(statics.DevelopmentTopics.Select(x => x.IdCode), false).
					Init(statics.ResearchTopics.Select(x => x.IdCode), false).Get;
					     
				var hull = statics.Hulls[predefDesign.HullCode].MakeHull(techLevels);
				var specials = predefDesign.SpecialEquipment.OrderBy(x => x.Key).Select(
					x => statics.SpecialEquipment[x.Key].MakeBest(techLevels, x.Value)
				).ToList();
				
				var armor = AComponentType.MakeBest(statics.Armors.Values, techLevels); //TODO(0.7) get id from template
				var reactor = ReactorType.MakeBest(techLevels, hull, specials, statics); //TODO(0.7) get id from template
				var isDrive = predefDesign.HasIsDrive ? IsDriveType.MakeBest(techLevels, hull, reactor, specials, statics) : null; //TODO(0.7) get id from template
				var sensor = AComponentType.MakeBest(statics.Sensors.Values, techLevels); //TODO(0.7) get id from template
				var shield = predefDesign.ShieldCode != null ? statics.Shields[predefDesign.ShieldCode].MakeBest(techLevels) : null;
				var equipment = predefDesign.MissionEquipment.Select(
					x => statics.MissionEquipment[x.Key].MakeBest(techLevels, x.Value)
				).ToList();
				
				var thruster = AComponentType.MakeBest(statics.Thrusters.Values, techLevels); //TODO(0.7) get id from template
	
				design = new Design(
					id, playerProc.Player, false, true, predefDesign.Name, predefDesign.HullImageIndex,
				    armor, hull, isDrive, reactor, sensor, shield, equipment, specials, thruster
				);
				
				design.CalcHash(statics);
				states.Designs.Add(design);
			}
			
			playerProc.Analyze(design, statics);
		}
	}
}
