using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class DictionaryStrategy : AEnumerableStrategy
	{
		public DictionaryStrategy(Type type)
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
		
		private static void copyChildren<K, V>(IDictionary<K, V> originalDictionary, IDictionary<K, V> dictionaryCopy, CopySession session)
		{
			foreach (var element in originalDictionary)
				dictionaryCopy.Add((K)session.CopyOf(element.Key), (V)session.CopyOf(element.Value));
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
			return typeof(DictionaryStrategy).
				GetMethod("copyChildren", BindingFlags.NonPublic | BindingFlags.Static).
				MakeGenericMethod(type.GetGenericArguments());
		}
	}
}
