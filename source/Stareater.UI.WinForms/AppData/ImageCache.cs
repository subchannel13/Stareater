using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
		
		private readonly Dictionary<string, Image> cache = new Dictionary<string, Image>();
		private readonly Dictionary<string, Image> disabledCache = new Dictionary<string, Image>();
		private readonly Dictionary<Tuple<string, Size>, Image> resized = new Dictionary<Tuple<string, Size>, Image>();
		
		public Image this[string path] 
		{
			get 
			{
				if (cache.ContainsKey(path))
					return cache[path];

				var root = SettingsWinforms.Get.DataRootPath ?? "";
                var file = new FileInfo(root + path);
				if (file.Extension == "")
					file = new FileInfo(root + path + ".png");

				Image image;
				if (cache.ContainsKey(file.FullName)) {
					image = cache[file.FullName];
					cache.Add(path, image);
					
					return image;
				}
				
				image = Image.FromFile(file.FullName);
				
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
			var disabledImage = new Bitmap(enabledImage.Width, enabledImage.Height);

			var matrix = new ColorMatrix { Matrix33 = 0.4f };

			var attributes = new ImageAttributes();
        	attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

        	using(var g = Graphics.FromImage(disabledImage))
        		g.DrawImage(
        			enabledImage, 
        			new Rectangle(0, 0, disabledImage.Width, disabledImage.Height), 
        			0, 0, disabledImage.Width, disabledImage.Height, 
        			GraphicsUnit.Pixel, attributes
        		);
        	
        	var file = new FileInfo(path);
        	if (file.FullName != path)
					disabledCache.Add(file.FullName, disabledImage);
        	disabledCache.Add(path, disabledImage);
        	
        	return disabledImage;
		}
		
		public Image Resized(string path, Size boundigSize)
		{
			var key = new Tuple<string, Size>(path, boundigSize);
			
			if (this.resized.ContainsKey(key))
				return this.resized[key];
			
			var baseImage = this[path];
			var xRatio = baseImage.Width / (double) boundigSize.Width;
			var yRatio = baseImage.Height / (double) boundigSize.Height;
			
			var width = (int)Math.Round((xRatio > yRatio) ? boundigSize.Width : (baseImage.Width / yRatio));
			var height = (int)Math.Round((xRatio > yRatio) ? (baseImage.Height / xRatio) : boundigSize.Height);
				
			var destRect = new Rectangle(0, 0, width, height);
		    var destImage = new Bitmap(width, height);
		
		    using (var graphics = Graphics.FromImage(destImage))
		    {
		        graphics.CompositingMode = CompositingMode.SourceCopy;
		        graphics.CompositingQuality = CompositingQuality.HighQuality;
		        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		        graphics.SmoothingMode = SmoothingMode.HighQuality;
		        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
		
		        using (var wrapMode = new ImageAttributes())
		        {
		            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
		            graphics.DrawImage(baseImage, destRect, 0, 0, baseImage.Width,baseImage.Height, GraphicsUnit.Pixel, wrapMode);
		        }
		    }
		    
		    this.resized[key] = destImage;
		    return destImage;
		}
	}
}
