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
	partial class Player
	{
		private Organization organization;
		
		private IEnumerable<object> messages; //TODO(v0.5): make type
		private Dictionary<object, object> messageFilter; //TODO(v0.5): make type
		
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
			
			this.UnlockedDesigns = new HashSet<PredefinedDesign>();
			this.Intelligence = new Intelligence();
			
			this.Orders = new ChangesDB();
		}

		public Player()
		{ }
		
		private void initPlayerControl(PlayerType type)
		{
			this.ControlType = type.ControlType;
			
			if (type.OffscreenPlayerFactory != null)
				this.OffscreenControl = type.OffscreenPlayerFactory.Create();
			else
				this.OffscreenControl = null;
		}
		
		private void copyDesigns(Player original)
		{
			this.UnlockedDesigns = new HashSet<PredefinedDesign>(original.UnlockedDesigns);
		}
		
		private void copyPlayerControl(Player original)
		{
			this.ControlType = original.ControlType;
			this.OffscreenControl = null;
		}
	}
}
