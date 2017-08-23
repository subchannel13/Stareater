using Ikadn.Ikon.Types;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	public interface ITypeStrategy
	{
		object Create(object originalValue);
		void FillCopy(object originalValue, object copyInstance, CopySession session);
		IEnumerable<object> Dependencies(object originalValue);
		IkonBaseObject Serialize(object originalValue, SaveSession session);
		object Deserialize(IkonBaseObject data, LoadSession session);
	}
}
