using System.IO;

namespace Stareater.Localization.Reading
{
	class Parser : IKON.Parser
	{
		public Parser(TextReader input) : base(input)
		{
			RegisterFactory(new ContextFactory());
			RegisterFactory(new SingleLineFactory());
		}
	}
}
