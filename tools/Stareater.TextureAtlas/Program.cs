using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using Ikadn.Ikon.Types;
using System.Drawing;
using Ikadn;

namespace Stareater.TextureAtlas
{
	class Program
	{
		static Dictionary<string, Bitmap> items = new Dictionary<string, Bitmap>();
		static Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();
		static int margin = 4;
		static int width = 512;
		static int heigth = 512;
		static string outputIkonPath = "output.txt";
		static string outputImagePath = "output.png";
		static string compositeTag = "TextureAtlas";

		static void Main(string[] args)
		{
			if (args.Length < 1) {
				Console.WriteLine("Usage: TextureAtlas <folder> [options]");
				return;
			} else
				ReadParameters(args);

			AtlasBuilder builder = new AtlasBuilder(
				items.Select(x => new KeyValuePair<string, Size>(x.Key, x.Value.Size)).ToArray(), 
				margin, new Size(width, heigth)
			);

			IkonComposite atlasIkon = new IkonComposite(compositeTag);
			Bitmap atlasImage = new Bitmap(width, heigth);

			using(Graphics g = Graphics.FromImage(atlasImage))
				foreach (var x in builder.Build()) {
					Console.WriteLine(x.Key + " " + x.Value);

					IkonArray textureCoords = new IkonArray();
					textureCoords.Add(serializeRectangle(x.Value.Left, x.Value.Top));
					textureCoords.Add(serializeRectangle(x.Value.Left, x.Value.Bottom));
					textureCoords.Add(serializeRectangle(x.Value.Right, x.Value.Bottom));
					textureCoords.Add(serializeRectangle(x.Value.Right, x.Value.Top));

					atlasIkon.Add(x.Key, textureCoords);
					g.DrawImage(items[x.Key], x.Value.Location);
				}

			using (var outStream = new StreamWriter(outputIkonPath)) {
				IkadnWriter writer = new IkadnWriter(outStream);
				atlasIkon.Compose(writer);
			}

			atlasImage.Save(outputImagePath, ImageFormat.Png);
		}

		private static Ikadn.IkadnBaseObject serializeRectangle(float x, float y)
		{
			IkonArray result = new IkonArray();
			result.Add(new IkonNumeric(x / width));
			result.Add(new IkonNumeric(y / heigth));

			return result;
		}
		
		static void ReadParameters(string[] args)
		{
			Queue<string> parameters = new Queue<string>(args);

			foreach (var file in new DirectoryInfo(parameters.Dequeue()).GetFiles()) {
				if (file.Extension.ToLower() != ".png" && file.Extension.ToLower() != ".bmp" && file.Extension.ToLower() != ".jpg")
					continue;

				string name = Path.GetFileNameWithoutExtension(file.Name);
				items.Add(name, new Bitmap(file.FullName));
				files.Add(name, file);
			}

			margin = 8;
		}
	}
}
