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

namespace Stareater.GameLogic
{
	class NativesProcessor
	{
		public Player OrganellePlayer { get; private set; }
		
		public NativesProcessor(Player organellePlayer)
		{
			this.OrganellePlayer = organellePlayer;
		}
		
		internal NativesProcessor Copy(PlayersRemap playersRemap)
		{
			return new NativesProcessor(playersRemap.Players[this.OrganellePlayer]);
		}

		public void ProcessPrecombat(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			var nativeDesign = states.Designs.OwnedBy[this.OrganellePlayer].FirstOrDefault();
			
			if (nativeDesign == null)
			{
				var someDesign = states.Designs.First(x => x.IsDrive != null); //TODO(v0.6) make predefined native designs
				nativeDesign = new Design(
					states.MakeDesignId(), this.OrganellePlayer, false, true, "test", someDesign.ImageIndex,
					someDesign.Armor, someDesign.Hull, someDesign.IsDrive, someDesign.Reactor, someDesign.Sensors,
					someDesign.Shield, new List<Component<MissionEquipmentType>>(someDesign.MissionEquipment), 
					new List<Component<SpecialEquipmentType>>(someDesign.SpecialEquipment), someDesign.Thrusters);
				nativeDesign.CalcHash(statics);
				states.Designs.Add(nativeDesign);
				derivates.Of(this.OrganellePlayer).Analyze(nativeDesign, statics);
			}
			
			var star = states.Stars.First();
			derivates.Of(this.OrganellePlayer).SpawnShip(star, nativeDesign, 1, new AMission[0], states);
		}
	}
}
