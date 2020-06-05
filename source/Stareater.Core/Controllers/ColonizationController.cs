using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class ColonizationController
	{
		private readonly MainGame game;
		private readonly PlayerController playerController;
		private readonly Player player;
		private readonly Planet planet;
		
		internal ColonizationController(MainGame game, Planet planet, bool readOnly, PlayerController playerController)
		{
			this.game = game;
			this.playerController = playerController;
			this.player = playerController.PlayerInstance(game);
			this.IsReadOnly = readOnly;
			this.planet = planet;
		}
		
		public bool IsReadOnly { get; private set; }

		#region Planet
		public StarInfo HostStar 
		{
			get { return new StarInfo(this.planet.Star); }
		}

		public PlanetInfo PlanetBody => new PlanetInfo(this.planet, this.game, this.player.Intelligence.About(this.planet));
		#endregion

		#region Colony
		public double Population 
		{
			get 
			{ 
				return !this.game.States.Colonies.AtPlanet.Contains(this.planet) ? 
					0 :
					this.game.States.Colonies.AtPlanet[this.planet].Population;
			}
		}
		
		public double PopulationMax 
		{
			get
			{ 
				if (this.game.States.Colonies.AtPlanet.Contains(this.planet))
					return this.game.Derivates[this.game.States.Colonies.AtPlanet[this.planet]].MaxPopulation;
				
				var vars = new Var(ColonyProcessor.PlanetSizeKey, this.planet.Size);
				return this.game.Statics.ColonyFormulas.UncolonizedMaxPopulation.Evaluate(vars.Get);
			}
		}
		#endregion

		#region Colonization
		public bool IsColonizing 
		{
			get 
			{ 
				return this.game.Orders[this.player].ColonizationTargets.Contains(this.planet);
			}
		}

		public void StartColonization()
		{
			if (this.IsReadOnly)
				return;
			
			if (!this.IsColonizing)
				this.game.Orders[this.player].ColonizationTargets.Add(this.planet);
			

			playerController.RunAutomation();
			//TODO(v0.8) update colony ship yards updateStellarises(...);
		}
		
		public void StopColonization()
		{
			if (!this.IsColonizing || this.IsReadOnly)
				return;

			this.game.Orders[this.player].ColonizationTargets.Remove(this.planet);
			playerController.RunAutomation();
			//TODO(v0.8) update colony ship yards updateStellarises(...);
		}
		#endregion

		public void RunAutomation()
		{
			this.playerController.RunAutomation();
		}
	}
}
