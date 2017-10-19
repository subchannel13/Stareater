using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class DictionaryStrategy : AEnumerableStrategy
	{
		private Func<object> constructor;

		public DictionaryStrategy(Type type)
			: base(type)
		{
			this.constructor = BuildConstructor(type);
		}

		public override object Create(object originalValue)
		{
			return this.constructor();
		}

		private void copyChildren<K, V>(IDictionary<K, V> originalDictionary, IDictionary<K, V> dictionaryCopy, CopySession session)
		{
			foreach (var element in originalDictionary)
				dictionaryCopy.Add((K)session.CopyOf(element.Key), (V)session.CopyOf(element.Value));
		}

		private IEnumerable<object> listChildren<K, V>(IDictionary<K, V> originalDictionary)
		{
			foreach (var element in originalDictionary)
			{
				yield return element.Key;
				yield return element.Value;
			}
        }

		private IkonBaseObject serializeChildren<K, V>(IDictionary<K, V> originalDictionary, SaveSession session)
		{
			var data = new IkonArray();

			foreach (var element in originalDictionary)
			{
				data.Add(session.Serialize(element.Key));
				data.Add(session.Serialize(element.Value));
			}

			return data;
		}

		private object deserializeChildren<K, V>(Ikadn.IkadnBaseObject rawData, LoadSession session)
		{
			var saveData = rawData.To<IkonArray>();
			var result = (IDictionary<K, V>)this.constructor();

			for (int i = 0; i < saveData.Count; i += 2)
				result.Add(
					session.Load<K>(saveData[i]),
					session.Load<V>(saveData[i + 1])
				);

			return result;
		}

		protected override MethodInfo getMethodInfo(Type type, string name)
		{
			var interfaceType = type.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>));

			return typeof(DictionaryStrategy).
				GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).
				MakeGenericMethod(interfaceType.GetGenericArguments());
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
