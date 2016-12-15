using System;
using System.IO;
using Ikadn.Ikon;

namespace Stareater.GameData.Reading
{
	class Parser : IkonParser
	{
		public Parser(TextReader reader) : base(reader)
		{
			this.RegisterFactory(new ExpressionFactory());
			this.RegisterFactory(new NoValueFactory());
			this.RegisterFactory(new SingleLineFactory());
		}
	}
}
