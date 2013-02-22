using System.IO;

namespace Stareater.Localization.Reading
{
	class Parser : Ikon.Parser
	{
		public Parser(TextReader input) : base(input)
		{
			RegisterFactory(new ContextFactory());
			RegisterFactory(new SingleLineFactory());
			RegisterFactory(new TextBlockFactory());
			RegisterFactory(new ExpressionTextFactory());
		}
	}
}
