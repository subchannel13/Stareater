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

			for (int nextCharCode = parser.Reader.Peek();
				nextCharCode != HelperMethods.EndOfStreamInt && (char)nextCharCode != '\n' && (char)nextCharCode != '\r';
				nextCharCode = parser.Reader.Peek()) {
				line.Append((char)parser.Reader.Read());
			}
			return new Text(line.ToString().Trim());
		}

		public char Sign
		{
			get { return '='; }
		}
	}
}
