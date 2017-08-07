using System;
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
			return loadControl(rawData);
		}

		private static IOffscreenPlayer loadControl(IkadnBaseObject rawData)
		{
			var dataMap = rawData.To<IkonComposite>();
			
			if (dataMap.Tag.Equals(PlayerType.NoControllerTag))
				return null;
			else if (dataMap.Tag.Equals(PlayerType.AiControllerTag))
			{
				string factoryId = dataMap[PlayerType.FactoryIdKey].To<string>();
				//TODO(later) what if no factory was found?
				return PlayerAssets.AIDefinitions.First(x => x.Id == factoryId).Load(dataMap);
			}
			else if (dataMap.Tag.Equals(PlayerType.OrganelleControllerTag))
				return new OrganellePlayerFactory().Load(dataMap);
			
			//TODO(later) Invalid controller data
			return null;
		}
	}
}
