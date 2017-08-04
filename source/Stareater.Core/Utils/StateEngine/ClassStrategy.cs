using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ikadn;

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

		public IEnumerable<object> Dependencies(object originalValue)
		{
			return this.properties.Select(x => x.Get(originalValue)).Where(x => x != null);
		}

		public IkonBaseObject Serialize(object originalValue, SaveSession session)
		{
			var data = new IkonComposite(type.Name); //TODO(v0.7) take name from attribute
			var reference = session.SaveReference(originalValue, data);

			foreach (var property in this.properties.Where(x => x.Attribute.DoSave))
				if (property.Get(originalValue) != null)
					data.Add(property.Name, property.Serialize(originalValue, session));

			return reference;
		}

		public object Deserialize(IkadnBaseObject rawData, LoadSession session)
		{
			var loadedValue = this.constructor();
			var saveData = rawData.To<IkonComposite>();

			foreach (var property in this.properties.Where(x => x.Attribute.DoSave))
				if (saveData.Keys.Contains(property.Name))
					property.Deserialize(loadedValue, saveData[property.Name], session);
				else
					property.SetNull(loadedValue);

			return loadedValue;
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
