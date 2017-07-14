using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class CollectionStrategy : AEnumerableStrategy
	{
		public CollectionStrategy(Type type)
			: base(type, BuildConstructor(type), CopyMethodInfo(type))
		{ }
		
		private static void copyChildren<T>(ICollection<T> originalCollection, ICollection<T> collectionCopy, CopySession session)
		{
			foreach (var element in originalCollection)
				collectionCopy.Add((T)session.CopyOf(element));
		}

		private static Func<object, object> BuildConstructor(Type type)
		{
			var ctorInfo = type.GetConstructor(new Type[0]);
			var funcBody = Expression.New(ctorInfo);

			var expr =
				Expression.Lambda<Func<object, object>>(
					funcBody,
					Expression.Parameter(typeof(object), "o")
				);

			return expr.Compile();
		}

		private static MethodInfo CopyMethodInfo(Type type)
		{
            var interfaceType = type.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>));

            return typeof(CollectionStrategy).
				GetMethod("copyChildren", BindingFlags.NonPublic | BindingFlags.Static).
				MakeGenericMethod(interfaceType.GetGenericArguments()[0]);
		}
	}
}
