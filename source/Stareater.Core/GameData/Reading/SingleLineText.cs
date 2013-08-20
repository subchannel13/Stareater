
using System;
using Ikadn;

namespace Stareater.GameData.Reading
{
	public class SingleLineText : IkadnBaseObject
	{
		private string text;
		
		public SingleLineText(string text)
		{
			this.text = text;
		}
		
		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException(Tag + " is not meant to be serialized.");
		}

		public override object Tag
		{
			get { return "GameData.SingleLineText"; }
		}

		public override T To<T>()
		{
			Type target = typeof(T);

			if (target == typeof(string))
				return (T)((object)this.text);
			else if (target.IsAssignableFrom(this.GetType()))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + Tag);
		}
	}
}
