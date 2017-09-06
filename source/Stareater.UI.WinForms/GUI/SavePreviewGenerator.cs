using Ikadn.Ikon.Types;
using Stareater.Controllers;
using System.Linq;

namespace Stareater.GUI
{
	class SavePreviewGenerator
	{
		private GameController controller;

		public SavePreviewGenerator(GameController controller)
		{
			this.controller = controller;
		}

		public IkonBaseObject Make()
		{
			//TODO(v0.7) add star map data
			return new IkonInteger(this.controller.LocalHumanPlayers().First().Turn);
		}
	}
}
