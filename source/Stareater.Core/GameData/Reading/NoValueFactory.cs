using System;
using Ikadn;

namespace Stareater.GameData.Reading
{
	class NoValueFactory : IIkadnObjectFactory
	{
		#region IIkadnObjectFactory implementation
		public IkadnBaseObject Parse(IkadnParser parser)
		{
			return new NoValue();
		}

		public char Sign 
		{
			get { return '.'; }
		}
		#endregion
	}
}
