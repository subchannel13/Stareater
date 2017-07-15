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
        public object Create(object originalValue)
        {
            return this.constructor();
        }

        public void FillCopy(object originalValue, object copyInstance, CopySession session)
        {
            foreach (var property in this.properties)
                property.Copy(originalValue, copyInstance, session);
        }
        #endregion

        private static IEnumerable<PropertyInfo> getProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).
				Where(StateManager.IsStateData);
		}
		
		private static Func<object> BuildConstructor(Type type)
		{
            var ctorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			var funcBody = Expression.New(ctorInfo);
			
			var expr =
				Expression.Lambda<Func<object>>(
					funcBody
				);

			return expr.Compile();
		}
	}
}
