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

		public string Name { get; private set; } //TODO(v0.7) take name from attribute
		public StateProperty Attribute { get; private set; }

		public PropertyStrategy(PropertyInfo property)
		{
			if (property.DeclaringType != property.ReflectedType)
				property = property.DeclaringType.GetProperty(property.Name);

			this.Attribute = (StateProperty)property.GetCustomAttributes(true).First(a => a is StateProperty);
			this.getter = BuildGetAccessor(property);
			this.setter = BuildSetAccessor(property);
			this.Name = property.Name;
		}

		public void Copy(object originalObject, object objectCopy, CopySession session)
		{
			this.setter(objectCopy, Attribute.DoCopy ? session.CopyOf(this.getter(originalObject)) : null);
		}

		public bool HasValue(object originalObject)
		{
			return this.getter(originalObject) != null;
		}

		public IkonBaseObject Serialize(object originalObject, SaveSession session)
		{
			return session.Serialize(this.getter(originalObject));
		}

		private static Func<object, object> BuildGetAccessor(PropertyInfo property)
		{
			var method = property.GetGetMethod(true);
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
	}
}
