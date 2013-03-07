using System.Text;
using Ikadn;

namespace Stareater.Localization.Reading
{
	class SingleLineFactory : IValueFactory
	{
		public IkadnBaseValue Parse(Ikadn.Parser parser)
		{
			return new SingleLineText(parser.Reader.ReadUntil('\n', '\r', IkadnReader.EndOfStreamResult));
		}

		public char Sign
		{
			get { return '='; }
		}
	}
}
