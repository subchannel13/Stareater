using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.Galaxy;

namespace Stareater.Players
{
	class Player
	{
		public string Name { get; private set; }
		public Color Color { get; private set; }
		private Organization organization;
		public PlayerControlType ControlType { get; private set; }
		public IOffscreenPlayer OffscreenControl { get; private set; }
		
		private IEnumerable<object> designs; //TODO: make type
		private IEnumerable<object> predefinedDesigns; //TODO: make type
		public Intelligence Intelligence { get; private set; }

		private IEnumerable<object> messages; //TODO: make type
		private Dictionary<object, object> messageFilter; //TODO: make type

		public ChangesDB Orders { get; internal set; }
		
		public Player(string name, Color color, Organization organization, PlayerType type)
		{
			this.Color = color;
			this.Name = name;
			this.organization = organization;
			this.ControlType = type.ControlType;
			
			if (type.OffscreenPlayerFactory != null)
				this.OffscreenControl = type.OffscreenPlayerFactory.Create();
			else
				this.OffscreenControl = null;
			
			this.Intelligence = new Intelligence();
			
			this.Orders = new ChangesDB();
		}

		public Player()
		{ }

		public Player Copy(GalaxyRemap galaxyRemap)
		{
			Player copy = new Player();

			copy.Name = this.Name;
			copy.Color = this.Color;
			copy.organization = this.organization;
			copy.ControlType = this.ControlType;
			copy.OffscreenControl = null;

			copy.designs = null; //TODO: make type
			copy.predefinedDesigns = null; //TODO: make type
			copy.Intelligence  = this.Intelligence.Copy(galaxyRemap);

			copy.messages = null; //TODO: make type
			copy.messageFilter = null; //TODO: make type

			copy.Orders = null; //Handled later

			return copy;
		}
	}
}
