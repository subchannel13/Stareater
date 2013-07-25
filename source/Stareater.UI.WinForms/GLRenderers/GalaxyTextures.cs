using System;
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
		
		const string AtlasImagePath = "./images/GalaxyTextures.png";
		const string AtlasIkonPath = "./images/GalaxyTextures.txt";
		
		const string AtlasTag = "TextureAtlas";
		const string SelectedStarTag = "selectedStar";
		const string StarColorTag = "starColor";
		const string StarGlowTag = "starGlow";
		const string SystemStarTag = "zoomedStar";
		
		private bool loaded = false;
		private int textureId;
		
		public TextureInfo StarColor { get; private set;}
		public TextureInfo StarGlow { get; private set;}
		public TextureInfo SelectedStar { get; private set;}
		public TextureInfo SystemStar { get; private set;}
		
		public void Load()
		{
			if (this.loaded)
				return;
			
			Bitmap textureImage = new Bitmap(AtlasImagePath);
			this.textureId = TextureUtils.Get.CreateTexture(textureImage);
			
			IkonParser ikonParser = new IkonParser(new StreamReader(AtlasIkonPath));
			IkonComposite ikonData = ikonParser.ParseNext(AtlasTag).To<IkonComposite>();
			
			SelectedStar = new TextureInfo(textureId, ikonData[SelectedStarTag].To<IkonArray>());
			StarColor = new TextureInfo(textureId, ikonData[StarColorTag].To<IkonArray>());
			StarGlow = new TextureInfo(textureId, ikonData[StarGlowTag].To<IkonArray>());
			SystemStar = new TextureInfo(textureId, ikonData[SystemStarTag].To<IkonArray>());
			
			ikonParser.Dispose();
			textureImage.Dispose();
			
			this.loaded = true;
		}
		
		public void Unload()
		{
			if (!loaded)
				return;
			
			TextureUtils.Get.DeleteTexture(textureId);
			this.textureId = 0;
			
			this.loaded = false;
		}
	}
}
