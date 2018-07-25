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
        private readonly Dictionary<Type, ITypeStrategy> experts;

		public StateManager()
		{
			this.experts = new Dictionary<Type, ITypeStrategy>
			{
				[typeof(bool)] = new TerminalStrategy(
					(x, session) => new IkonInteger((bool)x ? 1 : -1),
					(x, session) => x.To<int>() >= 0
				),
				[typeof(double)] = new TerminalStrategy(
					(x, session) => new IkonFloat((double)x),
					(x, session) => x.To<double>()
				),
				[typeof(float)] = new TerminalStrategy(
					(x, session) => new IkonFloat((float)x),
					(x, session) => x.To<float>()
				),
				[typeof(int)] = new TerminalStrategy(
					(x, session) => new IkonInteger((int)x),
					(x, session) => x.To<int>()
				),
				[typeof(long)] = new TerminalStrategy(
					(x, session) => new IkonInteger((long)x),
					(x, session) => x.To<long>()
				),
				[typeof(string)] = new TerminalStrategy(
					(x, session) => new IkonText((string)x),
					(x, session) => x.To<string>()
				),

				[typeof(System.Drawing.Color)] = new TerminalStrategy(
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
				),
				[typeof(Vector2D)] = new TerminalStrategy(
					(x, session) =>
					{
						var vector = (Vector2D)x;
						return new IkonArray(new[] {
								new IkonFloat(vector.X),
								new IkonFloat(vector.Y),
							});
					},
					(x, session) =>
					{
						var data = x.To<IkonArray>();
						return new Vector2D(
							data[0].To<double>(),
							data[1].To<double>()
						);
					}
				)
			};
		}

        public T Copy<T>(T obj)
        {
            return (T)new CopySession(getTypeStrategy).CopyOf(obj);
        }

		public T Load<T>(IkonComposite saveData, ObjectDeindexer deindexer, Dictionary<Type, Action<object>> postLoadActions)
		{
			return new LoadSession(getTypeStrategy, deindexer, postLoadActions).
				Load<T>(saveData[EntryPointKey]);
		}

		public IkonBaseObject Save(object obj, ObjectIndexer indexer)
		{
			var session = new SaveSession(getTypeStrategy, indexer);

			var mainData = session.Serialize(obj);
			var referencedData = session.GetSerialzedData();

			var result = new IkonComposite(MainGame.SaveGameTag);
			result.Add("references", new IkonArray(referencedData));
			result.Add(EntryPointKey, mainData);

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
			var typeAttributes = getAttribute<StateType>(type) ?? new StateType();

			if (type.IsArray)
                return new ArrayStrategy(type);

            if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
				return new DictionaryStrategy(type);

			if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)))
				return new CollectionStrategy(type);

			if (type.IsEnum)
				return new TerminalStrategy(
					(x, session) => new IkonComposite(x.ToString()),
					(x, session) => Enum.Parse(type, (string)x.Tag)
                );

			if (typeAttributes.NotStateData)
				return new TerminalStrategy(null, null);

			var baseAttributes = getAttribute<StateBaseType>(type);

			if ((type.IsInterface || type.IsAbstract) && baseAttributes != null)
				return new TerminalStrategy(
					null,
					BuildLoadMethodCaller(type, baseAttributes.LoaderClass, baseAttributes.LoadMethod)
				);

			if (type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Any(IsStateData) ||
				typeAttributes.SaveTag != null)
				return new ClassStrategy(type, typeAttributes);

			if (typeAttributes.SaveMethod != null)
				return new TerminalStrategy(
					BuildSaveMethodCaller(type, typeAttributes.SaveMethod),
					typeAttributes.LoadMethod != null ? BuildLoadMethodCaller(type, null, typeAttributes.LoadMethod) : null
				);

			throw new ArgumentException("Undefined terminal strategy for type: " + type.Name);
		}

		private static T getAttribute<T>(Type type)
		{
			var superTypes = new List<Type>();
			var baseType = type;
			do
			{
				superTypes.Add(baseType);
				baseType = baseType.BaseType;
			}
			while (baseType != null);
			superTypes.AddRange(type.GetInterfaces());

			return (T)superTypes.
				SelectMany(x => x.GetCustomAttributes(true)).
				FirstOrDefault(x => x is T);
        }

		private static Func<object, SaveSession, IkonBaseObject> BuildSaveMethodCaller(Type type, string saveMethod)
		{
			var originalParam = Expression.Parameter(typeof(object), "original");
			var sessionParam = Expression.Parameter(typeof(SaveSession), "session");
			var method = type.GetMethod(
				saveMethod,
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
				new[] { typeof(SaveSession) }, null
			);

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

		private const string EntryPointKey = "entryPoint";
	}
}
