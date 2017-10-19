using Ikadn.Ikon.Types;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Stareater.Utils.StateEngine
{
	class PropertyStrategy
	{
		private Func<object, object> getter;
		private Action<object, object> setter;
		private Action<object, IkonBaseObject, LoadSession> deserializer;
		private Type type;

		public string Name { get; private set; }
		public StateProperty Attribute { get; private set; }

		public PropertyStrategy(PropertyInfo property)
		{
			if (property.DeclaringType != property.ReflectedType)
				property = property.DeclaringType.GetProperty(property.Name);

			this.Attribute = (StateProperty)property.GetCustomAttributes(true).First(a => a is StateProperty);
			this.getter = BuildGetAccessor(property);
			this.setter = BuildSetAccessor(property);
			this.deserializer = BuildDeserializer(property);
			this.type = property.PropertyType;
			this.Name = this.Attribute.SaveKey ?? property.Name;
		}

		public void Copy(object originalObject, object objectCopy, CopySession session)
		{
			this.setter(objectCopy, Attribute.DoCopy ? session.CopyOf(this.getter(originalObject)) : null);
		}

		public object Get(object originalObject)
		{
			return this.getter(originalObject);
		}

		public void SetNull(object parentObject)
		{
			this.setter(parentObject, null);
        }

		public IkonBaseObject Serialize(object originalObject, SaveSession session)
		{
			return session.Serialize(this.getter(originalObject));
		}

		public void Deserialize(object parentObject, IkonBaseObject rawData, LoadSession session)
		{
			this.deserializer(parentObject, rawData, session);
		}

		private static Func<object, object> BuildGetAccessor(PropertyInfo property)
		{
			var method = property.GetGetMethod(true);

			if (method == null)
				throw new ArgumentException("Can't find getter for " + property.Name + " in " + property.DeclaringType.FullName);

			var obj = Expression.Parameter(typeof(object), "o");

			var expr =
				Expression.Lambda<Func<object, object>>(
					Expression.Convert(
						Expression.Call(
							Expression.Convert(obj, method.DeclaringType),
							method),
						typeof(object)),
					obj);

			return expr.Compile();
		}

		private static Action<object, object> BuildSetAccessor(PropertyInfo property)
		{
			var method = property.GetSetMethod(true);

			if (method == null)
				throw new ArgumentException("Can't find setter for " + property.Name + " in " + property.DeclaringType.FullName);

			var obj = Expression.Parameter(typeof(object), "o");
			var value = Expression.Parameter(typeof(object));

			var expr =
				Expression.Lambda<Action<object, object>>(
					Expression.Call(
						Expression.Convert(obj, method.DeclaringType),
						method,
						Expression.Convert(value, method.GetParameters()[0].ParameterType)),
					obj,
					value);

			return expr.Compile();
		}

		private static Action<object, IkonBaseObject, LoadSession> BuildDeserializer(PropertyInfo property)
		{
			var method = property.GetSetMethod(true);

			if (method == null)
				throw new ArgumentException("Can't find setter for " + property.Name + " in " + property.DeclaringType.FullName);

			var obj = Expression.Parameter(typeof(object), "o");
			var saveDataParam = Expression.Parameter(typeof(IkonBaseObject), "rawData");
			var sessionParam = Expression.Parameter(typeof(LoadSession), "session");

			var loadMethod = typeof(LoadSession).
				GetMethod("Load", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(IkonBaseObject) }, null).
				MakeGenericMethod(property.PropertyType);

			var expr =
				Expression.Lambda<Action<object, IkonBaseObject, LoadSession>>(
					Expression.Call(
						Expression.Convert(obj, method.DeclaringType),
						method,
						Expression.Call(sessionParam, loadMethod, saveDataParam)
					),
					obj,
					saveDataParam,
					sessionParam
				);

			return expr.Compile();
		}
	}
}
