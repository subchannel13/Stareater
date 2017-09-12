using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Stareater.GLData;
using Stareater.AppData;

namespace Stareater.GLRenderers
{
	class GalaxyTextures
	{
		#region Singleton
		static GalaxyTextures instance = null;

		public static GalaxyTextures Get
		{
			get
			{
				if (instance == null)
					instance = new GalaxyTextures();
				return instance;
			}
		}
		#endregion
		
		private GalaxyTextures()
		{ }
		
		const int SpriteMargin = 4;
		const string AtlasImagePath = "./images/galaxyTextures.png";
		const string AtlasIkonPath = "./images/galaxyTextures.txt";
		const string SpritesPath = "./images/";

		const string AtlasTag = "TextureAtlas";
		
		const string AsteroidsTag = "asteroids";
		const string BombButtonTag = "bombard";
		const string ColonizationMarkTag = "colonizationMark";
		const string ColonizationMarkColorTag = "colonizationMarkColor";
		const string FleetIndicatorTag = "fleetIndicator";
		const string GasGiantTag = "gasGiant";
		const string MoveArrowTag = "moveArrow";
		const string PathLineTag = "wormholePath";
		const string RockPlanetTag = "rockPlanet";
		const string SelectedStarTag = "selectedStar";
		const string StarColorTag = "starColor";
		const string StarGlowTag = "starGlow";
		const string SystemStarTag = "zoomedStar";
		
		private bool loaded = false;
		private int textureId;
		private Dictionary<string, TextureInfo> spriteNames  = new Dictionary<string, TextureInfo>();
		
		public TextureInfo Asteroids { get; private set;}
		public TextureInfo BombButton { get; private set;}
		public TextureInfo ColonizationMark { get; private set;}
		public TextureInfo ColonizationMarkColor { get; private set;}
		public TextureInfo FleetIndicator { get; private set;}
		public TextureInfo GasGiant { get; private set;}
		public TextureInfo MoveToArrow { get; private set;}
		public TextureInfo PathLine { get; private set;}
		public TextureInfo RockPlanet { get; private set;}
		public TextureInfo StarColor { get; private set;}
		public TextureInfo StarGlow { get; private set;}
		public TextureInfo SelectedStar { get; private set;}
		public TextureInfo SystemStar { get; private set;}
		
		public void Load()
		{
			if (this.loaded)
				return;
			
			var ikonData = loadAtlas();
			
			foreach(var name in ikonData.Keys)
				this.spriteNames[name] = new TextureInfo(textureId, ikonData[name].To<IkonArray>());

			/*
			 * If any sprite is missing, try running {repo root}/scripts/gen_textures.bat script.
			 */
			Asteroids = this.spriteNames[AsteroidsTag];
			BombButton  = this.spriteNames[BombButtonTag];
			ColonizationMark = this.spriteNames[ColonizationMarkTag];
			ColonizationMarkColor = this.spriteNames[ColonizationMarkColorTag];
			FleetIndicator = this.spriteNames[FleetIndicatorTag];
			GasGiant = this.spriteNames[GasGiantTag];
			MoveToArrow = this.spriteNames[MoveArrowTag];
			PathLine = this.spriteNames[PathLineTag];
			RockPlanet = this.spriteNames[RockPlanetTag];
			SelectedStar = this.spriteNames[SelectedStarTag];
			StarColor = this.spriteNames[StarColorTag];
			StarGlow = this.spriteNames[StarGlowTag];
			SystemStar = this.spriteNames[SystemStarTag];
			
			this.loaded = true;
		}
		
		public void Unload()
		{
			if (!loaded)
				return;
			
			TextureUtils.DeleteTexture(textureId);
			this.textureId = 0;
			
			this.loaded = false;
		}
		
		public TextureInfo Sprite(string spriteName)
		{
			if (!this.spriteNames.ContainsKey(spriteName))
			{
				var file = new FileInfo(spriteName);
				this.spriteNames.Add(spriteName, this.spriteNames[file.Name]);
			}
			
			return this.spriteNames[spriteName];
		}

		private IkonComposite loadAtlas()
		{
			var atlasFile = new FileInfo(AtlasImagePath);
			var metadataFile = new FileInfo(AtlasIkonPath);
			var rootFolder = SettingsWinforms.Get.DataRootPath ?? "";
            var extraSprites = new DirectoryInfo(rootFolder + SpritesPath).
				GetFiles().
				Where(x => x.Name != atlasFile.Name && x.Name != metadataFile.Name).
				ToList();
			
			IkonComposite ikonData;
			using(var ikonParser = new IkonParser(new StreamReader(rootFolder + AtlasIkonPath)))
				ikonData = ikonParser.ParseNext(AtlasTag).To<IkonComposite>();

			using (var atlasImage = new Bitmap(rootFolder + AtlasImagePath))
			{
				if (extraSprites.Any())
				{
#if DEBUG
					System.Diagnostics.Trace.WriteLine("Adding loose sprites to atlas: " + string.Join(" ", extraSprites.Select(x => x.Name)));
#endif
					var atlasBuilder = new AtlasBuilder(ikonData, SpriteMargin, atlasImage.Size);
					using(Graphics g = Graphics.FromImage(atlasImage))
					{
						g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
						g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None; 

						foreach(var spriteFile in extraSprites)
							using(var sprite = new Bitmap(spriteFile.FullName))
							{
								var spriteRegion = atlasBuilder.Add(sprite.Size);
								g.DrawImage(sprite, spriteRegion);

								var textureCoords = new IkonArray();
								textureCoords.Add(serializeSpriteCorner(spriteRegion.Left, spriteRegion.Top, atlasImage.Size));
								textureCoords.Add(serializeSpriteCorner(spriteRegion.Right, spriteRegion.Top, atlasImage.Size));
								textureCoords.Add(serializeSpriteCorner(spriteRegion.Right, spriteRegion.Bottom, atlasImage.Size));
								textureCoords.Add(serializeSpriteCorner(spriteRegion.Left, spriteRegion.Bottom, atlasImage.Size));
								ikonData.Add(Path.GetFileNameWithoutExtension(spriteFile.Name), textureCoords);
							}
					}
				}
				this.textureId = TextureUtils.CreateTexture(atlasImage);
			}
			
			return ikonData;
		}

		Ikadn.IkadnBaseObject serializeSpriteCorner(int x, int y, Size atlasSize)
		{
			var result = new IkonArray();
			result.Add(new IkonFloat(x / (double)atlasSize.Width));
			result.Add(new IkonFloat(y / (double)atlasSize.Height));

			return result;
		}
	}
}
