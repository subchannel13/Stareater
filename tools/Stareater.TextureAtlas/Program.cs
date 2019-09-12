using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using Ikadn.Ikon.Types;
using System.Drawing;
using Ikadn;

namespace Stareater.TextureAtlas
{
	class Program
	{
		static readonly Dictionary<string, Bitmap> items = new Dictionary<string, Bitmap>();
		static readonly Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();
		static int margin = 4;
		static int width = 1024;
		static int heigth = 1024;
		static string outputIkonPath = "output.txt";
		static string outputImagePath = "output.png";
		const string compositeTag = "TextureAtlas";

		static void Main(string[] args)
		{
			if (args.Length < 1) {
				Console.WriteLine("Usage: TextureAtlas <folder> [options]");
				return;
			} else
				ReadParameters(args);

			var builder = new AtlasBuilder(
				items.Select(x => new KeyValuePair<string, Size>(x.Key, x.Value.Size)).ToArray(), 
				margin, new Size(width, heigth)
			);

			var atlasIkon = new IkonComposite(compositeTag);
			var atlasImage = new Bitmap(width, heigth);

			using(Graphics g = Graphics.FromImage(atlasImage))
				foreach (var x in builder.Build()) {
					Console.WriteLine(x.Key + " " + x.Value);
					string[] nameParams = x.Key.Split('-');

					var bounds = x.Value;
					foreach (var param in nameParams)
						if (param.StartsWith("hStretch"))
						{
							int amount;
							if (!int.TryParse(param.Substring("hStretch".Length), out amount))
								amount = 1;
							bounds.Inflate(-amount, 0);
						}
					
					var textureCoords = new IkonArray();
					textureCoords.Add(serializeRectangle(bounds.Left, bounds.Top));
					textureCoords.Add(serializeRectangle(bounds.Right, bounds.Top));
					textureCoords.Add(serializeRectangle(bounds.Right, bounds.Bottom));
					textureCoords.Add(serializeRectangle(bounds.Left, bounds.Bottom));

					atlasIkon.Add(nameParams[0], textureCoords);
					var destRect = new Rectangle(x.Value.Location, x.Value.Size);
					g.DrawImage(items[x.Key], destRect);
				}

			using (var outStream = new StreamWriter(outputIkonPath)) {
				var writer = new IkadnWriter(outStream);
				atlasIkon.Compose(writer);
			}

			atlasImage.Save(outputImagePath, ImageFormat.Png);
		}

		private static Ikadn.IkadnBaseObject serializeRectangle(float x, float y)
		{
			var result = new IkonArray();
			result.Add(new IkonNumeric(x / width));
			result.Add(new IkonNumeric(y / heigth));

			return result;
		}
		
		static void ReadParameters(string[] args)
		{
			var parameters = new Queue<string>(args);

			foreach (var file in new DirectoryInfo(parameters.Dequeue()).GetFiles()) {
				if (file.Extension.ToLower() != ".png" && file.Extension.ToLower() != ".bmp" && file.Extension.ToLower() != ".jpg")
					continue;

				string name = Path.GetFileNameWithoutExtension(file.Name);
				items.Add(name, new Bitmap(file.FullName));
				files.Add(name, file);
			}

			while (parameters.Count > 0) {
				string option = parameters.Dequeue().ToLower();
				switch (option) {
					case "-m":
						margin = int.Parse(parameters.Dequeue());
						break;
					case "-w":
						width = int.Parse(parameters.Dequeue());
						break;
					case "-h":
						heigth = int.Parse(parameters.Dequeue());
						break;
					case "-o":
						outputImagePath = parameters.Dequeue();
						outputIkonPath = parameters.Dequeue();
						break;
					case "-oi":
						outputIkonPath = parameters.Dequeue();
						break;
					case "-ot":
						outputImagePath = parameters.Dequeue();
						break;
					default:
						Console.WriteLine("Invalid option " + option);
						break;
				}
			}
		}
	}
}
