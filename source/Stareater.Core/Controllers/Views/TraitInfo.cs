using Stareater.Galaxy.BodyTraits;

namespace Stareater.Controllers.Views
{
	public class TraitInfo
	{
		private readonly TraitType data;
		
		internal TraitInfo(TraitType data)
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
