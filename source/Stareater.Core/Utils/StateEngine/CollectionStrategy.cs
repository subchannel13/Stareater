using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class CollectionStrategy : AEnumerableStrategy
	{
		public CollectionStrategy(Type type)
			: base(type, BuildConstructor(type), CopyMethodInfo(type))
		{ }

		#region implemented abstract members of AEnumerableStrategy
		public override IEnumerable<Type> Dependencies
		{
			get 
			{
				foreach(var type in this.type.GetGenericArguments())
					yield return type;
			}
		}
		#endregion
		
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
			return typeof(CollectionStrategy).
				GetMethod("copyChildren", BindingFlags.NonPublic | BindingFlags.Static).
				MakeGenericMethod(type.GetGenericArguments()[0]);
		}
	}
}
