using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

using Ikadn.Ikon.Types;
using Stareater.AppData;
using Stareater.Controllers.NewGameHelpers;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.Localization;
using Stareater.Players;
using Stareater.Utils.Collections;
using Stareater.Utils.PluginParameters;

namespace Stareater.Controllers
{
	public class NewGameController
	{
		private const int MaxPlayers = 4;

		private readonly PickList<Color> colors = new PickList<Color>(PlayerAssets.Colors);
		private readonly PickList<OrganizationInfo> organizations = new PickList<OrganizationInfo>(PlayerAssets.Organizations);
		private readonly PickList<PlayerType> aiPlayers = new PickList<PlayerType>();

		private readonly List<NewGamePlayerInfo> players = new List<NewGamePlayerInfo>();

		public NewGameController()
		{
			foreach (var aiFactory in PlayerAssets.AIDefinitions.Values)
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
			this.StarPositioner = ParameterLoadingVisitor.Load(Settings.Get.LastGame.StarPositionerConfig, MapAssets.StarPositioners);
			this.StarConnector = ParameterLoadingVisitor.Load(Settings.Get.LastGame.StarConnectorConfig, MapAssets.StarConnectors);
			this.StarPopulator = ParameterLoadingVisitor.Load(Settings.Get.LastGame.StarPopulatorConfig, MapAssets.StarPopulators);
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
				Context context = LocalizationManifest.Get.CurrentLanguage["PlayerTypes"];

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

		public void UpdatePlayer(int index, OrganizationInfo newOrganization)
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
		
		public void ShufflePlayers(Random random)
		{
			var oldPlayers = new PickList<NewGamePlayerInfo>(random, this.players);
			this.players.Clear();
			
			while(oldPlayers.InnerList.Count > 0)
				this.players.Add(oldPlayers.Take());
		}

		private static PlayerType localHuman = new PlayerType(PlayerControlType.LocalHuman, LocalizationManifest.Get.CurrentLanguage["PlayerTypes"]["localHuman"].Text());
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
				return PlayerAssets.IsLoaded && MapAssets.IsLoaded;
			}
		}

		internal static Organization Resolve(OrganizationInfo selection, Random rng)
		{
			if (selection == null)
				return PlayerAssets.OrganizationsRaw[rng.Next(PlayerAssets.OrganizationsRaw.Length)];

			return selection.Data;
		}

		internal void SaveLastGame()
		{
			Settings.Get.LastGame.StartConditions = this.SelectedStart;
			var saver = new ParameterSavingVisitor();
			Settings.Get.LastGame.StarPositionerConfig = saver.Save(this.StarPositioner);
			Settings.Get.LastGame.StarConnectorConfig = saver.Save(this.StarConnector);
			Settings.Get.LastGame.StarPopulatorConfig = saver.Save(this.StarPopulator);
		}
	}
}
