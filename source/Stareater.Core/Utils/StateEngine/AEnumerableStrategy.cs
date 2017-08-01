using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	abstract class AEnumerableStrategy : ITypeStrategy
	{
		private Func<object, object> enumerableConstructor;
		private Action<object, object, CopySession> copyChildrenInvoker;
		private Func<object, IEnumerable<object>> childDependencyInvoker;
		private Func<object, SaveSession, IkonBaseObject> serializeChildrenInvoker;
		protected Type type;

		protected AEnumerableStrategy(Type type, Func<object, object> enumerableConstructor, MethodInfo copyChildrenMethod,
			MethodInfo childDependencyMethod, MethodInfo serializeChildrenMethod)
		{
			this.type = type;
			this.enumerableConstructor = enumerableConstructor;
			this.copyChildrenInvoker = BuildCopyInvoker(type, copyChildrenMethod);
			this.childDependencyInvoker = BuildChildDependencyInvoker(type, childDependencyMethod);
			this.serializeChildrenInvoker = BuildSerializeInvoker(type, serializeChildrenMethod);
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

		public IEnumerable<object> Dependencies(object originalValue)
		{
			return this.childDependencyInvoker(originalValue);
		}

		public IkonBaseObject Serialize(object originalValue, SaveSession session)
		{
			return this.serializeChildrenInvoker(originalValue, session);
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

		private static Func<object, IEnumerable<object>> BuildChildDependencyInvoker(Type type, MethodInfo childDependencyMethod)
		{
			var originalParam = Expression.Parameter(typeof(object), "original");

			var expr =
				Expression.Lambda<Func<object, IEnumerable<object>>>(
					Expression.Call(
						childDependencyMethod,
						Expression.Convert(originalParam, type)
					),
					originalParam
				);

			return expr.Compile();
		}

		private static Func<object, SaveSession, IkonBaseObject> BuildSerializeInvoker(Type type, MethodInfo serializeChildrenMethod)
		{
			var originalParam = Expression.Parameter(typeof(object), "original");
			var sessionParam = Expression.Parameter(typeof(SaveSession), "session");

			var expr =
				Expression.Lambda<Func<object, SaveSession, IkonBaseObject>>(
					Expression.Call(
						serializeChildrenMethod,
						Expression.Convert(originalParam, type),
						sessionParam
					),
					originalParam,
					sessionParam
				);

			return expr.Compile();
		}
	}
}
