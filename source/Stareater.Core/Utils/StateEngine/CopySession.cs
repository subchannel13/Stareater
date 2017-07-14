using System;
using System.Collections.Generic;

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
                this.copies[originalValue] = this.expertGetter(originalValue.GetType()).Copy(originalValue, this);

            return this.copies[originalValue];
        }
    }
}
