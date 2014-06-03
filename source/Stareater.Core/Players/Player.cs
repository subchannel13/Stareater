 
using System;
using System.Collections.Generic;
using System.Drawing;
using Stareater.GameData.Databases;
using Stareater.GameData;

namespace Stareater.Players 
{
	partial class Player 
	{
		public string Name { get; private set; }
		public Color Color { get; private set; }
		public PlayerControlType ControlType { get; private set; }
		public IOffscreenPlayer OffscreenControl { get; private set; }
		public ICollection<PredefinedDesign> UnlockedDesigns { get; private set; }
		public Intelligence Intelligence { get; private set; }
		public ChangesDB Orders { get;  set; }

		public Player(string name, Color color, PlayerType type) 
		{
			this.Name = name;
			this.Color = color;
			initPlayerControl(type);
			
			this.UnlockedDesigns = new HashSet<PredefinedDesign>();
			this.Intelligence = new Intelligence();
			this.Orders = new ChangesDB();
 
		} 

		private Player(Player original, GalaxyRemap galaxyRemap, string name, Color color) 
		{
			this.Name = original.Name;
			this.Color = original.Color;
			copyPlayerControl(original);
			
			copyDesigns(original);
			this.Intelligence = original.Intelligence.Copy(galaxyRemap);
			
 
		}

		internal Player Copy(GalaxyRemap galaxyRemap)
		{
			return new Player(this, galaxyRemap, this.Name, this.Color);
		}
 
	}
}
