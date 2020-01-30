using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using OpenTK;
using OpenTK.Graphics;
using Stareater.AppData;
using Stareater.Galaxy;

namespace Stareater.GLData
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
		const string ButtonBackgroundTag = "buttonBackground";
		const string ColonizationMarkTag = "colonizationMark";
		const string ColonizationMarkColorTag = "colonizationMarkColor";
		const string FleetIndicatorTag = "fleetIndicator";
		const string GasGiantTag = "gasGiant";
		const string IntroStareaterOutlineTag = "introStareaterOutline";
		const string IntroStareaterUnderlineTag = "introStareaterUnderline";
		const string MainMenuTag = "mainMenu";
		const string MoveArrowTag = "moveArrow";
		const string PathLineTag = "wormholePath";
		const string RockPlanetTag = "rockPlanet";
		const string SelectedStarTag = "selectedStar";
		const string StarColorTag = "starColor";
		const string StarGlowTag = "starGlow";
		const string SystemStarTag = "zoomedStar";
		
		private bool loaded = false;
		private int textureId;
		private readonly Dictionary<string, TextureInfo> spriteNames  = new Dictionary<string, TextureInfo>();

		public Vector2 Size { get; private set; }

		public TextureInfo Asteroids { get; private set;}
		public TextureInfo Blank { get; private set; }
		public TextureInfo BombButton { get; private set;}
		public TextureInfo ButtonBackground { get; private set; }
		public TextureInfo ButtonHover { get; private set; }
		public TextureInfo ButtonNormal { get; private set; }
		public TextureInfo Colonization { get; private set; }
		public TextureInfo ColonizationMark { get; private set;}
		public TextureInfo ColonizationMarkColor { get; private set;}
		public TextureInfo Design { get; private set; }
		public TextureInfo Development { get; private set; }
		public TextureInfo Diplomacy { get; private set; }
		public TextureInfo EndTurnHover { get; private set; }
		public TextureInfo EndTurnNormal { get; private set; }
		public TextureInfo FleetIndicator { get; private set;}
		public TextureInfo GasGiant { get; private set;}
		public TextureInfo IntroStareaterOutline { get; private set; }
		public TextureInfo IntroStareaterUnderline { get; private set; }
		public TextureInfo Library { get; private set; }
		public TextureInfo MainMenu { get; private set; }
		public TextureInfo MoveToArrow { get; private set;}
		public TextureInfo NewReports { get; private set; }
		public TextureInfo PanelBackground { get; private set; }
		public TextureInfo PathLine { get; private set;}
		public TextureInfo Radar { get; private set; }
		public TextureInfo Reports { get; private set; }
		public TextureInfo Research { get; private set; }
		public TextureInfo RockPlanet { get; private set;}
		public TextureInfo StarColor { get; private set;}
		public TextureInfo StarGlow { get; private set;}
		public TextureInfo SelectedStar { get; private set;}
		public TextureInfo Stareater { get; private set; }
		public TextureInfo SystemStar { get; private set;}
		public TextureInfo ToggleHover { get; private set; }
		public TextureInfo ToggleNormal { get; private set; }
		public TextureInfo ToggleToggled { get; private set; }

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
			this.Asteroids = this.spriteNames[AsteroidsTag];
			this.Blank = this.spriteNames["blank"];
			this.BombButton = this.spriteNames[BombButtonTag];
			this.ButtonBackground = this.spriteNames[ButtonBackgroundTag];
			this.ButtonHover = this.spriteNames["buttonHover"];
			this.ButtonNormal = this.spriteNames["buttonNormal"];
			this.Colonization = this.spriteNames["colonization"];
			this.ColonizationMark = this.spriteNames[ColonizationMarkTag];
			this.ColonizationMarkColor = this.spriteNames[ColonizationMarkColorTag];
			this.Design = this.spriteNames["design"];
			this.Diplomacy = this.spriteNames["diplomacy"];
			this.Development = this.spriteNames["development"];
			this.EndTurnHover = this.spriteNames["endTurnHover"];
			this.EndTurnNormal = this.spriteNames["endTurnNormal"];
			this.FleetIndicator = this.spriteNames[FleetIndicatorTag];
			this.GasGiant = this.spriteNames[GasGiantTag];
			this.IntroStareaterOutline = this.spriteNames[IntroStareaterOutlineTag];
			this.IntroStareaterUnderline = this.spriteNames[IntroStareaterUnderlineTag];
			this.Library = this.spriteNames["library"];
			this.MainMenu = this.spriteNames[MainMenuTag];
			this.NewReports = this.spriteNames["newReports"];
			this.MoveToArrow = this.spriteNames[MoveArrowTag];
			this.PanelBackground = this.spriteNames["panelBackground"];
			this.PathLine = this.spriteNames[PathLineTag];
			this.Radar = this.spriteNames["radar"];
			this.Reports = this.spriteNames["reports"];
			this.Research = this.spriteNames["research"];
			this.RockPlanet = this.spriteNames[RockPlanetTag];
			this.SelectedStar = this.spriteNames[SelectedStarTag];
			this.Stareater = this.spriteNames["stareater"];
			this.StarColor = this.spriteNames[StarColorTag];
			this.StarGlow = this.spriteNames[StarGlowTag];
			this.SystemStar = this.spriteNames[SystemStarTag];
			this.ToggleHover = this.spriteNames["toggleHover"];
			this.ToggleNormal = this.spriteNames["toggleUntoggled"];
			this.ToggleToggled = this.spriteNames["toggleToggled"];

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

		public TextureInfo PlanetSprite(PlanetType type)
		{
			switch (type)
			{
				case PlanetType.Asteriod:
					return GalaxyTextures.Get.Asteroids;
				case PlanetType.GasGiant:
					return GalaxyTextures.Get.GasGiant;
				case PlanetType.Rock:
					return GalaxyTextures.Get.RockPlanet;
				default:
					throw new NotImplementedException("No sprite for " + type);
			}
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

								var textureCoords = new IkonArray
								{
									serializeSpriteCorner(spriteRegion.Left, spriteRegion.Top, atlasImage.Size),
									serializeSpriteCorner(spriteRegion.Right, spriteRegion.Top, atlasImage.Size),
									serializeSpriteCorner(spriteRegion.Right, spriteRegion.Bottom, atlasImage.Size),
									serializeSpriteCorner(spriteRegion.Left, spriteRegion.Bottom, atlasImage.Size)
								};
								ikonData.Add(Path.GetFileNameWithoutExtension(spriteFile.Name), textureCoords);
							}
					}
				}

				//TODO(later) move to atlas generator
				var atlasData = new ColorMap(atlasImage);
				for (int y = 0; y < atlasData.Height; y++)
					for (int x = 0; x < atlasData.Width; x++)
						if (atlasData[x, y].A == 0)
						{
							var colorSum = new Vector4();
							var alphaSum = 0f;
							foreach (var color in atlasData.Subregion(x - 1, y - 1, x + 1, y + 1))
							{
								colorSum += new Vector4(color.R, color.G, color.B, 1) * color.A;
								alphaSum += color.A;
							}

							if (alphaSum > 0)
							{
								colorSum /= alphaSum;
								atlasData[x, y] = new Color4(colorSum.X, colorSum.Y, colorSum.Z, 0);
							}
						}

				this.textureId = TextureUtils.CreateTexture(atlasData);
				this.Size = new Vector2(atlasImage.Width, atlasImage.Height);
			}
			
			return ikonData;
		}

		Ikadn.IkadnBaseObject serializeSpriteCorner(int x, int y, Size atlasSize)
		{
			var result = new IkonArray
			{
				new IkonFloat(x / (double)atlasSize.Width),
				new IkonFloat(y / (double)atlasSize.Height)
			};

			return result;
		}
	}
}
