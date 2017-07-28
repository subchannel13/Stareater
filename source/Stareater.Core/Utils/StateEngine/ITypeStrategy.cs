using Ikadn.Ikon.Types;

namespace Stareater.Utils.StateEngine
{
	public interface ITypeStrategy
	{
        object Create(object originalValue);
        void FillCopy(object originalValue, object copyInstance, CopySession session);
		IkonBaseObject Serialize(object originalValue, SaveSession session);
	}
}
