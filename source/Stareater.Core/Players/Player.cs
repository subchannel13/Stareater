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
		
		private IEnumerable<object> designs; //TODO: make type
		private IEnumerable<object> predefinedDesigns; //TODO: make type
		private IEnumerable<object> technologies; //TODO: make type
		private object intelligence; //TODO: make type

		private IEnumerable<object> messages; //TODO: make type
		private Dictionary<object, object> messageFilter; //TODO: make type

		public Player(string name, Color color, Organization organization, PlayerType type)
		{
			this.color = color;
			this.name = name;
			this.organization = organization;
			this.type = type;
		}
	}
}
