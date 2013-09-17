using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

using Stareater.AppData;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.Localization;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class NewGameController
	{
		const int MaxPlayers = 4;

		PickList<Color> colors = new PickList<Color>(PlayerAssets.Colors);
		PickList<Organization> organizations = new PickList<Organization>(Organization.List);
		PickList<PlayerType> aiPlayers = new PickList<PlayerType>();

		List<NewGamePlayerInfo> players = new List<NewGamePlayerInfo>();

		public NewGameController()
		{
			foreach (var aiFactory in PlayerAssets.AIDefinitions)
				aiPlayers.Add(new PlayerType(PlayerControlType.LocalAI, aiFactory));

			players.Add(new NewGamePlayerInfo("Marko Kovač",
				colors.Take(),
				null,
				localHuman));

			players.Add(new NewGamePlayerInfo(generateAiName(),
				colors.Take(),
				null,
				aiPlayers.Pick()));

			this.CustomStart = LastStartingCondition ?? DefaultStartingCondition;
			this.StarPositioner = MapAssets.StarPositioners[0];
			this.StarConnector = MapAssets.StarConnectors[0];
			this.StarPopulator = MapAssets.StarPopulators[0];
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

		public IList<NewGamePlayerInfo> PlayerList
		{
			get
			{
				return new ReadOnlyCollection<NewGamePlayerInfo>(players);
			}
		}

		public void UpdatePlayer(int index, string newName)
		{
			players[index] = new NewGamePlayerInfo(
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

			players[index] = new NewGamePlayerInfo(
				players[index].Name,
				newColor,
				players[index].Organization,
				players[index].ControlType);
		}

		public void UpdatePlayer(int index, Organization newOrganization)
		{
			players[index] = new NewGamePlayerInfo(
				players[index].Name,
				players[index].Color,
				newOrganization,
				players[index].ControlType);
		}

		public void UpdatePlayer(int index, PlayerType newType)
		{
			players[index] = new NewGamePlayerInfo(
				players[index].Name,
				players[index].Color,
				players[index].Organization,
				newType);
		}

		public void AddPlayer()
		{
			if (players.Count < MaxPlayers)
				players.Add(new NewGamePlayerInfo(generateAiName(),
					colors.Take(),
					null,
					aiPlayers.Pick()));
		}

		public void RemovePlayer(int index)
		{
			if (players.Count > 2)
				players.RemoveAt(index);
		}

		private static PlayerType localHuman = new PlayerType(PlayerControlType.LocalHuman, Settings.Get.Language["PlayerTypes"]["localHuman"].Text());
		#endregion

		#region Map
		public StartingConditions CustomStart { get; set; }
		public StartingConditions SelectedStart { get; set; }

		public IStarPositioner StarPositioner { get; set; }
		public IStarConnector StarConnector { get; set; }
		public IStarPopulator StarPopulator { get; set; }

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
