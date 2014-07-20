using System;

namespace Stareater.Controllers.Data
{
	public class SavedGameData
	{
		public string Title { get; private set; }
		public int Turn { get; private set; }

		public SavedGameData(string title, int turn)
		{
			this.Title = title;
			this.Turn = turn;
		}
	}
}
