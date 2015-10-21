using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class ColonizationController
	{
		internal Game Game { get; private set; }
		public Planet PlanetBody { get; private set; }
		
		internal ColonizationController(Game game, Planet planet, bool readOnly)
		{
			this.Game = game;
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
				return !Game.States.Colonies.AtPlanetContains(PlanetBody) ? 
					0 : 
					Game.States.Colonies.AtPlanet(PlanetBody).Population;
			}
		}
		
		public double PopulationMax 
		{
			get
			{ 
				if (Game.States.Colonies.AtPlanetContains(PlanetBody))
					return Game.Derivates.Of(Game.States.Colonies.AtPlanet(PlanetBody)).MaxPopulation;
				
				var vars = new Var(ColonyProcessor.PlanetSizeKey, PlanetBody.Size);
				return Game.Statics.ColonyFormulas.UncolonizedMaxPopulation.Evaluate(vars.Get);
			}
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
			
			var plan = new ColonizationPlan(this.PlanetBody);
			if (colonizationSources != null && colonizationSources.Length > 0)
				plan.Sources.AddRange(colonizationSources.Select(x => x.Stellaris.Location.Star));
			
			this.Game.CurrentPlayer.Orders.ColonizationOrders.Add(this.PlanetBody, plan);
			updateStellarises(plan.Sources);
		}
		
		public void StopColonization(params StellarisInfo[] colonizationSources)
		{
			if (!this.IsColonizing || this.IsReadOnly)
				return;
			
			var plan = this.Game.CurrentPlayer.Orders.ColonizationOrders[this.PlanetBody];
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
				this.Game.CurrentPlayer.Orders.ColonizationOrders.Remove(this.PlanetBody);
			else
				updateStellarises(plan.Sources);
			
			updateStellarises(toUpdate);
		}
		
		public IEnumerable<StellarisInfo> AvailableSources()
		{
			var used = new HashSet<StarData>();
			
			if (this.Game.CurrentPlayer.Orders.ColonizationOrders.ContainsKey(this.PlanetBody))
				used.UnionWith(this.Game.CurrentPlayer.Orders.ColonizationOrders[this.PlanetBody].Sources);
			
			foreach(var stellaris in this.Game.States.Stellarises.OwnedBy(this.Game.CurrentPlayer))
				if (!used.Contains(stellaris.Location.Star))
					yield return new StellarisInfo(stellaris);
		}
		
		public IEnumerable<StellarisInfo> Sources()
		{
			if (!this.Game.CurrentPlayer.Orders.ColonizationOrders.ContainsKey(this.PlanetBody))
				return new StellarisInfo[0];
			
			var stars = new HashSet<StarData>();
			//TODO(0.5) add states
			//stars.UnionWith(this.Game.States.ColonizationProjects.Of(this.PlanetBody).Select(x => x));
			stars.UnionWith(this.Game.CurrentPlayer.Orders.ColonizationOrders[this.PlanetBody].Sources);
			
			return stars.Select(x => new StellarisInfo(this.Game.States.Stellarises.At(x)));
		}
		#endregion
		
		private void updateStellarises(IEnumerable<StarData> sources)
		{
			var playerProcessor = this.Game.Derivates.Of(this.Game.CurrentPlayer);
			
			foreach(var source in sources)
			{
				var stellaris = this.Game.States.Stellarises.At(source);
				this.Game.Derivates.Stellarises.Of(stellaris).CalculateSpending(
					playerProcessor, 
					this.Game.Derivates.Colonies.At(stellaris.Location.Star).Where(x => x.Owner == this.Game.CurrentPlayer)
				);
			}
		}
	}
}
