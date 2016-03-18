using System;
using Stareater.Galaxy;
using Ikadn.Ikon.Types;
using Stareater.Utils;

namespace Stareater.AppData
{
	public class LastGameInfo
	{
		public StartingConditions StartConditions { get; set; }

		public LastGameInfo()
		{
			StartConditions = null;
		}

		public LastGameInfo(IkonComposite ikstonData) : this()
		{
			this.StartConditions = ikstonData.ToOrDefault(
				StartingConditionsKey,
				x => new StartingConditions(x.To<IkonComposite>()),
				this.StartConditions
			);
		}

		public IkonComposite BuildSaveData()
		{
			var lastGameData = new IkonComposite(ClassName);

			if (StartConditions != null)
				lastGameData.Add(StartingConditionsKey, StartConditions.BuildSaveData()); //TODO(v0.5) check if data is valid before loading

			return lastGameData;
		}

		#region Attribute keys
		const string ClassName = "LastGame";
		const string StartingConditionsKey = "StartinConditions";
		#endregion
	}
}
