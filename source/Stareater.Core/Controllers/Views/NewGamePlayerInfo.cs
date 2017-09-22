using System.Drawing;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class NewGamePlayerInfo
	{
		public string Name { get; private set; }
		public Color Color { get; private set; }
		public OrganizationInfo Organization { get; private set; }
		public PlayerType ControlType { get; private set; }

		internal NewGamePlayerInfo(string name, Color color, OrganizationInfo organization,
			PlayerType type)
		{
			this.Color = color;
			this.Name = name;
			this.Organization = organization;
			this.ControlType = type;
		}
	}
}
