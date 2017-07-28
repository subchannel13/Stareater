using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class StateManager
	{
        private readonly Dictionary<Type, ITypeStrategy> experts = new Dictionary<Type, ITypeStrategy>();

		public StateManager()
		{
			this.experts[typeof(bool)] = new TerminalStrategy((x, session) => new IkonInteger((bool)x ? 1 : -1));
			this.experts[typeof(double)] = new TerminalStrategy((x, session) => new IkonFloat((double)x));
			this.experts[typeof(float)] = new TerminalStrategy((x, session) => new IkonFloat((float)x));
			this.experts[typeof(int)] = new TerminalStrategy((x, session) => new IkonInteger((int)x));
			this.experts[typeof(string)] = new TerminalStrategy((x, session) => new IkonText((string)x));

			this.experts[typeof(System.Drawing.Color)] = new TerminalStrategy((x, session) =>
				{
					var color = (System.Drawing.Color)x;
                    return new IkonArray(new[] {
						new IkonInteger(color.R),
                        new IkonInteger(color.G),
                        new IkonInteger(color.B),
					});
				});
			this.experts[typeof(NGenerics.DataStructures.Mathematical.Vector2D)] = new TerminalStrategy((x, session) =>
			{
				var vector = (NGenerics.DataStructures.Mathematical.Vector2D)x;
				return new IkonArray(new[] {
						new IkonFloat(vector.X),
						new IkonFloat(vector.Y),
					});
			});
		}

        public T Copy<T>(T obj)
        {
            return (T)new CopySession(getTypeStrategy).CopyOf(obj);
        }

		public IEnumerable<IkonBaseObject> Save(object obj, ObjectIndexer indexer)
		{
			var session = new SaveSession(getTypeStrategy, indexer);

			yield return session.Serialize(obj);

			foreach (var data in session.GetSerialzedData())
				yield return data;
		}

		private ITypeStrategy getTypeStrategy(Type type)
        {
            if (!this.experts.ContainsKey(type))
                this.experts.Add(type, MakeExpert(type));

            return this.experts[type];
        }

        public static bool IsStateData(ICustomAttributeProvider info)
        {
            return info.GetCustomAttributes(true).Any(a => a is StateProperty);
        }

        public static ITypeStrategy MakeExpert(Type type)
        {
            if (type.IsInterface || type.IsAbstract)
                throw new ArgumentException("Type must be concrete");

            if (type.IsArray)
                return new ArrayStrategy(type);

            if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
				return new DictionaryStrategy(type);

			if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)))
				return new CollectionStrategy(type);

			if (type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Any(IsStateData))
				return new ClassStrategy(type);

			if (type.IsEnum)
				return new TerminalStrategy((x, session) => new IkonComposite(x.ToString()));

			var properties = (StateType)type.GetCustomAttributes(true).
				Concat(type.GetInterfaces().SelectMany(x => x.GetCustomAttributes(true))).
				FirstOrDefault(x => x is StateType);

			if (properties != null && properties.NotStateData)
				return new TerminalStrategy(null);

			if (properties != null && properties.SaveMethod != null)
				return new TerminalStrategy(BuildSaveMethodCaller(type, properties.SaveMethod));

			throw new ArgumentException("Undefined terminal strategy for type: " + type.Name);
		}

		private static Func<object, SaveSession, IkonBaseObject> BuildSaveMethodCaller(Type type, string saveMethod)
		{
			var originalParam = Expression.Parameter(typeof(object), "original");
			var sessionParam = Expression.Parameter(typeof(SaveSession), "session");
			var method = type.GetMethod(saveMethod, new[] { typeof(SaveSession) });

			var expr =
				Expression.Lambda<Func<object, SaveSession, IkonBaseObject>>(
					Expression.Call(
						Expression.Convert(originalParam, type),
						method,
						sessionParam
					),
					originalParam,
					sessionParam
				);

			return expr.Compile();
		}
	}
}
