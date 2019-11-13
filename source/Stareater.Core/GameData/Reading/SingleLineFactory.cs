using Ikadn;
using Ikadn.Ikon.Factories;
using Ikadn.Ikon.Types;
using System;

namespace Stareater.GameData.Reading
{
	class SingleLineFactory : AIkonFactory
	{
		protected override IkadnBaseObject ParseObject(IkadnReader reader)
		{
			reader.SkipWhiteSpaces();
			return new IkonText(reader.ReadWhile(x => !char.IsWhiteSpace(x)).Trim());
		}

		public override char Sign => ':';
	}
}
