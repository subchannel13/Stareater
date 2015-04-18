using System.Collections.Generic;

namespace Stareater.Localization
{
	public interface IText
	{
		string Text();
		string Text(double trivialVariable);
		string Text(IDictionary<string, double> variables);
		string Text(IDictionary<string, string> placeholderContents);
		string Text(IDictionary<string, double> variables, IDictionary<string, string> placeholderContents);
		IEnumerable<string> VariableNames();
	}
}
