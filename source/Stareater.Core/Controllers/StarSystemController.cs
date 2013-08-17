using System;
using System.Collections.Generic;
using Stareater.Galaxy;

namespace Stareater.Controllers
{
	public class StarSystemController
	{
		private Game game;
		
		public StarData Star { get; private set; }
			
		internal StarSystemController(Game game, StarData star)
		{
			this.game = game;
			this.Star = star;
		}
		
		public IEnumerable<Planet> Planets
		{
			get {
				return game.States.Planets.StarSystem(Star);
			}
		}
	}
}
