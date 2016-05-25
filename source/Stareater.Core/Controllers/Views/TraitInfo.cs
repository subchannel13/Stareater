using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views
{
	public class TraitInfo
	{
		private readonly BodyTraitType data;
		
		internal TraitInfo(BodyTraitType data)
		{
			this.data = data;
		}
		
		public string ImagePath 
		{
			get
			{
				return data.ImagePath;
			}
		}
	}
}
