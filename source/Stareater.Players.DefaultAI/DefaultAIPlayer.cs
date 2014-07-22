using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			return new IkonComposite("None");
		}
	}
}
