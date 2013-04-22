using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stareater.Players
{
	class Player
	{
		private string name;
		private Color color;
		private Organization organization;		
		private PlayerType type;
		
		private IEnumerable<object> designs;
		private IEnumerable<object> predefinedDesigns;
		private object intelligence;

		private IEnumerable<object> messages;
		private Dictionary<object, object> messageFilter;
	}
}
