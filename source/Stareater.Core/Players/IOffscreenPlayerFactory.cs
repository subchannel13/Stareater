using System;
using Ikadn.Ikon.Types;

namespace Stareater.Players
{
	public interface IOffscreenPlayerFactory
	{
		string Id { get; }
		string Name { get; }
		
		IOffscreenPlayer Create();
		IOffscreenPlayer Load(IkonComposite rawData);
	}
}
