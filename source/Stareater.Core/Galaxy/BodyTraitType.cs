using System;

namespace Stareater.Galaxy
{
	public class BodyTraitType
	{
		public string NameCode { get; private set; }
		public string DescriptionCode{ get; private set; }
		
		public string ImagePath { get; private set; }
		public string IdCode { get; private set; }
		
		public BodyTraitType(string nameCode, string descriptionCode, string imagePath, string idCode)
		{
			this.NameCode = nameCode;
			this.DescriptionCode = descriptionCode;
			this.ImagePath = imagePath;
			this.IdCode = idCode;
		}
	}
}
