using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Ikadn;

namespace Stareater.Utils.StateEngine
{
	abstract class AEnumerableStrategy : ITypeStrategy
	{
		private Action<object, object, object, CopySession> copyChildrenInvoker;
		private Func<object, object, IEnumerable<object>> childDependencyInvoker;
		private Func<object, object, SaveSession, IkonBaseObject> serializeChildrenInvoker;
		private Func<object, IkadnBaseObject, LoadSession, object> deserializeChildrenInvoker;
		protected Type type;

		protected AEnumerableStrategy(Type type)
		{
			this.type = type;
			this.copyChildrenInvoker = BuildCopyInvoker(type, getMethodInfo(type, "copyChildren"));
			this.childDependencyInvoker = BuildChildDependencyInvoker(type, getMethodInfo(type, "listChildren"));
			this.serializeChildrenInvoker = BuildSerializeInvoker(type, getMethodInfo(type, "serializeChildren"));
			this.deserializeChildrenInvoker = BuildDeserializeInvoker(type, getMethodInfo(type, "deserializeChildren"));
		}

		#region ITypeStrategy implementation
		public abstract object Create(object originalValue);

        public void FillCopy(object originalValue, object copyInstance, CopySession session)
        {
            this.copyChildrenInvoker(this, originalValue, copyInstance, session);
        }

		public IEnumerable<object> Dependencies(object originalValue)
		{
			return this.childDependencyInvoker(this, originalValue);
		}

		public IkonBaseObject Serialize(object originalValue, SaveSession session)
		{
			return this.serializeChildrenInvoker(this, originalValue, session);
		}

		public object Deserialize(IkadnBaseObject data, LoadSession session)
		{
			return this.deserializeChildrenInvoker(this, data, session);
		}
		#endregion

		protected abstract MethodInfo getMethodInfo(Type type, string name);

		private static Action<object, object, object, CopySession> BuildCopyInvoker(Type type, MethodInfo copyChildrenMethod)
		{
			var thisParam = Expression.Parameter(typeof(object), "thisObj");
			var originalParam = Expression.Parameter(typeof(object), "original");
			var copyParam = Expression.Parameter(typeof(object), "copy");
			var sessionParam = Expression.Parameter(typeof(CopySession), "session");

			var expr =
				Expression.Lambda<Action<object, object, object, CopySession>>(
					Expression.Call(
						thisParam,
                        copyChildrenMethod, 
						Expression.Convert(originalParam, type), 
						Expression.Convert(copyParam, type),
						sessionParam
					),
					thisParam,
					originalParam,
					copyParam,
					sessionParam
				);

			return expr.Compile();
		}

		private static Func<object, object, IEnumerable<object>> BuildChildDependencyInvoker(Type type, MethodInfo childDependencyMethod)
		{
			var thisParam = Expression.Parameter(typeof(object), "thisObj");
			var originalParam = Expression.Parameter(typeof(object), "original");

			var expr =
				Expression.Lambda<Func<object, object, IEnumerable<object>>>(
					Expression.Call(
						thisParam,
						childDependencyMethod,
						Expression.Convert(originalParam, type)
					),
					thisParam,
					originalParam
				);

			return expr.Compile();
		}

		private static Func<object, object, SaveSession, IkonBaseObject> BuildSerializeInvoker(Type type, MethodInfo serializeChildrenMethod)
		{
			var thisParam = Expression.Parameter(typeof(object), "thisObj");
			var originalParam = Expression.Parameter(typeof(object), "original");
			var sessionParam = Expression.Parameter(typeof(SaveSession), "session");

			var expr =
				Expression.Lambda<Func<object, object, SaveSession, IkonBaseObject>>(
					Expression.Call(
						thisParam,
						serializeChildrenMethod,
						Expression.Convert(originalParam, type),
						sessionParam
					),
					thisParam,
					originalParam,
					sessionParam
				);

			return expr.Compile();
		}

		private static Func<object, IkadnBaseObject, LoadSession, object> BuildDeserializeInvoker(Type type, MethodInfo deserializeChildrenMethod)
		{
			var thisParam = Expression.Parameter(typeof(object), "thisObj");
			var saveDataParam = Expression.Parameter(typeof(IkadnBaseObject), "rawData");
			var sessionParam = Expression.Parameter(typeof(LoadSession), "session");

			var expr =
				Expression.Lambda<Func<object, IkadnBaseObject, LoadSession, object>>(
					Expression.Call(
						thisParam,
						deserializeChildrenMethod,
						saveDataParam,
						sessionParam
					),
					thisParam,
					saveDataParam,
					sessionParam
				);

			return expr.Compile();
		}
	}
}
