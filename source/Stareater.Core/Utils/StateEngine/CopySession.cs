using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
    public class CopySession
    {
        private readonly Func<Type, ITypeStrategy> expertGetter;
		private readonly Dictionary<Type, Dictionary<object, object>> copies = new Dictionary<Type, Dictionary<object, object>>();

        public CopySession(Func<Type, ITypeStrategy> expertGetter)
        {
            this.expertGetter = expertGetter;
        }

        internal object CopyOf(object originalValue)
        {
            if (originalValue == null)
                return null;

			var type = originalValue.GetType();
			if (!this.copies.ContainsKey(type))
				this.copies[type] = new Dictionary<object, object>();
			var copiesTable = this.copies[type];

			if (!copiesTable.ContainsKey(originalValue))
            {
                var expert = this.expertGetter(originalValue.GetType());
                var copy = expert.Create(originalValue);

				copiesTable[originalValue] = copy;
                expert.FillCopy(originalValue, copy, this);
            }

            return copiesTable[originalValue];
        }
    }
}
