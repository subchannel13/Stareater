using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Stareater.AppData;
using Stareater.Controllers.NewGameHelpers;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.GameData.Databases;
using Stareater.Localization;
using Stareater.Players;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class NewGameController
	{
		private const int MaxPlayers = 4;

		private readonly PickList<Color> colors = new PickList<Color>(PlayerAssets.Colors);
		private readonly PickList<OrganizationInfo> organizations = new PickList<OrganizationInfo>(PlayerAssets.Organizations);
		private readonly PickList<PlayerType> aiPlayers = new PickList<PlayerType>();

		private readonly List<NewGamePlayerInfo> players = new List<NewGamePlayerInfo>();

		private SystemEvaluator evaluator;
		internal StaticsDB Statics { get; private set; }

		public NewGameController(IEnumerable<TracableStream> staticDataSources)
		{
			foreach (var aiFactory in PlayerAssets.AIDefinitions.Values)
				aiPlayers.Add(new PlayerType(PlayerControlType.LocalAI, aiFactory));

			//TODO(v0.8) move to controller user
			players.Add(new NewGamePlayerInfo("Marko Kovač",
				colors.Take(),
				null,
				localHuman));

			players.Add(new NewGamePlayerInfo(generateAiName(),
				colors.Take(),
				null,
				aiPlayers.Pick()));

			this.Statics = StaticsDB.Load(staticDataSources);
			this.evaluator = new SystemEvaluator(this.Statics);
			foreach (var populator in MapAssets.StarPopulators)
				populator.SetGameData(this.Statics.PlanetTraits.Values);

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

		private IStarPopulator mStarPopulator = null;
		public IStarPopulator StarPopulator
		{
			get { return this.mStarPopulator; }
			set
			{
				this.mStarPopulator = value;

				this.BestSystemScore = this.mStarPopulator.MaxScore;
				this.WorstSystemScore = this.mStarPopulator.MinScore;
			}
		}

		public double BestSystemScore { get; private set; }
		public double WorstSystemScore { get; private set; }

		public MapPreview GeneratePreview(Random random)
		{
			var stars = this.StarPositioner.Generate(random, this.PlayerList.Count);
			var starlanes = this.StarConnector.Generate(random, stars);
			var systems = this.StarPopulator.Generate(random, this.evaluator, stars);

			var starIndices = new Dictionary<Vector2D, int>();
			for (int i = 0; i < stars.Stars.Length; i++)
				starIndices[stars.Stars[i]] = i;
			var starFromIndex = systems.ToDictionary(x => starIndices[x.Star.Position], x => x.Star);

			return new MapPreview(
				systems, 
				new HashSet<StarData>(stars.HomeSystems.Select(x => starFromIndex[x])),
				starlanes.Select(x => new Wormhole(starFromIndex[x.FromIndex], starFromIndex[x.ToIndex])),
				this.evaluator
			);
		}

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
