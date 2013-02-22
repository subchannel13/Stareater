using System.Collections.Generic;

namespace Stareater.Localization
{
	public interface IText
	{
		string Text();
		string Text(double trivialVariable);
		string Text(IDictionary<string, double> variables);
		IEnumerable<string> VariableNames();
	}
}
