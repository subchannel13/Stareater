using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Localization
{
	interface IText
	{
		string Get(params double[] variables);
	}
}
