using Stareater.Galaxy;
using Ikadn.Ikon.Types;
using Stareater.Utils;
using System;

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

		public IkonComposite BuildSaveData()
		{
			var lastGameData = new IkonComposite(ClassName);

			if (this.StartConditions != null)
				lastGameData.Add(StartingConditionsKey, this.StartConditions.BuildSaveData());
			
			if (this.StarPositionerConfig != null)
				lastGameData.Add(StarPositionerKey, this.StarPositionerConfig);
			
			if (this.StarConnectorConfig != null)
				lastGameData.Add(StarConnectorKey, this.StarConnectorConfig);
			
			if (this.StarPopulatorConfig != null)
				lastGameData.Add(StarPopulatorKey, this.StarPopulatorConfig);

			return lastGameData;
		}

		internal static LastGameInfo Load(IkonComposite ikstonData)
		{
			var conditions = ikstonData.ToOrDefault(StartingConditionsKey, x => StartingConditions.Load(x.To<IkonComposite>()), null);
			if (conditions == null)
				return null;

			var info = new LastGameInfo
			{
				StartConditions = conditions,
				StarPositionerConfig = ikstonData.ToOrDefault(StarPositionerKey, x => x.To<IkonArray>(), null),
				StarConnectorConfig = ikstonData.ToOrDefault(StarConnectorKey, x => x.To<IkonArray>(), null),
				StarPopulatorConfig = ikstonData.ToOrDefault(StarPopulatorKey, x => x.To<IkonArray>(), null)
			};

			return info;
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
