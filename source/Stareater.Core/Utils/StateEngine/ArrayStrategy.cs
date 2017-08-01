using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Ikadn.Ikon.Types;
using System.Linq;

namespace Stareater.Utils.StateEngine
{
	class ArrayStrategy : AEnumerableStrategy
	{
		public ArrayStrategy(Type type)
			: base(type, BuildConstructor(type), CopyMethodInfo(type), DependencyMethodInfo(type), SerializeMethodInfo(type))
		{ }

		private static void copyChildren<T>(T[] originalArray, T[] arrayCopy, CopySession session)
		{
			for (int i = 0; i < originalArray.Length; i++)
				arrayCopy[i] = (T)session.CopyOf(originalArray[i]);
		}

		private static IEnumerable<object> listChildren<T>(T[] originalArray)
		{
			foreach (var element in originalArray)
				yield return element;
		}

		private static IkonBaseObject serializeChildren<T>(T[] originalArray, SaveSession session)
		{
			return new IkonArray(originalArray.Select(x => session.Serialize(x)));
		}

		private static Func<object, object> BuildConstructor(Type type)
		{
			var originalArray = Expression.Parameter(typeof(object), "original");
			var funcBody = Expression.NewArrayBounds(
				type.GetElementType(), 
				Expression.ArrayLength(Expression.Convert(originalArray, type))
			);

			var expr =
				Expression.Lambda<Func<object, object>>(
					funcBody, originalArray
				);

			return expr.Compile();
		}

		//TODO(v0.7) try to unify with other AEnumerables
		private static MethodInfo CopyMethodInfo(Type type)
		{
			return typeof(ArrayStrategy).
				GetMethod("copyChildren", BindingFlags.NonPublic | BindingFlags.Static).
				MakeGenericMethod(type.GetElementType());
		}

		private static MethodInfo DependencyMethodInfo(Type type)
		{
			return typeof(ArrayStrategy).
				GetMethod("listChildren", BindingFlags.NonPublic | BindingFlags.Static).
				MakeGenericMethod(type.GetElementType());
		}

		private static MethodInfo SerializeMethodInfo(Type type)
		{
			return typeof(ArrayStrategy).
				GetMethod("serializeChildren", BindingFlags.NonPublic | BindingFlags.Static).
				MakeGenericMethod(type.GetElementType());
		}
	}
}
