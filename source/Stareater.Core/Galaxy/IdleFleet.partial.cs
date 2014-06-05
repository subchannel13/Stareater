using System;
using Stareater.GameData;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater.Galaxy
{
	partial class IdleFleet
	{
		private void copyShips(IdleFleet original, PlayersRemap playersRemap)
		{
			foreach(var group in original.Ships)
				this.Ships.Add(group.Copy(playersRemap));
		}
	}
}
