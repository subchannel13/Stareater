using System;
using Ikadn;
using Ikadn.Ikon.Types;

namespace Stareater.GameData.Reading
{
	public class SingleLineFactory : IIkadnObjectFactory
	{
		public IkadnBaseObject Parse(IkadnParser parser)
		{
			parser.Reader.SkipWhiteSpaces();
			return new IkonText(parser.Reader.ReadWhile(x => !char.IsWhiteSpace(x)).Trim());
		}

		public char Sign
		{
			get { return ':'; }
		}
	}
}
