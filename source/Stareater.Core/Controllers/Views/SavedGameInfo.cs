using Ikadn.Ikon.Types;
using System.IO;

namespace Stareater.Controllers.Views
{
	public class SavedGameInfo
	{
		public string Title { get; private set; }
		public IkonBaseObject PreviewData { get; private set; }

		internal FileInfo FileInfo { get; private set; }

		internal SavedGameInfo(string title, IkonBaseObject previewData, FileInfo fileInfo)
		{
			this.Title = title;
			this.PreviewData = previewData;

			this.FileInfo = fileInfo;
		}
	}
}
