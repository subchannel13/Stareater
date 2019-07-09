using System;
using System.Drawing;

namespace Stareater.GLData
{
	interface ICharacterDrawer : IDisposable
	{
		Rectangle Draw(char c);
	}
}
