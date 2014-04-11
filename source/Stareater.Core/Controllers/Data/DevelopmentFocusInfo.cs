using System;
using Stareater.GameData.Databases.Tables;

namespace Stareater.Controllers.Data
{
	public class DevelopmentFocusInfo
	{
		private DevelopmentFocus focusData;
		
		internal DevelopmentFocusInfo(DevelopmentFocus focusData)
		{
			this.focusData = focusData;
		}
	}
}
