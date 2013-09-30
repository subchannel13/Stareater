using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
		Dictionary<string, Image> disabledCache = new Dictionary<string, Image>();
		
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
		
		public Image Disabled(string path)
		{
			if (disabledCache.ContainsKey(path))
				return disabledCache[path];
			
			var enabledImage = this[path];
			Bitmap disabledImage = new Bitmap(enabledImage.Width, enabledImage.Height);

        	ColorMatrix matrix = new ColorMatrix();
        	matrix.Matrix33 = 0.4f;

        	ImageAttributes attributes = new ImageAttributes();
        	attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

        	using(var g = Graphics.FromImage(disabledImage))
        		g.DrawImage(
        			enabledImage, 
        			new Rectangle(0, 0, disabledImage.Width, disabledImage.Height), 
        			0, 0, disabledImage.Width, disabledImage.Height, 
        			GraphicsUnit.Pixel, attributes
        		);
        	
        	FileInfo file = new FileInfo(path);
        	if (file.FullName != path)
					disabledCache.Add(file.FullName, disabledImage);
        	disabledCache.Add(path, disabledImage);
        	
        	return disabledImage;
		}
	}
}
