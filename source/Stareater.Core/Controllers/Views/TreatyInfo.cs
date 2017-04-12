using System;
using Stareater.Localization;

namespace Stareater.Controllers.Views
{
	public class TreatyInfo
	{
		public string Name 
		{
			get { return LocalizationManifest.Get.CurrentLanguage["Treaties"]["war"].Text(); }
		}
	}
}
