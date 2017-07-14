using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Utils.StateEngine
{
    class CopySession
    {
        private readonly Func<Type, ITypeStrategy> expertGetter;
        private readonly Dictionary<object, object> copies = new Dictionary<object, object>();

        public CopySession(Func<Type, ITypeStrategy> expertGetter)
        {
            this.expertGetter = expertGetter;
        }

        internal object CopyOf(object originalValue)
        {
            if (originalValue == null)
                return null;

            if (!this.copies.ContainsKey(originalValue))
            {
                var expert = this.expertGetter(originalValue.GetType());
                var copy = expert.Create(originalValue);

                this.copies[originalValue] = copy;
                expert.FillCopy(originalValue, copy, this);
            }

            return this.copies[originalValue];
        }
    }
}
