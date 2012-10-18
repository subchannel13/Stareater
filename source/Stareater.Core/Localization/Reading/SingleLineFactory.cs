using Ikon;
using Ikon.Utilities;
using System.Text;

namespace Stareater.Localization.Reading
{
	class SingleLineFactory : IValueFactory
	{
		public Value Parse(Ikon.Parser parser)
		{
			StringBuilder line = new StringBuilder();

			while(parser.CanRead) {
				char nextChar = parser.ReadChar();
				if (nextChar == '\n' || nextChar == '\r')
					break;
				line.Append(nextChar);
			}
			return new SingleLineText(line.ToString().Trim());
		}

		public char Sign
		{
			get { return '='; }
		}
	}
}
