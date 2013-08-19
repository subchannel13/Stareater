using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;

namespace Stareater.Galaxy
{
	abstract class AConstructionSite
	{
		private Player owner;
		private IEnumerable<object> buildings; //TODO: make type
		private IEnumerable<object> orderQueue; //TODO: make type
		private object leftovers; //TODO: make type
	}
}
