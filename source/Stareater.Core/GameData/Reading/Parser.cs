using System;
using System.IO;
using Ikadn.Ikon;

namespace Stareater.GameData.Reading
{
	public class Parser : IkonParser
	{
		public Parser(TextReader reader) : base(reader)
		{
			this.RegisterFactory(new ExpressionFactory());
			this.RegisterFactory(new SingleLineFactory());
		}
	}
}
