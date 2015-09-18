using System;
using System.Collections.Generic;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;

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

		public void StartColonization(params StellarisInfo[] colonizationSources)
		{
			if (this.IsColonizing || this.IsReadOnly)
				return;
			
			this.Game.CurrentPlayer.Orders.ColonizationOrders.Add(this.PlanetBody, new ColonizationPlan(this.PlanetBody));
		}
		
		public void StopColonization()
		{
			if (!this.IsColonizing || this.IsReadOnly)
				return;
			
			this.Game.CurrentPlayer.Orders.ColonizationOrders.Remove(this.PlanetBody);
		}
		#endregion
		
		#region Empire
		public IEnumerable<StellarisInfo> Stellarises()
		{
			foreach(var stellaris in this.Game.States.Stellarises.OwnedBy(this.Game.CurrentPlayer))
				yield return new StellarisInfo(stellaris);
		}
		#endregion
	}
}
