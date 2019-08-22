using System;
using System.Drawing;

namespace Stareater.GLData
{
	interface ICharacterDrawer : IDisposable
	{
		CharTextureInfo Draw(char c);
	}
}
