using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class StellarisFormulaSet
	{
		public Formula ScanRange { get; private set; }
		public Formula StarlaneDiscoveryDifficulty { get; internal set; }

		public StellarisFormulaSet(Formula scanRange, Formula starlaneStealth)
		{
			this.ScanRange = scanRange;
			this.StarlaneDiscoveryDifficulty = starlaneStealth;
		}
	}
}
