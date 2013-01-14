using System;
using System.Collections.Generic;
using Stareater.Maps;
using Ikon.Ston.Values;
using Ikon;

namespace Stareater.AppData
{
	public class LastGameInfo
	{
		public StartingConditions StartConditions { get; set; }

		public LastGameInfo()
		{
			StartConditions = null;
		}

		public LastGameInfo(ObjectValue ikstonData) : this()
		{
			if (ikstonData.Keys.Contains(StartingConditionsKey))
				StartConditions = new StartingConditions(ikstonData[StartingConditionsKey] as ObjectValue);
		}

		public ObjectValue BuildSaveData()
		{
			ObjectValue lastGameData = new ObjectValue(ClassName);

			if (StartConditions != null)
				lastGameData.Add(StartingConditionsKey, StartConditions.BuildSaveData());

			return lastGameData;
		}

		#region Attribute keys
		const string ClassName = "LastGame";
		const string StartingConditionsKey = "StartinConditions";
		#endregion
	}
}
