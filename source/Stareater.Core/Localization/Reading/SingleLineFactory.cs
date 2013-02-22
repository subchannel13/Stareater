using Ikon;
using Ikon.Utilities;
using System.Text;

namespace Stareater.Localization.Reading
{
	class SingleLineFactory : IValueFactory
	{
		public IkonBaseValue Parse(Ikon.Parser parser)
		{
			StringBuilder line = new StringBuilder();

			while(parser.Reader.HasNext) {
				char nextChar = parser.Reader.Read();
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
