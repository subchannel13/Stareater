using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Stareater.GLData;

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
		
		const string AtlasImagePath = "./images/galaxyTextures.png";
		const string AtlasIkonPath = "./images/galaxyTextures.txt";
		
		const string AsteroidsTag = "asteroids";
		const string AtlasTag = "TextureAtlas";
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
			
			using (var textureImage = new Bitmap(AtlasImagePath))
				this.textureId = TextureUtils.CreateTexture(textureImage);
			
			IkonComposite ikonData;
			using(var ikonParser = new IkonParser(new StreamReader(AtlasIkonPath)))
				ikonData = ikonParser.ParseNext(AtlasTag).To<IkonComposite>();
			
			foreach(var name in ikonData.Keys)
				this.spriteNames[name] = new TextureInfo(textureId, ikonData[name].To<IkonArray>());

			/*
			 * If any sprite is missing, try running {repo root}/scripts/gen_textures.bat script.
			 */
			//TODO(v0.6) generate texture atlas here
			Asteroids = this.spriteNames[AsteroidsTag];
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
	}
}
