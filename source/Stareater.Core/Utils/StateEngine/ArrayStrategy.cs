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
		private Func<object, object> mirrorConstructor;
		private Func<int, object> lengthConstructor;

		public ArrayStrategy(Type type)
			: base(type)
		{
			this.mirrorConstructor = BuildMirrorConstructor(type);
			this.lengthConstructor = BuildLengthConstructor(type);
		}

		public override object Create(object originalValue)
		{
			return this.mirrorConstructor(originalValue);
		}

		private void copyChildren<T>(T[] originalArray, T[] arrayCopy, CopySession session)
		{
			for (int i = 0; i < originalArray.Length; i++)
				arrayCopy[i] = (T)session.CopyOf(originalArray[i]);
		}

		private IEnumerable<object> listChildren<T>(T[] originalArray)
		{
			foreach (var element in originalArray)
				yield return element;
		}

		private IkonBaseObject serializeChildren<T>(T[] originalArray, SaveSession session)
		{
			return new IkonArray(originalArray.Select(x => session.Serialize(x)));
		}

		private object deserializeChildren<T>(Ikadn.IkadnBaseObject rawData, LoadSession session)
		{
			var saveData = rawData.To<IkonArray>();
			var result = (T[])this.lengthConstructor(saveData.Count);

			for (int i = 0; i < saveData.Count; i++)
				result[i] = session.Load<T>(saveData[i]);

			return result;
		}

		protected override MethodInfo getMethodInfo(Type type, string name)
		{
			return typeof(ArrayStrategy).
				GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).
				MakeGenericMethod(type.GetElementType());
		}

		private static Func<int, object> BuildLengthConstructor(Type type)
		{
			var lengthParam = Expression.Parameter(typeof(int), "length");

			var expr =
				Expression.Lambda<Func<int, object>>(
					Expression.NewArrayBounds(
						type.GetElementType(),
						lengthParam
					), 
					lengthParam
				);

			return expr.Compile();
		}

		private static Func<object, object> BuildMirrorConstructor(Type type)
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
	}
}
