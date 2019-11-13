using System.Collections.Generic;
using Ikadn.Ikon;
using Ikadn.Ikon.Factories;
using Ikadn.Utilities;

namespace Stareater.GameData.Reading
{
	class Parser : IkonParser
	{
		public Parser(IEnumerable<NamedStream> streams) : 
			base(streams, new AIkonFactory[] {
				new ExpressionFactory(),
				new NoValueFactory(),
				new SingleLineFactory()
			})
		{ }
	}
}
