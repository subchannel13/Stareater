using Ikadn.Ikon.Types;
using Stareater.Players;
using Stareater.Utils.Collections;
using System.Collections.Generic;
using System.Drawing;

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

		internal IkonComposite BuildSaveData()
		{
			return new IkonComposite("Player")
			{
				{NameKey, new IkonText(this.Name) },
				{OrganizationKey, new IkonText(this.Organization?.Data.IdCode ?? "") },
				{TypeKey, new IkonArray{
					new IkonInteger((int)this.ControlType.ControlType),
					new IkonText(this.ControlType?.OffscreenPlayerFactory?.Id ?? "")
				} }
			};
		}

		internal static NewGamePlayerInfo Load(IkonComposite data, PickList<Color> colors, Dictionary<string, OrganizationInfo> organizations, Dictionary<string, IOffscreenPlayerFactory> aiFactories)
		{
			var organizationId = data[OrganizationKey].To<string>();
			var playerData = data[TypeKey].To<IkonArray>();
			var playerType = (PlayerControlType)playerData[0].To<int>();
			var name = data[NameKey].To<string>();

			return new NewGamePlayerInfo(
				name,
				colors.Take(),
				!string.IsNullOrEmpty(organizationId) ? organizations[organizationId] : null,
				playerType == PlayerControlType.LocalHuman ?
					new PlayerType(playerType, name) :
					new PlayerType(playerType, aiFactories[playerData[1].To<string>()], name)
			);
		}

		#region Attribute keys
		const string NameKey = "name";
		const string OrganizationKey = "organization";
		const string TypeKey = "type";
		#endregion
	}
}
