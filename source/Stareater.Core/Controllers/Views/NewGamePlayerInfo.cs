using System;
using System.Linq;
using System.Drawing;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public struct NewGamePlayerInfo
	{
		public string Name;
		public Color Color;
		public Organization Organization;
		public PlayerType ControlType;

		public NewGamePlayerInfo(string Name, Color Color, Organization Organization,
			PlayerType Type)
		{
			this.Color = Color;
			this.Name = Name;
			this.Organization = Organization;
			this.ControlType = Type;
		}
	}
}
