
using System;
using Ikadn;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Reading
{
	public class Expression : IkadnBaseObject
	{
		public Formula Formula { get; private set; }
		
		public Expression(Formula formula)
		{
			this.Formula = formula;
		}

		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException(Tag + " is not meant to be serialized.");
		}

		public override T To<T>()
		{
			Type target = typeof(T);

			if (target.IsAssignableFrom(typeof(Formula)))
				return (T)(object)this.Formula;
			else if (target.IsAssignableFrom(this.GetType()))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + Tag);
		}

		public override object Tag
		{
			get { return "Expression"; }
		}
	}
}
