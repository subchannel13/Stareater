using System;
using Stareater.GameData;
using Stareater.Localization;

namespace Stareater.Controllers.Views
{
	public class BuildingInfo
	{
		private const string LangContext = "Buildings";
		
		private readonly BuildingType building;
		
		public BuildingInfo(BuildingType building, double quantity)
		{
			this.building = building;
			this.Quantity = quantity;
		}
		
		public string Name 
		{
			get 
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(building.LanguageCode).Text();
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return building.ImagePath;
			}
		}
		
		public double Quantity { get; private set; }
	}
}
