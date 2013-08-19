using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{
	public class Prerequisite
	{
		private string code;
		private Formula level;
		
		public Prerequisite(string code, Formula level)
		{
			this.code = code;
			this.level = level;
		}
	}
}
