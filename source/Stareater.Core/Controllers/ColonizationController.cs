using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class ColonizationController
	{
		public Planet PlanetBody { get; private set; }
		
		private readonly Game game;
		private readonly Player player;
		
		internal ColonizationController(Game game, Planet planet, bool readOnly, Player player)
		{
			this.game = game;
			this.player = player;
			this.IsReadOnly = readOnly;
			this.PlanetBody = planet;
		}
		
		public bool IsReadOnly { get; private set; }

		#region Planet
		public StarData HostStar 
		{
			get { return PlanetBody.Star; }
		}
		#endregion

		#region Colony
		public double Population 
		{
			get 
			{ 
				return !game.States.Colonies.AtPlanetContains(PlanetBody) ? 
					0 : 
					game.States.Colonies.AtPlanet(PlanetBody).Population;
			}
		}
		
		public double PopulationMax 
		{
			get
			{ 
				if (game.States.Colonies.AtPlanetContains(PlanetBody))
					return game.Derivates.Of(game.States.Colonies.AtPlanet(PlanetBody)).MaxPopulation;
				
				var vars = new Var(ColonyProcessor.PlanetSizeKey, PlanetBody.Size);
				return game.Statics.ColonyFormulas.UncolonizedMaxPopulation.Evaluate(vars.Get);
			}
		}
		#endregion
		
		#region Colonization
		public bool IsColonizing 
		{
			get 
			{ 
				return this.player.Orders.ColonizationOrders.ContainsKey(this.PlanetBody);
			}
		}

		public void StartColonization(params StellarisInfo[] colonizationSources)
		{
			if (this.IsColonizing || this.IsReadOnly)
				return;
			
			var plan = new ColonizationPlan(this.PlanetBody);
			if (colonizationSources != null && colonizationSources.Length > 0)
				plan.Sources.AddRange(colonizationSources.Select(x => x.Stellaris.Location.Star));
			
			this.player.Orders.ColonizationOrders.Add(this.PlanetBody, plan);
			updateStellarises(plan.Sources);
		}
		
		public void StopColonization(params StellarisInfo[] colonizationSources)
		{
			if (!this.IsColonizing || this.IsReadOnly)
				return;
			
			var plan = this.player.Orders.ColonizationOrders[this.PlanetBody];
			IEnumerable<StarData> toUpdate = new StarData[0];
			
			if (colonizationSources != null && colonizationSources.Length > 0)
				foreach(var source in colonizationSources)
				{
					plan.Sources.Remove(source.Stellaris.Location.Star);
					toUpdate = colonizationSources.Select(x => x.HostStar);
				}
			else
			{
				toUpdate = plan.Sources.ToArray();
				plan.Sources.Clear();
			}
			
			if (plan.Sources.Count == 0)
				this.player.Orders.ColonizationOrders.Remove(this.PlanetBody);
			else
				updateStellarises(plan.Sources);
			
			updateStellarises(toUpdate);
		}
		
		public IEnumerable<StellarisInfo> AvailableSources()
		{
			var used = new HashSet<StarData>();
			
			if (this.player.Orders.ColonizationOrders.ContainsKey(this.PlanetBody))
				used.UnionWith(this.player.Orders.ColonizationOrders[this.PlanetBody].Sources);
			
			foreach(var stellaris in this.game.States.Stellarises.OwnedBy(this.player))
				if (!used.Contains(stellaris.Location.Star))
					yield return new StellarisInfo(stellaris);
		}
		
		public IEnumerable<StellarisInfo> Sources()
		{
			if (!this.player.Orders.ColonizationOrders.ContainsKey(this.PlanetBody))
				return new StellarisInfo[0];
			
			var stars = new HashSet<StarData>();
			//TODO(0.5) add states
			//stars.UnionWith(this.Game.States.ColonizationProjects.Of(this.PlanetBody).Select(x => x));
			stars.UnionWith(this.player.Orders.ColonizationOrders[this.PlanetBody].Sources);
			
			return stars.Select(x => new StellarisInfo(this.game.States.Stellarises.At(x)));
		}
		#endregion
		
		private void updateStellarises(IEnumerable<StarData> sources)
		{
			var playerProcessor = this.game.Derivates.Of(this.player);
			
			foreach(var source in sources)
			{
				var stellaris = this.game.States.Stellarises.At(source);
				this.game.Derivates.Stellarises.Of(stellaris).CalculateSpending(
					playerProcessor, 
					this.game.Derivates.Colonies.At(stellaris.Location.Star).Where(x => x.Owner == this.player)
				);
			}
		}
	}
}
