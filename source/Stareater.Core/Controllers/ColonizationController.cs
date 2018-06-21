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
		private readonly MainGame game;
		private readonly Player player;
		private readonly Planet planet;
		
		internal ColonizationController(MainGame game, Planet planet, bool readOnly, Player player)
		{
			this.game = game;
			this.player = player;
			this.IsReadOnly = readOnly;
			this.planet = planet;
		}
		
		public bool IsReadOnly { get; private set; }

		#region Planet
		public StarInfo HostStar 
		{
			get { return new StarInfo(this.planet.Star); }
		}

		public PlanetInfo PlanetBody
		{
			get { return new PlanetInfo(this.planet); }
		}
		#endregion

		#region Colony
		public double Population 
		{
			get 
			{ 
				return !game.States.Colonies.AtPlanet.Contains(this.planet) ? 
					0 : 
					game.States.Colonies.AtPlanet[this.planet].Population;
			}
		}
		
		public double PopulationMax 
		{
			get
			{ 
				if (game.States.Colonies.AtPlanet.Contains(this.planet))
					return game.Derivates.Of(game.States.Colonies.AtPlanet[this.planet]).MaxPopulation;
				
				var vars = new Var(ColonyProcessor.PlanetSizeKey, this.planet.Size);
				return game.Statics.ColonyFormulas.UncolonizedMaxPopulation.Evaluate(vars.Get);
			}
		}
		#endregion
		
		#region Colonization
		public bool IsColonizing 
		{
			get 
			{ 
				return this.game.Orders[this.player].ColonizationOrders.ContainsKey(this.planet);
			}
		}

		public void StartColonization(params StellarisInfo[] colonizationSources)
		{
			if (this.IsReadOnly)
				return;
			
			ColonizationPlan plan = null;
			if (this.IsColonizing)
				plan = this.game.Orders[this.player].ColonizationOrders[this.planet];
			else
			{
				plan = new ColonizationPlan(this.planet);
				this.game.Orders[this.player].ColonizationOrders.Add(this.planet, plan);
			}
			
			if (colonizationSources != null && colonizationSources.Length > 0)
				foreach(var source in colonizationSources)
					if (!plan.Sources.Contains(source.Stellaris.Location.Star))
						plan.Sources.Add(source.Stellaris.Location.Star); //TODO(later) convert source list to set?

			updateStellarises(plan.Sources);
		}
		
		public void StopColonization(params StellarisInfo[] colonizationSources)
		{
			if (!this.IsColonizing || this.IsReadOnly)
				return;
			
			var plan = this.game.Orders[this.player].ColonizationOrders[this.planet];
			IEnumerable<StarData> toUpdate = new StarData[0];
			
			if (colonizationSources != null && colonizationSources.Length > 0)
				foreach(var source in colonizationSources)
				{
					plan.Sources.Remove(source.Stellaris.Location.Star);
					toUpdate = colonizationSources.Select(x => x.Stellaris.Location.Star);
				}
			else
			{
				toUpdate = plan.Sources.ToArray();
				plan.Sources.Clear();
			}
			
			if (plan.Sources.Count == 0)
				this.game.Orders[this.player].ColonizationOrders.Remove(this.planet);
			else
				updateStellarises(plan.Sources);
			
			updateStellarises(toUpdate);
		}
		
		public IEnumerable<StellarisInfo> AvailableSources()
		{
			var used = new HashSet<StarData>();
			
			if (this.game.Orders[this.player].ColonizationOrders.ContainsKey(this.planet))
				used.UnionWith(this.game.Orders[this.player].ColonizationOrders[this.planet].Sources);
			
			foreach(var stellaris in this.game.States.Stellarises.OwnedBy[this.player])
				if (!used.Contains(stellaris.Location.Star))
					yield return new StellarisInfo(stellaris, this.game);
		}
		
		public IEnumerable<StellarisInfo> Sources()
		{
			if (!this.game.Orders[this.player].ColonizationOrders.ContainsKey(this.planet))
				return new StellarisInfo[0];
			
			var stars = new HashSet<StarData>();
			stars.UnionWith(this.game.Orders[this.player].ColonizationOrders[this.planet].Sources);
			
			return stars.Select(x => new StellarisInfo(this.game.States.Stellarises.At[x].First(s => s.Owner == this.player), this.game));
		}
		#endregion
		
		private void updateStellarises(IEnumerable<StarData> sources)
		{
			var playerProcessor = this.game.Derivates.Of(this.player);
			
			foreach(var source in sources)
			{
				var stellaris = this.game.States.Stellarises.At[source].First(x => x.Owner == this.player);
				this.game.Derivates.Stellarises.Of[stellaris].CalculateSpending(this.game);
			}
		}
	}
}
