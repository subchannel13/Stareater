using Ikadn.Ikon.Types;
using Stareater.Utils.StateEngine;

namespace Stareater.Players
{
	public interface IOffscreenPlayerFactory
	{
		string Id { get; }
		string Name { get; }
		
		IOffscreenPlayer Create();
		IOffscreenPlayer Load(IkonComposite rawData, LoadSession session);
	}
}
