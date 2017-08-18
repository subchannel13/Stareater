using System.Collections.Generic;
using System.Linq;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData.Ships;
using Stareater.Players.Natives;
using Stareater.Utils.StateEngine;

namespace Stareater.Players
{
	partial class Player
	{
		private Organization organization; //TODO(v0.7) add to type
		
		private void initPlayerControl(PlayerType type)
		{
			this.ControlType = type.ControlType;
			
			this.OffscreenControl = type.OffscreenPlayerFactory != null ? type.OffscreenPlayerFactory.Create() : null;
		}
		
		private void copyDesigns(Player original)
		{
			this.UnlockedDesigns = new HashSet<PredefinedDesign>(original.UnlockedDesigns);
		}
		
		private void copyPlayerControl(Player original)
		{
			this.ControlType = original.ControlType;
			this.OffscreenControl = null;
		}

		private static IOffscreenPlayer loadControl(IkadnBaseObject rawData, LoadSession session)
		{
			var tag = rawData.Tag as string;

			if (tag.Equals(PlayerType.NoControllerTag))
				return null;
			else if (tag.Equals(PlayerType.OrganelleControllerTag))
				return new OrganellePlayerFactory().Load(rawData.To<IkonComposite>(), session);
			else if (PlayerAssets.AIDefinitions.ContainsKey(tag))
				return PlayerAssets.AIDefinitions[tag].Load(rawData.To<IkonComposite>(), session);

			throw new KeyNotFoundException("Can't load player controller for " + tag);
		}
	}
}
