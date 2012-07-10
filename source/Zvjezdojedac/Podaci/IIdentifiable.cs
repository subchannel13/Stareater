using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Podaci
{
	public interface IIdentifiable
	{
		/// <summary>
		/// Dohvat jedinstvenog identifikatora objekta.
		/// </summary>
		int id
		{
			get;
		}
	}
}
