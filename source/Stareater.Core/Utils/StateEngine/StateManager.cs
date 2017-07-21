using Ikadn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class StateManager
	{
        private readonly Dictionary<Type, ITypeStrategy> experts = new Dictionary<Type, ITypeStrategy>();

		public void AddTerminalExpert(Type type, Func<object, IkadnBaseObject> serializationMethod)
		{
			this.experts[type] = new TerminalStrategy(serializationMethod);
		}

        public T Copy<T>(T obj)
        {
            return (T)new CopySession(getTypeStrategy).CopyOf(obj);
        }

		public IEnumerable<IkadnBaseObject> Save(object obj)
		{
			return new SaveSession(getTypeStrategy).Save(obj);
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

			if (type.GetProperties().Any(IsStateData))
				return new ClassStrategy(type);

			throw new ArgumentException("Undefined terminal strategy for type: " + type.Name);
		}
	}
}
