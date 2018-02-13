using Stareater.GameLogic;

namespace Stareater.Controllers.Views.Ships
{
	public class AbilityInfo
	{
		internal AbilityStats Data { get; private set; }
		internal int Index { get; private set; }
		public double Quantity { get; private set; }
		
		internal AbilityInfo(AbilityStats data, int index, double quantity)
		{
			this.Data = data;
			this.Index = index;
			this.Quantity = quantity;
		}
		
		public string ImagePath 
		{
			get
			{
				return this.Data.Type.ImagePath;
			}
		}
		
		public int Range 
		{ 
			get { return this.Data.Range; }
		}
	}
}
