 
using System;
using System.Collections.Generic;
using System.Drawing;
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

		public Player(string name, Color color, PlayerControlType controlType, IOffscreenPlayer offscreenControl, ICollection<PredefinedDesign> unlockedDesigns, Intelligence intelligence) 
		{
			this.Name = name;
			this.Color = color;
			this.ControlType = controlType;
			this.OffscreenControl = offscreenControl;
			this.UnlockedDesigns = unlockedDesigns;
			this.Intelligence = intelligence;
 
		} 


		internal Player Copy()
		{
			return new Player(this.Name, this.Color, this.ControlType, this.OffscreenControl, this.UnlockedDesigns, this.Intelligence);
		}
 
	}
}
