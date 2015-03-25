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
		private const char BlockCloseChar = '\\';
		private const string PlaceholderMark = "=";
		private const char SubstitutionOpenChar = '{';
		private const char SubstitutionCloseChar = '}';
		
		const char EscapeChar = '\\';

		public IkadnBaseObject Parse(IkadnParser parser)
		{
			string textIndent =
				parser.Reader.LineIndentation +
				readIndentSpec(parser.Reader.ReadUntil('\n','\r').Trim());
			
			skipEndOfLine(parser.Reader);
			
			var textRuns = new Queue<string>();
			var substitutions = new Dictionary<string, IText>();
			var textRunBuilder = new StringBuilder();
			
			if (!checkIndentation(parser.Reader, textIndent))
				return new SingleLineText("");
			
			while(true) {
				if (parser.Reader.Peek() == SubstitutionOpenChar) {
					parser.Reader.Read();
					string substitutionName = parser.Reader.ReadUntil(SubstitutionCloseChar);
					parser.Reader.Read();
					if (substitutionName.Length == 0)
						throw new FormatException("Substitution name at " + parser.Reader + " is empty (zero length)");
					
					textRuns.Enqueue(null);
					textRuns.Enqueue(substitutionName);

					if (!substitutions.ContainsKey(substitutionName) && !substitutionName.StartsWith(PlaceholderMark, StringComparison.Ordinal))
						substitutions.Add(substitutionName, null);
				}
				else {
					string textPart = Parser.ParseString(parser.Reader,
						new int[]{SubstitutionOpenChar, '\n','\r'},
						EscapeChar, c => c
					);

					if (parser.Reader.Peek() != SubstitutionOpenChar) {
						textRunBuilder.AppendLine(textPart);
						skipEndOfLine(parser.Reader);
						if (!checkIndentation(parser.Reader, textIndent))
							break;
					}
					else {
						textRuns.Enqueue(textRunBuilder.ToString() + textPart);
						textRunBuilder.Clear();
					}
				}
			}
			if (textRunBuilder.Length > Environment.NewLine.Length) {
				textRunBuilder.Length -= Environment.NewLine.Length;
				textRuns.Enqueue(textRunBuilder.ToString());
			}

			for(int i = 0; i < substitutions.Count;i++) {
				if (parser.Reader.SkipWhiteSpaces().EndOfStream)
					throw new EndOfStreamException("Unexpectedend of stream at " + parser.Reader.PositionDescription);

				string substitutionName = parser.Reader.ReadUntil(c => 
					c != IkadnReader.EndOfStreamResult && char.IsWhiteSpace((char)c) 
				);

				substitutions[substitutionName] = parser.ParseNext().To<IText>();
			}

			var texts = new List<IText>();
			while (textRuns.Count > 0) {
				string textRun = textRuns.Dequeue();

				if (textRun != null)
					texts.Add(new SingleLineText(textRun));
				else {
					string textKey = textRuns.Dequeue();

					if (textKey.StartsWith(PlaceholderMark, StringComparison.Ordinal))
						texts.Add(new PlaceholderText(textKey.Substring(PlaceholderMark.Length)));
					else
						texts.Add(substitutions[textKey]);
				}
				
			}
			
			return new ChainText(texts.ToArray());
		}

		public char Sign
		{
			get { return '§'; }
		}
		
		private bool checkIndentation(IkadnReader reader, string indentation)
		{
			foreach(char expectedSpace in indentation)
				if (reader.Peek() == expectedSpace)
						reader.Read();
					else
						if (reader.Peek() == BlockCloseChar) {
							reader.Read();
							return false;
						}
						else
							throw new FormatException("Unexpected character at " + reader.PositionDescription + ", expected block closing character or block content indentation.");
		   
			return true;
		}
		
		private string readIndentSpec(string indentSpec)
		{
			return string.IsNullOrEmpty(indentSpec) ? 
				"\t\t" : 
				indentSpec.Replace("\\t", "\t").Replace("\\s", " ");
		   
		}
		
		private void skipEndOfLine(IkadnReader reader)
		{
			char nextChar = reader.Peek();
		   
			if (nextChar == '\r') {
					reader.Read();
					nextChar = reader.Peek();
			}
		   
			if (nextChar != '\n')
					throw new FormatException("No or unknown type of line ending method at " + reader.PositionDescription);
		   
			reader.Read();
		}
	}
}
