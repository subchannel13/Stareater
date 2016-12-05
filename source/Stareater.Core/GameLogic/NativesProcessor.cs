using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Ships;

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
			if (states.Designs.OwnedBy[this.OrganellePlayer].Count == 0)
			{
				var someDesign = states.Designs.First(); //TODO(v0.6) make predefined native designs
				var copy = new Design(
					states.MakeDesignId(), this.OrganellePlayer, false, true, "test", someDesign.ImageIndex,
					someDesign.Armor, someDesign.Hull, someDesign.IsDrive, someDesign.Reactor, someDesign.Sensors,
					someDesign.Shield, new List<Component<MissionEquipmentType>>(someDesign.MissionEquipment), 
					new List<Component<SpecialEquipmentType>>(someDesign.SpecialEquipment), someDesign.Thrusters);
				copy.CalcHash(statics);
				states.Designs.Add(copy);
				derivates.Of(this.OrganellePlayer).Analyze(copy, statics);
			}
		}
	}
}
