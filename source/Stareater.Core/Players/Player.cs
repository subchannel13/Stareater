using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stareater.Players
{
	public class Player
	{
		public string Name { get; private set; }
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
			this.Name = name;
			this.organization = organization;
			this.type = type;
		}
	}
}
