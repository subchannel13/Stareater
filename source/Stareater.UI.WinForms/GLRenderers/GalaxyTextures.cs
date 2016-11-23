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
		private Dictionary<string, TextureInfo> sprites;
		private VertexArray vertexArray;
		
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
		public SpriteInfo SelectedStar { get; private set;}
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
			
			this.sprites = new Dictionary<string, TextureInfo>();
			var spriteIndices = new Dictionary<string, int>();
			var vaoBuilder = new VertexArrayBuilder();
			int spriteIndex = 0;
			foreach(var name in ikonData.Keys)
			{
				var spriteTexture = new TextureInfo(textureId, ikonData[name].To<IkonArray>());
				this.sprites.Add(name, spriteTexture);
				spriteIndices[name] = spriteIndex;
				spriteIndex++;
				
				vaoBuilder.BeginObject();
				vaoBuilder.AddTexturedRect(spriteTexture);
				vaoBuilder.EndObject();
			}
			this.vertexArray = vaoBuilder.Generate(ShaderLibrary.Sprite);

			/*
			 * If any sprite is missing, try running {repo root}/scripts/gen_textures.bat script.
			 */
			Asteroids = new TextureInfo(textureId, ikonData[AsteroidsTag].To<IkonArray>());
			ColonizationMark = new TextureInfo(textureId, ikonData[ColonizationMarkTag].To<IkonArray>());
			ColonizationMarkColor = new TextureInfo(textureId, ikonData[ColonizationMarkColorTag].To<IkonArray>());
			FleetIndicator = new TextureInfo(textureId, ikonData[FleetIndicatorTag].To<IkonArray>());
			GasGiant = new TextureInfo(textureId, ikonData[GasGiantTag].To<IkonArray>());
			MoveToArrow = new TextureInfo(textureId, ikonData[MoveArrowTag].To<IkonArray>());
			PathLine = new TextureInfo(textureId, ikonData[PathLineTag].To<IkonArray>());
			RockPlanet = new TextureInfo(textureId, ikonData[RockPlanetTag].To<IkonArray>());
			SelectedStar = new SpriteInfo(this.vertexArray, textureId, spriteIndices[SelectedStarTag]);
			StarColor = new TextureInfo(textureId, ikonData[StarColorTag].To<IkonArray>());
			StarGlow = new TextureInfo(textureId, ikonData[StarGlowTag].To<IkonArray>());
			SystemStar = new TextureInfo(textureId, ikonData[SystemStarTag].To<IkonArray>());
			
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
			if (!this.sprites.ContainsKey(spriteName))
			{
				var file = new FileInfo(spriteName);
				this.sprites.Add(spriteName, this.sprites[file.Name]);
			}
			
			return this.sprites[spriteName];
		}
	}
}
