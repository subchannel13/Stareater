using Ikadn.Ikon.Types;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Utils.StateEngine;

namespace Stareater.Players
{
	[StateBaseType("loadControl", typeof(Player))]
	public interface IOffscreenPlayer
	{
		PlayerController Controller { set; }
		
		void PlayTurn();
		IBattleEventListener StartBattle(SpaceBattleController controller);
		IBombardEventListener StartBombardment(BombardmentController controller);
	}
}
