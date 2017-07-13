using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class ClassStrategy : ITypeStrategy
	{
		private Type type;
		private Func<object> constructor;
		private List<PropertyStrategy> properties;
		
		public ClassStrategy(Type type)
		{
			this.type = type;
			this.constructor = BuildConstructor(type);
			this.properties = getProperties(type).
				Select(x => new PropertyStrategy(x)).
				ToList();
		}
		
		#region ITypeStrategy implementation
		public object Copy(object originalValue, CopySession session)
		{
			var copy = this.constructor();

			foreach (var property in this.properties)
				property.Copy(originalValue, copy, session);

			return copy;
		}
		
		public IEnumerable<Type> Dependencies
		{
			get 
			{
				return getProperties(this.type).
					Select(x => x.PropertyType).
					ToList();
			}
		}
		#endregion
		
		private static IEnumerable<PropertyInfo> getProperties(Type type)
		{
			return type.GetProperties().Where(StateManager.IsStateData);
		}
		
		private static Func<object> BuildConstructor(Type type)
		{
			var ctorInfo = type.GetConstructor(new Type[0]);
			var funcBody = Expression.New(ctorInfo);
			
			var expr =
				Expression.Lambda<Func<object>>(
					funcBody
				);

			return expr.Compile();
		}
	}
}
