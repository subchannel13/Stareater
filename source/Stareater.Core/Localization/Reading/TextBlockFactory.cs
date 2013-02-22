using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikon;
using Ikon.Utilities;

namespace Stareater.Localization.Reading
{
	class TextBlockFactory : IValueFactory
	{
		public const char EndingChar = ';';
		private const char BlockCloseChar = '}';
		private const char SupstitutionOpenChar = '{';
		private const char SupstitutionCloseChar = '}';

		const char EscapeChar = '\\';

		public IkonBaseValue Parse(Ikon.Parser parser)
		{
			parser.Reader.SkipWhile(nextChar =>
			{
				if (!char.IsWhiteSpace(nextChar))
					throw new FormatException("Unexpected non-white character at" + parser.Reader.PositionDescription);
				return nextChar != '\n' && nextChar != '\r';
			});
			parser.Reader.SkipWhile('\n', '\r');

			Queue<string> textRuns = new Queue<string>();
			bool escaping = false;
			while (parser.Reader.Peek() != BlockCloseChar) {
				if (parser.Reader.Peek() == SupstitutionOpenChar) {
					parser.Reader.Read();
					textRuns.Enqueue(null);
					textRuns.Enqueue(parser.Reader.ReadWhile(c =>
					{
						return new ReadingDecision(c, (c == SupstitutionCloseChar) ?
							CharacterAction.Skip | CharacterAction.Stop :
							CharacterAction.AcceptAsIs);
					}));
				}
				else
					textRuns.Enqueue(parser.Reader.ReadWhile(c =>
					{
						if (c == EscapeChar && !escaping) {
							escaping = true;
							return new ReadingDecision(c, CharacterAction.Skip);
						}
						escaping = false;
						return new ReadingDecision(c, (c == SupstitutionOpenChar || c == BlockCloseChar) ?
							CharacterAction.Stop :
							CharacterAction.AcceptAsIs);
					}));
			}
			parser.Reader.Read();

			Dictionary<string, IText> supstitutions = new Dictionary<string, IText>();
			while (parser.Reader.HasNext) {
				parser.Reader.SkipWhiteSpaces();

				if (parser.Reader.Peek() == EndingChar) {
					parser.Reader.Read();
					break;
				}

				StringBuilder supstituent = new StringBuilder();
				while (parser.Reader.HasNext && !char.IsWhiteSpace(parser.Reader.Peek()))
					supstituent.Append(parser.Reader.Read());
				
				supstitutions.Add(supstituent.ToString(), parser.ParseNext().To<IText>());
			}

			List<IText> texts = new List<IText>();
			while (textRuns.Count > 0) {
				string textRun = textRuns.Dequeue();
				texts.Add((textRun == null) ?
					supstitutions[textRuns.Dequeue()] :
					new SingleLineText(textRun)
				);
			}
			
			return new ChainText(texts.ToArray());
		}

		public char Sign
		{
			get { return '{'; }
		}
	}
}
