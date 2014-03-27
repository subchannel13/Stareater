using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ikadn;
using Ikadn.Utilities;

namespace Stareater.Localization.Reading
{
	class TextBlockFactory : IIkadnObjectFactory
	{
		private const char BlockCloseChar = '}';
		private const string PlaceholderMark = "=";
		private const char SubstitutionOpenChar = '{';
		private const char SubstitutionCloseChar = '}';
		
		const char EscapeChar = '\\';

		public IkadnBaseObject Parse(IkadnParser parser)
		{
			parser.Reader.SkipWhile(nextChar =>
			{
				if (!char.IsWhiteSpace(nextChar))
					throw new FormatException("Unexpected non-white character at " + parser.Reader.PositionDescription);
				return nextChar != '\n' && nextChar != '\r';
			});
			parser.Reader.SkipWhile('\n', '\r'); //FIXME(v0.5): try to include new lines below first one

			Queue<string> textRuns = new Queue<string>();
			Dictionary<string, IText> substitutions = new Dictionary<string, IText>();
			while (parser.Reader.Peek() != BlockCloseChar) {
				if (parser.Reader.Peek() == SubstitutionOpenChar) {
					parser.Reader.Read();
					string substitutionName = parser.Reader.ReadUntil(SubstitutionCloseChar);
					parser.Reader.Read();
					if (substitutionName.Length == 0)
						throw new FormatException("Substitution name at " + parser.Reader + " is empty (zero length)");

					textRuns.Enqueue(null);
					textRuns.Enqueue(substitutionName);

					if (!substitutions.ContainsKey(substitutionName) && !substitutionName.StartsWith(PlaceholderMark))
						substitutions.Add(substitutionName, null);
				}
				else
					textRuns.Enqueue(Parser.ParseString(parser.Reader,
						new int[]{SubstitutionOpenChar, BlockCloseChar},
						EscapeChar, c => c));
			}
			parser.Reader.Read();

			for(int i = 0; i < substitutions.Count;i++) {
				if (parser.Reader.SkipWhiteSpaces().EndOfStream)
					throw new EndOfStreamException("Unexpectedend of stream at " + parser.Reader.PositionDescription);

				string substitutionName = parser.Reader.ReadUntil(c => 
				{ 
					return c != IkadnReader.EndOfStreamResult && char.IsWhiteSpace((char)c); 
				});

				substitutions[substitutionName] = parser.ParseNext().To<IText>();
			}

			List<IText> texts = new List<IText>();
			while (textRuns.Count > 0) {
				string textRun = textRuns.Dequeue();

				if (textRun != null)
					texts.Add(new SingleLineText(textRun));
				else {
					string textKey = textRuns.Dequeue();

					if (textKey.StartsWith(PlaceholderMark))
						texts.Add(new PlaceholderText(textKey.Substring(PlaceholderMark.Length)));
					else
						texts.Add(substitutions[textKey]);
				}
				
			}
			
			//FIXME(v0.5): add deterministic ending (from the perspective of LL(1) parser) for type syntax
			
			return new ChainText(texts.ToArray());
		}

		public char Sign
		{
			get { return '{'; }
		}
	}
}
