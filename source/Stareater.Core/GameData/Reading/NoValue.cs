using System;
using Ikadn;

namespace Stareater.GameData.Reading
{
	class NoValue  : IkadnBaseObject
	{
		#region implemented abstract members of IkadnBaseObject
		public override T To<T>()
		{
			throw new InvalidOperationException("Cast to " + typeof(T).Name + " is not supported for " + Tag);
		}
		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException(Tag + " is not meant to be serialized.");
		}
		public override object Tag 
		{
			get { return "NoValue"; }
		}
		#endregion
	}
}
