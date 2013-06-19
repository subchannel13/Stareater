using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace Stareater.GLRenderers
{
	public class TextRenderUtil
	{
		#region Singleton
		static TextRenderUtil instance = null;

		public static TextRenderUtil Get
		{
			get
			{
				if (instance == null)
					instance = new TextRenderUtil();
				return instance;
			}
		}
		#endregion
		
		Dictionary<char, Vector2[]> characterPositions = new Dictionary<char, Vector2[]>();
		
		private TextRenderUtil()
		{ }
		
		public void Prepare(IEnumerable<string> texts)
		{ 
			HashSet<char> missinCharacters = new HashSet<char>();
			foreach(string text in texts)
				foreach(char c in text)
					if (!characterPositions.ContainsKey(c))
						missinCharacters.Add(c);					
				
			Bitmap textureBitmap;
			
			if (TextureManager.Get.FontTextureId == 0) {
				textureBitmap = new Bitmap(512, 512);
				TextureManager.Get.Load(TextureContext.Font, textureBitmap);
			}
		}
	}
}
