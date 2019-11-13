using Ikadn;
using Ikadn.Ikon.Factories;
using System;

namespace Stareater.GameData.Reading
{
	class NoValueFactory : AIkonFactory
	{
		#region AIkonFactory implementation
		protected override IkadnBaseObject ParseObject(IkadnReader reader)
		{
			return new NoValue();
		}

		public override char Sign => '.';
		#endregion
	}
}
