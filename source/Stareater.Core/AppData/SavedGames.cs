using System;
using System.Collections.Generic;
using Stareater.Controllers.Data;

namespace Stareater.AppData
{
	public class SavedGames
	{
		private static List<SavedGameData> games;
		
		public static IEnumerable<SavedGameData> Games
		{
			get
			{
				//TODO(v0.5) read list of files from folder
				yield return new SavedGameData("Test", 15);
			}
		}
	}
}
