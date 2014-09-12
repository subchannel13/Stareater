using System;
using Ikadn.Ikon.Types;

namespace Stareater.Controllers.Data
{
	public class SavedGameInfo
	{
		public string Title { get; private set; }
		public int Turn { get; private set; }

		public DateTime? LastModified { get; private set; }
		public IkonComposite RawData { get; private set; }

		public SavedGameInfo(string title, int turn)
		{
			this.Title = title;
			this.Turn = turn;

			this.LastModified = null;
			this.RawData = null;
		}

		public SavedGameInfo(string title, int turn, IkonComposite rawData, DateTime lastModified)
		{
			this.Title = title;
			this.Turn = turn;

			this.LastModified = lastModified;
			this.RawData = rawData;
		}
	}
}
