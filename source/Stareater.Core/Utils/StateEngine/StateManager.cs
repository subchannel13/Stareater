using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class StateManager
	{
		private readonly Dictionary<Type, ITypeStrategy> experts = new Dictionary<Type, ITypeStrategy>();

		public T Copy<T>(T obj)
		{
			var type = obj.GetType();
			this.InitType(type);
			
			var session = new CopySession(this.experts);
			return (T)this.experts[type].Copy(obj, session);
		}
		
		public void InitType(Type type)
		{
			if (this.experts.ContainsKey(type))
				return;
			
			var typeExpert = MakeExpert(type);
			this.experts.Add(type, typeExpert);
			foreach(var dependency in typeExpert.Dependencies)
				this.InitType(dependency);
		}
		
		public static bool IsStateData(ICustomAttributeProvider info)
		{
			return info.GetCustomAttributes(true).Any(a => a is StateProperty);
		}

		public static ITypeStrategy MakeExpert(Type type)
		{
			if (type.IsArray)
				return new ArrayStrategy(type);

			if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
				return new DictionaryStrategy(type);

			if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)))
				return new CollectionStrategy(type);

			return type.GetProperties().Any(IsStateData) ?
				(ITypeStrategy)new ClassStrategy(type) :
				(ITypeStrategy)new TerminalStrategy();
		}
	}
}
