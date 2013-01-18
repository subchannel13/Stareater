using System;
using System.Collections.Generic;
using Stareater.Controllers.Data;
using System.Collections.ObjectModel;
using Stareater.Players;
using Stareater.Utils.Collections;
using System.Drawing;
using Stareater.Localization;
using Stareater.AppData;
using Stareater.Maps;

namespace Stareater.Controllers
{
	public class NewGameController
	{
		const int MaxPlayers = 4;

		PickList<Color> colors = new PickList<Color>(PlayerAssets.Colors);
		PickList<Organization> organizations = new PickList<Organization>(Organization.List);
		PickList<PlayerType> aiPlayers = new PickList<PlayerType>();

		List<PlayerInfo> players = new List<PlayerInfo>();

		public NewGameController()
		{
			foreach (var aiFactory in PlayerAssets.AIDefinitions)
				aiPlayers.Add(new PlayerType(PlayerControlType.LocalAI, aiFactory));

			players.Add(new PlayerInfo("Marko Kovač",
				colors.Take(),
				null,
				localHuman));

			players.Add(new PlayerInfo(generateAiName(),
				colors.Take(),
				null,
				aiPlayers.Pick()));

			this.CustomStart = LastStartingCondition ?? DefaultStartingCondition;
			this.MapFactory = MapAssets.MapFactories[0];
		}

		private string generateAiName()
		{
			return "HAL Bot";
		}

		#region Players
		public IEnumerable<PlayerType> PlayerTypes
		{
			get
			{
				Context context = Settings.Get.Language["PlayerTypes"];

				yield return localHuman;

				foreach (var aiType in aiPlayers.InnerList)
					yield return aiType;
			}
		}

		public IList<PlayerInfo> PlayerList
		{
			get
			{
				return new ReadOnlyCollection<PlayerInfo>(players);
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
			if (!colors.InnerList.Contains(newColor))
				return;

			colors.InnerList.Add(players[index].Color);
			colors.InnerList.Remove(newColor);

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

		public void AddPlayer()
		{
			if (players.Count < MaxPlayers)
				players.Add(new PlayerInfo(generateAiName(),
					colors.Take(),
					null,
					aiPlayers.Pick()));
		}

		public void RemovePlayer(int index)
		{
			if (players.Count > 2)
				players.RemoveAt(index);
		}

		private static PlayerType localHuman = new PlayerType(PlayerControlType.LocalHuman, Settings.Get.Language["PlayerTypes"]["localHuman"]);
		#endregion

		#region Map
		public StartingConditions CustomStart { get; set; }

		public IMapFactory MapFactory { get; set; }

		public static StartingConditions DefaultStartingCondition
		{
			get { return MapAssets.Starts[MapAssets.Starts.Length / 2]; }
		}

		public static StartingConditions LastStartingCondition
		{
			get { return Settings.Get.LastGame.StartConditions; }
			set
			{
				Settings.Get.LastGame.StartConditions = value;
			}
		}

		public static readonly string CustomStartNameKey = "custom";
		#endregion

		public static bool CanCreateGame
		{
			get
			{
				return Organization.IsLoaded &&
					PlayerAssets.IsLoaded &&
					MapAssets.IsLoaded;
			}
		}
	}
}
