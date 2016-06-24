using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using OpenTK;

namespace Stareater.GLRenderers
{
	public class GalaxyTextures
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
		
		const string AtlasTag = "TextureAtlas";
		const string ColonizationMarkTag = "colonizationMark";
		const string ColonizationMarkColorTag = "colonizationMarkColor";
		const string FleetIndicatorTag = "fleetIndicator";
		const string MoveArrowTag = "moveArrow";
		const string PathLineTag = "wormholePath";
		const string PlanetTag = "planet";
		const string SelectedStarTag = "selectedStar";
		const string StarColorTag = "starColor";
		const string StarGlowTag = "starGlow";
		const string SystemStarTag = "zoomedStar";
		
		private bool loaded = false;
		private int textureId;
		private Dictionary<string, TextureInfo> sprites;
		
		public TextureInfo ColonizationMark { get; private set;}
		public TextureInfo ColonizationMarkColor { get; private set;}
		public TextureInfo FleetIndicator { get; private set;}
		public TextureInfo MoveToArrow { get; private set;}
		public TextureInfo Planet { get; private set;}
		public TextureInfo PathLine { get; private set;}
		public TextureInfo StarColor { get; private set;}
		public TextureInfo StarGlow { get; private set;}
		public TextureInfo SelectedStar { get; private set;}
		public TextureInfo SystemStar { get; private set;}
		
		public void Load()
		{
			if (this.loaded)
				return;
			
			var textureImage = new Bitmap(AtlasImagePath);
			this.textureId = TextureUtils.CreateTexture(textureImage);
			
			var ikonParser = new IkonParser(new StreamReader(AtlasIkonPath));
			var ikonData = ikonParser.ParseNext(AtlasTag).To<IkonComposite>();

			/*
			 * If any sprite is missing, try running {repo root}/scripts/gen_textures.bat script.
			 */
			ColonizationMark = new TextureInfo(textureId, ikonData[ColonizationMarkTag].To<IkonArray>());
			ColonizationMarkColor = new TextureInfo(textureId, ikonData[ColonizationMarkColorTag].To<IkonArray>());
			FleetIndicator = new TextureInfo(textureId, ikonData[FleetIndicatorTag].To<IkonArray>());
			MoveToArrow = new TextureInfo(textureId, ikonData[MoveArrowTag].To<IkonArray>());
			Planet = new TextureInfo(textureId, ikonData[PlanetTag].To<IkonArray>());
			PathLine = new TextureInfo(textureId, ikonData[PathLineTag].To<IkonArray>());
			SelectedStar = new TextureInfo(textureId, ikonData[SelectedStarTag].To<IkonArray>());
			StarColor = new TextureInfo(textureId, ikonData[StarColorTag].To<IkonArray>());
			StarGlow = new TextureInfo(textureId, ikonData[StarGlowTag].To<IkonArray>());
			SystemStar = new TextureInfo(textureId, ikonData[SystemStarTag].To<IkonArray>());
			
			this.sprites = new Dictionary<string, TextureInfo>();
			foreach(var name in ikonData.Keys)
				this.sprites.Add(name, new TextureInfo(textureId, ikonData[name].To<IkonArray>()));
			
			ikonParser.Dispose();
			textureImage.Dispose();
			
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
