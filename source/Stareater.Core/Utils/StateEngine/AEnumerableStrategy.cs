using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	abstract class AEnumerableStrategy : ITypeStrategy
	{
		private Func<object, object> enumerableConstructor;
		private Action<object, object, CopySession> copyChildrenInvoker;
		protected Type type;

		protected AEnumerableStrategy(Type type, Func<object, object> enumerableConstructor, MethodInfo copyChildrenMethod)
		{
			this.type = type;
			this.enumerableConstructor = enumerableConstructor;
			this.copyChildrenInvoker = BuildCopyInvoker(type, copyChildrenMethod);
		}

        #region ITypeStrategy implementation
        public object Create(object originalValue)
        {
            return this.enumerableConstructor(originalValue);
        }

        public void FillCopy(object originalValue, object copyInstance, CopySession session)
        {
            this.copyChildrenInvoker(originalValue, copyInstance, session);
        }
        #endregion

        private static Action<object, object, CopySession> BuildCopyInvoker(Type type, MethodInfo copyChildrenMethod)
		{
			var originalParam = Expression.Parameter(typeof(object), "original");
			var copyParam = Expression.Parameter(typeof(object), "copy");
			var sessionParam = Expression.Parameter(typeof(CopySession), "session");

			var expr =
				Expression.Lambda<Action<object, object, CopySession>>(
					Expression.Call(
						copyChildrenMethod, 
						Expression.Convert(originalParam, type), 
						Expression.Convert(copyParam, type),
						sessionParam
					),
					originalParam,
					copyParam,
					sessionParam
				);

			return expr.Compile();
		}
	}
}
