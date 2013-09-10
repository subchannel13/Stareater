using System;
using Ikadn;

namespace Stareater.GameData.Reading
{
	public class SingleLineFactory : IIkadnObjectFactory
	{
		public IkadnBaseObject Parse(IkadnParser parser)
		{
			parser.Reader.SkipWhiteSpaces();
			return new SingleLineText(parser.Reader.ReadWhile(x => !char.IsWhiteSpace(x)).Trim());
		}

		public char Sign
		{
			get { return ':'; }
		}
	}
}
