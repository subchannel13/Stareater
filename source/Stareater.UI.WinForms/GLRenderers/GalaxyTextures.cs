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
		private Dictionary<string, SpriteInfo> spriteNames  = new Dictionary<string, SpriteInfo>();
		private Dictionary<string, int> spriteIndices  = new Dictionary<string, int>();
		private VertexArray vertexArray;
		
		public SpriteInfo Asteroids { get; private set;}
		public SpriteInfo ColonizationMark { get; private set;}
		public SpriteInfo ColonizationMarkColor { get; private set;}
		public SpriteInfo FleetIndicator { get; private set;}
		public SpriteInfo GasGiant { get; private set;}
		public SpriteInfo MoveToArrow { get; private set;}
		public SpriteInfo PathLine { get; private set;}
		public SpriteInfo RockPlanet { get; private set;}
		public SpriteInfo StarColor { get; private set;}
		public SpriteInfo StarGlow { get; private set;}
		public SpriteInfo SelectedStar { get; private set;}
		public SpriteInfo SystemStar { get; private set;}
		
		public void Load()
		{
			if (this.loaded)
				return;
			
			using (var textureImage = new Bitmap(AtlasImagePath))
				this.textureId = TextureUtils.CreateTexture(textureImage);
			
			IkonComposite ikonData;
			using(var ikonParser = new IkonParser(new StreamReader(AtlasIkonPath)))
				ikonData = ikonParser.ParseNext(AtlasTag).To<IkonComposite>();
			
			var vaoBuilder = new VertexArrayBuilder();
			var textures = new Dictionary<string, TextureInfo>();
			int spriteIndex = 0;
			foreach(var name in ikonData.Keys)
			{
				var spriteTexture = new TextureInfo(textureId, ikonData[name].To<IkonArray>());
				textures[name] = spriteTexture;
				spriteIndices[name] = spriteIndex;
				spriteIndex++;
				
				vaoBuilder.BeginObject();
				vaoBuilder.AddTexturedRect(spriteTexture);
				vaoBuilder.EndObject();
			}
			
			this.vertexArray = vaoBuilder.Generate(ShaderLibrary.Sprite);
			foreach(var name in ikonData.Keys)
				this.spriteNames.Add(name, new SpriteInfo(this.vertexArray, spriteIndices[name], textures[name]));
			
			/*
			 * If any sprite is missing, try running {repo root}/scripts/gen_textures.bat script.
			 */
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
		
		public SpriteInfo Sprite(string spriteName)
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
