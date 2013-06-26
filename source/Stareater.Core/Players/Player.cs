using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stareater.Players
{
	public class Player
	{
		private string name;
		private Color color;
		private Organization organization;		
		private PlayerType type;
		
		private IEnumerable<object> designs;
		private IEnumerable<object> predefinedDesigns;
		private IEnumerable<object> technologies;
		private object intelligence;

		private IEnumerable<object> messages;
		private Dictionary<object, object> messageFilter;

		public Player(string name, Color color, Organization organization, PlayerType type)
		{
			this.color = color;
			this.name = name;
			this.organization = organization;
			this.type = type;
		}
	}
}
