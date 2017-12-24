using Stareater.AppData.Expressions;
using Stareater.Galaxy.BodyTraits;
using System.Collections.Generic;

namespace Stareater.GameData.Databases.Tables
{
	class PlanetForumlaSet
	{
		public IEnumerable<string> ImpliciteTraits { get; private set; }

		public PlanetForumlaSet(IEnumerable<string> impliciteTraits)
		{
			this.ImpliciteTraits = impliciteTraits;
		}
	}
}
