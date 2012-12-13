using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Controllers.Data;
using System.Collections.ObjectModel;
using Stareater.Players;
using Stareater.Utils.Collections;
using System.Drawing;
using Stareater.Localization;
using Stareater.AppData;

namespace Stareater.Controllers
{
	public class NewGameController
	{
		PickList<Color> colors = new PickList<Color>(PlayerAssets.Colors);
		PickList<Organization> organizations = new PickList<Organization>(Organization.List);

		List<PlayerInfo> players = new List<PlayerInfo>();

		public NewGameController()
		{
			players.Add(new PlayerInfo("Marko Kovač",
				colors.Take(),
				null,
				localHuman));

			players.Add(new PlayerInfo("HAL Bot",
				colors.Take(),
				null,
				aiPlayers.Pick()));
		}

		public IList<PlayerInfo> PlayerList
		{
			get
			{
				return new ReadOnlyCollection<PlayerInfo>(players);
			}
		}

		public static bool CanCreateGame
		{
			get
			{
				return Organization.IsLoaded &&
					PlayerAssets.IsLoaded;
			}
		}

		private static PlayerType localHuman = new PlayerType(PlayerControlType.LocalHuman, Settings.Get.Language["PlayerTypes"]["localHuman"]);
		private static PickList<PlayerType> aiPlayers = new PickList<PlayerType>(new PlayerType[]{
			new PlayerType(PlayerControlType.LocalAI, Settings.Get.Language["PlayerTypes"]["AI"])
		});

		public static IEnumerable<PlayerType> PlayerTypes
		{
			get
			{
				Context context = Settings.Get.Language["PlayerTypes"];

				yield return localHuman;

				foreach(var aiType in aiPlayers.InnerList)
					yield return aiType;
			}
		}

		public void UpdatePlayer(int index, string newName)
		{
			players[index] = new PlayerInfo(
				newName,
				players[index].Color,
				players[index].Organization,
				players[index].ControlType);
		}

		public void UpdatePlayer(int index, Color newColor)
		{
			players[index] = new PlayerInfo(
				players[index].Name,
				newColor,
				players[index].Organization,
				players[index].ControlType);
		}

		public void UpdatePlayer(int index, Organization newOrganization)
		{
			players[index] = new PlayerInfo(
				players[index].Name,
				players[index].Color,
				newOrganization,
				players[index].ControlType);
		}

		public void UpdatePlayer(int index, PlayerType newType)
		{
			players[index] = new PlayerInfo(
				players[index].Name,
				players[index].Color,
				players[index].Organization,
				newType);
		}
	}
}
