using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Controllers.Data
{
	public enum BodyType
	{
		Star = 1,
		Planet = 2,

		Unoccupied = 0,
		Own = 4,
		Foreign = 8,

		Empty = 0,
		NoStarManagement = Star | Unoccupied,
		OwnStarManagement = Star | Own,
		ForeignStar = Star | Foreign,
		NotColonised = Planet | Unoccupied,
		OwnColony = Planet | Own,
		ForeignColony = Planet | Foreign
	}
}
