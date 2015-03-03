using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Controllers.Views
{
	public enum BodyType
	{
		Star = 1,
		Planet = 2,

		Unoccupied = 0,
		Own = 4,
		Foreign = 8,

		Empty = 0,
		NoStellarises = Star | Unoccupied,
		OwnStellaris = Star | Own,
		ForeignStellaris = Star | Foreign,
		NotColonised = Planet | Unoccupied,
		OwnColony = Planet | Own,
		ForeignColony = Planet | Foreign
	}
}
