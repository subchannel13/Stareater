using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class PlayerFormulaSet
	{
		public Formula Research { get; private set; }
		
		public PlayerFormulaSet(Formula research)
		{
			this.Research = research;
		}
	}
}
