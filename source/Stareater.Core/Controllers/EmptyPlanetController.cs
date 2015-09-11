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

		#region Planet
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
		#endregion

		#region Colonization
		public bool IsColonizing 
		{
			get 
			{ 
				return this.Game.CurrentPlayer.Orders.ColonizationOrders.ContainsKey(this.PlanetBody);
			}
		}

		public void StartColonization()
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
		
		public void StopColonization()
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
		#endregion
	}
}
