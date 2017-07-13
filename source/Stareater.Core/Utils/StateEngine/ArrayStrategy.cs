using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class ArrayStrategy : AEnumerableStrategy
	{
		public ArrayStrategy(Type type)
			: base(type, BuildConstructor(type), CopyMethodInfo(type))
		{ }

		#region implemented abstract members of AEnumerableStrategy
		public override IEnumerable<Type> Dependencies
		{
			get 
			{
				yield return this.type.GetElementType();
			}
		}
		#endregion
		
		private static void copyChildren<T>(T[] originalArray, T[] arrayCopy, CopySession session)
		{
			for (int i = 0; i < originalArray.Length; i++)
				arrayCopy[i] = (T)session.CopyOf(originalArray[i]);
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

		private static MethodInfo CopyMethodInfo(Type type)
		{
			return typeof(ArrayStrategy).
				GetMethod("copyChildren", BindingFlags.NonPublic | BindingFlags.Static).
				MakeGenericMethod(type.GetElementType());
		}
	}
}
