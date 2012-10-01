using IKON;
using IKON.Utils;
using System.Text;

namespace Stareater.Localization.Reading
{
	class SingleLineFactory : IValueFactory
	{
		public Value Parse(IKON.Parser parser)
		{
			StringBuilder line = new StringBuilder();

			for (int nextCharCode = parser.PeakReader;
				nextCharCode != HelperMethods.EndOfStreamInt && (char)nextCharCode != '\n' && (char)nextCharCode != '\r';
				nextCharCode = parser.PeakReader) {
				line.Append(parser.ReadChar());
			}
			return new SingleLineText(line.ToString().Trim());
		}

		public char Sign
		{
			get { return '='; }
		}
	}
}
