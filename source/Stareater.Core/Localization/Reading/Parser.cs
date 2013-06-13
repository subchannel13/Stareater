using System.IO;
using System.Collections.Generic;
using System;
using Ikadn;
using Ikadn.Utilities;

namespace Stareater.Localization.Reading
{
	class Parser : IkadnParser
	{
		public Parser(TextReader input) : base(input)
		{
			RegisterFactory(new ContextFactory());
			RegisterFactory(new SingleLineFactory());
			RegisterFactory(new TextBlockFactory());
			RegisterFactory(new ExpressionTextFactory());
			RegisterFactory(new ConditionalTextFactory());
		}

		public static string ParseString(IkadnReader reader, IEnumerable<int> terminatingCharacters, char escapeCharacter, Func<char, char> escapeCode) {
			var terminatingCharsSet = new HashSet<int>(terminatingCharacters);
			bool escaping = false;

			return reader.ReadConditionally(c =>
					{
						if (c == escapeCharacter && !escaping) {
							escaping = true;
							return new ReadingDecision((char)c, CharacterAction.Skip);
						}
						escaping = false;
						return new ReadingDecision((char)c, terminatingCharsSet.Contains(c) ?
							CharacterAction.Stop :
							CharacterAction.AcceptAsIs);
					});
		}
	}
}
