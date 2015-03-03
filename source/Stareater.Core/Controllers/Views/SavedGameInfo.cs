using System;
using Ikadn.Ikon.Types;
using System.IO;

namespace Stareater.Controllers.Views
{
	public class SavedGameInfo
	{
		public string Title { get; private set; }
		public int Turn { get; private set; }

		internal FileInfo FileInfo { get; private set; }
		internal IkonComposite RawData { get; private set; }

		internal SavedGameInfo(string title, int turn, IkonComposite rawData, FileInfo fileInfo)
		{
			this.Title = title;
			this.Turn = turn;

			this.FileInfo = fileInfo;
			this.RawData = rawData;
		}
	}
}
