using System.Collections.Generic;
using System.IO;
using Ikadn.Ikon;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Reading
{
	class Parser : IkonParser
	{
		public Parser(TextReader reader, Dictionary<string, Formula> subformulas) : base(reader)
		{
			this.RegisterFactory(new ExpressionFactory(subformulas));
			this.RegisterFactory(new NoValueFactory());
			this.RegisterFactory(new SingleLineFactory());
		}

		public Parser(TextReader reader) : this(reader, new Dictionary<string, Formula>())
		{ }
	}
}
