using System;
using Stareater.Galaxy;
using Ikadn.Ikon.Types;
using Stareater.Utils;

namespace Stareater.AppData
{
	public class LastGameInfo
	{
		public StartingConditions StartConditions { get; set; }
		public IkonArray StarPositionerConfig { get; set; }
		public IkonArray StarConnectorConfig { get; set; }
		public IkonArray StarPopulatorConfig { get; set; }

		public LastGameInfo()
		{
			this.StartConditions = null;
			this.StarPositionerConfig = null;
		}

		public LastGameInfo(IkonComposite ikstonData) : this()
		{
			this.StartConditions = ikstonData.ToOrDefault(StartingConditionsKey, x => new StartingConditions(x.To<IkonComposite>()), null);
			this.StarPositionerConfig = ikstonData.ToOrDefault(StarPositionerKey, x => x.To<IkonArray>(), null);
			this.StarConnectorConfig = ikstonData.ToOrDefault(StarConnectorKey, x => x.To<IkonArray>(), null);
			this.StarPopulatorConfig = ikstonData.ToOrDefault(StarPopulatorKey, x => x.To<IkonArray>(), null);
		}

		public IkonComposite BuildSaveData()
		{
			var lastGameData = new IkonComposite(ClassName);

			if (this.StartConditions != null)
				lastGameData.Add(StartingConditionsKey, this.StartConditions.BuildSaveData()); //TODO(v0.5) check if data is valid before loading
			
			if (this.StarPositionerConfig != null)
				lastGameData.Add(StarPositionerKey, this.StarPositionerConfig);
			
			if (this.StarConnectorConfig != null)
				lastGameData.Add(StarConnectorKey, this.StarConnectorConfig);
			
			if (this.StarPopulatorConfig != null)
				lastGameData.Add(StarPopulatorKey, this.StarPopulatorConfig);

			return lastGameData;
		}

		#region Attribute keys
		const string ClassName = "LastGame";
		const string StartingConditionsKey = "startingConditions";
		const string StarConnectorKey = "starConnector";
		const string StarPopulatorKey = "starPopulator";
		const string StarPositionerKey = "starPositioner";
		#endregion
	}
}
