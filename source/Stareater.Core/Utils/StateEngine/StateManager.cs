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
			this.experts[typeof(bool)] = new TerminalStrategy(
				(x, session) => new IkonInteger((bool)x ? 1 : -1),
				(x, session) => x.To<int>() >= 0
			);
			this.experts[typeof(double)] = new TerminalStrategy(
				(x, session) => new IkonFloat((double)x),
				(x, session) => x.To<double>()
			);
			this.experts[typeof(float)] = new TerminalStrategy(
				(x, session) => new IkonFloat((float)x),
				(x, session) => x.To<float>()
			);
			this.experts[typeof(int)] = new TerminalStrategy(
				(x, session) => new IkonInteger((int)x),
				(x, session) => x.To<int>()
			);
			this.experts[typeof(long)] = new TerminalStrategy(
				(x, session) => new IkonInteger((long)x),
				(x, session) => x.To<long>()
			);
			this.experts[typeof(string)] = new TerminalStrategy(
				(x, session) => new IkonText((string)x),
				(x, session) => x.To<string>()
			);

			this.experts[typeof(System.Drawing.Color)] = new TerminalStrategy(
				(x, session) =>
				{
					var color = (System.Drawing.Color)x;
                    return new IkonArray(new[] {
						new IkonInteger(color.R),
                        new IkonInteger(color.G),
                        new IkonInteger(color.B),
					});
				},
				(x, session) =>
				{
					var data = x.To<IkonArray>();
					return System.Drawing.Color.FromArgb(
						data[0].To<int>(),
						data[1].To<int>(),
						data[2].To<int>()
					);
				}
            );
			this.experts[typeof(NGenerics.DataStructures.Mathematical.Vector2D)] = new TerminalStrategy(
				(x, session) =>
				{
					var vector = (NGenerics.DataStructures.Mathematical.Vector2D)x;
					return new IkonArray(new[] {
							new IkonFloat(vector.X),
							new IkonFloat(vector.Y),
						});
				},
				(x, session) =>
				{
					var data = x.To<IkonArray>();
					return new NGenerics.DataStructures.Mathematical.Vector2D(
						data[0].To<int>(),
						data[1].To<int>()
					);
				}
			);
		}

        public T Copy<T>(T obj)
        {
            return (T)new CopySession(getTypeStrategy).CopyOf(obj);
        }

		public T Load<T>(IkonComposite saveData, ObjectDeindexer deindexer)
		{
			return new LoadSession(getTypeStrategy, deindexer).Load<T>(saveData["entryPoint"]);
		}

		public IkonBaseObject Save(object obj, ObjectIndexer indexer)
		{
			var session = new SaveSession(getTypeStrategy, indexer);

			var mainData = session.Serialize(obj);
			var referencedData = session.GetSerialzedData();

			var result = new IkonComposite("Save"); //TODO(v0.7) make constant
			result.Add("references", new IkonArray(referencedData)); //TODO(v0.7) make constant
			result.Add("entryPoint", mainData); //TODO(v0.7) make constant

			return result;
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

			var properties = (StateType)getBaseTypes(type).
				Concat(type.GetInterfaces()).
                SelectMany(x => x.GetCustomAttributes(true)).
				FirstOrDefault(x => x is StateType);

			if (type.IsArray)
                return new ArrayStrategy(type);

            if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
				return new DictionaryStrategy(type);

			if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)))
				return new CollectionStrategy(type);

			if (type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Any(IsStateData))
				return new ClassStrategy(type, properties);

			if (type.IsEnum)
				return new TerminalStrategy(
					(x, session) => new IkonComposite(x.ToString()),
					(x, session) => Enum.Parse(type, (string)x.Tag)
                );

			if (properties != null && properties.NotStateData)
				return new TerminalStrategy(null, null);

			if (properties != null && properties.SaveMethod != null)
				return new TerminalStrategy(
					BuildSaveMethodCaller(type, properties.SaveMethod),
					BuildLoadMethodCaller(type, properties.LoaderClass, properties.LoadMethod)
				);

			throw new ArgumentException("Undefined terminal strategy for type: " + type.Name);
		}

		private static IEnumerable<Type> getBaseTypes(Type type)
		{
			do
			{
				yield return type;
				type = type.BaseType;
			}
			while (type != null);
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

		private static Func<Ikadn.IkadnBaseObject, LoadSession, object> BuildLoadMethodCaller(Type type, Type loaderClass, string loadMethod)
		{
			var dataParam = Expression.Parameter(typeof(Ikadn.IkadnBaseObject), "rawData");
			var sessionParam = Expression.Parameter(typeof(LoadSession), "session");
			var method = (loaderClass ?? type).GetMethod(
				loadMethod,
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, 
				new[] { typeof(Ikadn.IkadnBaseObject), typeof(LoadSession) }, null
			);

			var expr =
				Expression.Lambda<Func<Ikadn.IkadnBaseObject, LoadSession, object>>(
					Expression.Convert(
						Expression.Call(
							method,
							dataParam,
							sessionParam
						),
						typeof(object)
					),
					dataParam,
					sessionParam
				);

			return expr.Compile();
		}
	}
}
