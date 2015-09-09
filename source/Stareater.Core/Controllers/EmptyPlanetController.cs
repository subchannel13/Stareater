using System;
using Stareater.Galaxy;

namespace Stareater.Controllers
{
	public class EmptyPlanetController
	{
		internal Game Game { get; private set; }
		internal Planet PlanetBody { get; private set; }
		
		internal EmptyPlanetController(Game game, Planet planet, bool readOnly)
		{
			this.Game = game;
			this.IsReadOnly = readOnly;
			this.PlanetBody = planet;
		}
		
		public bool IsReadOnly { get; private set; }

		public int BodyPosition 
		{
			get { return PlanetBody.Position; }
		}

		public PlanetType BodyType 
		{
			get { return PlanetBody.Type; }
		}
		
		public StarData HostStar 
		{
			get { return PlanetBody.Star; }
		}
	}
}
