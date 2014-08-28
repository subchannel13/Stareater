using System;
using Ikadn.Ikon.Types;

namespace Stareater.Players.DefaultAI
{
	class DefaultAIPlayer : IOffscreenPlayer
	{
		public void PlayTurn()
		{
			//UNDONE
			//throw new NotImplementedException();
		}

		public void PlayBattle()
		{
			throw new NotImplementedException();
		}
		
		public Ikadn.IkadnBaseObject Save()
		{
			var data = new IkonComposite(PlayerType.AiControllerTag);
			data.Add(PlayerType.FactoryIdKey, new IkonText(DefaultAIFactory.FactoryId));
			
			return data;
		}
	}
}
