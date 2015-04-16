using System;
using System.Collections.Generic;
using System.Linq;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData.Ships;

namespace Stareater.Players
{
	partial class Player
	{
		private Organization organization; //TODO(later): add to type
		private Dictionary<object, object> messageFilter; //TODO(v0.5): make type
		
		private void initPlayerControl(PlayerType type)
		{
			this.ControlType = type.ControlType;
			
			if (type.OffscreenPlayerFactory != null)
				this.OffscreenControl = type.OffscreenPlayerFactory.Create();
			else
				this.OffscreenControl = null;
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
		
		private static IOffscreenPlayer loadControl(IkadnBaseObject rawData)
		{
			var dataMap = rawData.To<IkonComposite>();
			
			if (dataMap.Tag.Equals(PlayerType.NoControllerTag))
				return null;
			else if (dataMap.Tag.Equals(PlayerType.AiControllerTag))
			{
				string factoryId = dataMap[PlayerType.FactoryIdKey].To<string>();
				//TODO: what if no factory was found?
				return PlayerAssets.AIDefinitions.Where(x => x.Id == factoryId).First().Load(dataMap);
			}
			
			//TODO: Invalid controller data
			return null;
		}
	}
}
