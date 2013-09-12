using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Stareater.AppData
{
	public class ImageCache
	{
		#region Singleton
		static ImageCache instance = null;

		public static ImageCache Get
		{
			get
			{
				if (instance == null)
					instance = new ImageCache();
				return instance;
			}
		}
		#endregion
		
		Dictionary<string, Image> cache = new Dictionary<string, Image>();
		
		public Image this[string path] 
		{
			get 
			{
				if (cache.ContainsKey(path))
					return cache[path];
				
				FileInfo file = new FileInfo(path);
				Image image;
				
				if (cache.ContainsKey(file.FullName)) {
					image = cache[file.FullName];
					cache.Add(path, image);
					
					return image;
				}
				
				image = Image.FromFile(path);
				
				if (file.FullName != path)
					cache.Add(file.FullName, image);
				cache.Add(path, image);
				
				return image;
			}
		}
	}
}
