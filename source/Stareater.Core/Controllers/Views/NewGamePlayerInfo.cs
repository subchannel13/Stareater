using System.Drawing;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class NewGamePlayerInfo
	{
		public string Name { get; private set; }
		public Color Color { get; private set; }
		public Organization Organization { get; private set; }
		public PlayerType ControlType { get; private set; }

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
