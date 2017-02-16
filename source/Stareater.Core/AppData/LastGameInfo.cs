using System;
using Stareater.Galaxy;
using Ikadn.Ikon.Types;
using Stareater.Utils;

namespace Stareater.AppData
{
	public class LastGameInfo
	{
		public StartingConditions StartConditions { get; set; }
		public IkonComposite StarPositionerConfig { get; set; }

		public LastGameInfo()
		{
			this.StartConditions = null;
			this.StarPositionerConfig = null;
		}

		public LastGameInfo(IkonComposite ikstonData) : this()
		{
			this.StartConditions = ikstonData.ToOrDefault(StartingConditionsKey, x => new StartingConditions(x.To<IkonComposite>()), null);
			this.StarPositionerConfig = ikstonData.ToOrDefault(StarPositionerKey, x => x.To<IkonComposite>(), null);
		}

		public IkonComposite BuildSaveData()
		{
			var lastGameData = new IkonComposite(ClassName);

			if (this.StartConditions != null)
				lastGameData.Add(StartingConditionsKey, this.StartConditions.BuildSaveData()); //TODO(v0.5) check if data is valid before loading
			
			if (this.StarPositionerConfig != null)
				lastGameData.Add(StarPositionerKey, this.StarPositionerConfig);

			return lastGameData;
		}

		#region Attribute keys
		const string ClassName = "LastGame";
		const string StartingConditionsKey = "startingConditions";
		const string StarPositionerKey = "starPositioner";
		#endregion
	}
}
