using System;
using System.Collections.Generic;
using Stareater.Localization.StarNames;

namespace Stareater.Galaxy
{
	public class StarNamer
	{
		Queue<IStarName> starNames = new Queue<IStarName>();
		
		public StarNamer(int starCount)
		{
			Settings
		}
		
		public IStarName NextName()
		{
			return starNames.Dequeue();
		}
	}
}
