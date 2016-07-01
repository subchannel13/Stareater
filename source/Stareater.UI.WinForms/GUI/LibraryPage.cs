using System;

namespace Stareater.GUI
{
	public class LibraryPage
	{
		public Func<int, string> Title { get; private set; }
		public Func<int, string> Text { get; private set; }
		public string ImagePath { get; private set; }
		public int MaxLevel { get; private set; }
		
		public LibraryPage(Func<int, string> title, Func<int, string> text, string imagePath, int maxLevel)
		{
			this.Title = title;
			this.Text = text;
			this.ImagePath = imagePath;
			this.MaxLevel = maxLevel;
		}
	}
}
