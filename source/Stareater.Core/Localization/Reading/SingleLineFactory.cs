using System.Text;
using Ikadn;

namespace Stareater.Localization.Reading
{
	class SingleLineFactory : IIkadnObjectFactory
	{
		public IkadnBaseObject Parse(IkadnParser parser)
		{
			return new SingleLineText(parser.Reader.ReadUntil('\n', '\r', IkadnReader.EndOfStreamResult));
		}

		public char Sign
		{
			get { return '='; }
		}
	}
}
