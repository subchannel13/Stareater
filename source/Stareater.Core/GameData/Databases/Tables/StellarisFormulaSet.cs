using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class StellarisFormulaSet
	{
		public Formula ScanRange { get; private set; }

		public StellarisFormulaSet(Formula scanRange)
		{
			this.ScanRange = scanRange;
		}
	}
}
