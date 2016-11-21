using System;

namespace Stareater.GLData
{
	interface IGlProgram
	{
		int ProgramId { get; }
		void SetupAttributes();
	}
}
