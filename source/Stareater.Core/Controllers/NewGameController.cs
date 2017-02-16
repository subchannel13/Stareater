using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

using Ikadn.Ikon.Types;
using Stareater.AppData;
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
		const int MaxPlayers = 4;

		PickList<Color> colors = new PickList<Color>(PlayerAssets.Colors);
		PickList<Organization> organizations = new PickList<Organization>(PlayerAssets.Organizations);
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
			this.StarPositioner = loalBuilderConfig(Settings.Get.LastGame.StarPositionerConfig, MapAssets.StarPositioners);
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

		#region
		internal void SaveLastGame()
		{
			Settings.Get.LastGame.StartConditions = this.SelectedStart;
			Settings.Get.LastGame.StarPositionerConfig = saveBuilderConfig(this.StarPositioner);
		}

		//TODO(v0.6) may fail if format is incorrect
		private static IkonComposite saveBuilderConfig<T>(T generator) where T : IMapBuilderPiece
		{
			var data = new IkonComposite(generator.Code);
			
			var selectors = new IkonArray();
			foreach(var selector in generator.Parameters.Selectors)
				selectors.Add(new IkonInteger(selector.Value));
			data.Add(SelectorParamKey, selectors);
			
			var intRanges = new IkonArray();
			foreach(var range in generator.Parameters.IntegerRanges)
				intRanges.Add(new IkonInteger(range.Value));
			data.Add(IntRangeParamKey, intRanges);
			
			var realRanges = new IkonArray();
			foreach(var range in generator.Parameters.RealRanges)
				realRanges.Add(new IkonFloat(range.Value));
			data.Add(RealRangeParamKey, realRanges);
			
			return data;
		}

		private static T loalBuilderConfig<T>(IkonComposite config, T[] generators) where T : IMapBuilderPiece
		{
			if (config == null || generators.All(x => x.Code != config.Tag as string))
				return generators[0];
			
			var generator = generators.First(x => x.Code == config.Tag as string);
			
			var data = config[SelectorParamKey].To<IkonArray>();
			for(int i = 0; i < data.Count; i++)
				generator.Parameters.Selectors[i].Value = data[i].To<int>();
			
			data = config[IntRangeParamKey].To<IkonArray>();
			for(int i = 0; i < data.Count; i++)
				generator.Parameters.IntegerRanges[i].Value = data[i].To<int>();
			
			data = config[RealRangeParamKey].To<IkonArray>();
			for(int i = 0; i < data.Count; i++)
				generator.Parameters.RealRanges[i].Value = data[i].To<double>();
			
			return generator;
		}
		
		private const string SelectorParamKey = "selectors";
		private const string IntRangeParamKey = "intRanges";
		private const string RealRangeParamKey = "realRanges";
		#endregion
	}
}
