using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class CollectionStrategy : AEnumerableStrategy
	{
		private Func<object> constructor;

		public CollectionStrategy(Type type)
			: base(type)
		{
			this.constructor = BuildConstructor(type);
		}

		public override object Create(object originalValue)
		{
			return this.constructor();
		}

		private void copyChildren<T>(ICollection<T> originalCollection, ICollection<T> collectionCopy, CopySession session)
		{
			foreach (var element in originalCollection)
				collectionCopy.Add((T)session.CopyOf(element));
		}

		private IEnumerable<object> listChildren<T>(ICollection<T> originalCollection)
		{
			foreach (var element in originalCollection)
				yield return element;
		}

		private IkonBaseObject serializeChildren<T>(ICollection<T> originalCollection, SaveSession session)
		{
			var data = new IkonArray();

			foreach (var element in originalCollection)
				data.Add(session.Serialize(element));

			return data;
		}

		private object deserializeChildren<T>(Ikadn.IkadnBaseObject rawData, LoadSession session)
		{
			var saveData = rawData.To<IkonArray>();
			var result = (ICollection<T>)this.constructor();

			foreach (var elementData in saveData)
				result.Add(session.Load<T>(elementData));

			return result;
		}

		protected override MethodInfo getMethodInfo(Type type, string name)
		{
			var interfaceType = type.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>));

			return typeof(CollectionStrategy).
				GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).
				MakeGenericMethod(interfaceType.GetGenericArguments()[0]);
		}

		private static Func<object> BuildConstructor(Type type)
		{
			var ctorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);

			if (ctorInfo == null)
				throw new ArgumentException(type.FullName + " has no default constructor");

			return Expression.Lambda<Func<object>>(Expression.New(ctorInfo)).Compile();
		}
	}
}
