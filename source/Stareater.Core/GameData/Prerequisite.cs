using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{
	public class Prerequisite
	{
		public string Code { get; private set; }
		public Formula Level { get; private set; }
		
		public Prerequisite(string code, Formula level)
		{
			this.Code = code;
			this.Level = level;
		}
	}
}
