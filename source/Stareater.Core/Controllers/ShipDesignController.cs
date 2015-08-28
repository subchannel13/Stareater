using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Controllers.Views.Ships;
using Stareater.GameData.Ships;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class ShipDesignController
	{
		private readonly Game game;
		private readonly Dictionary<string, int> playersTechLevels;
		
		internal ShipDesignController(Game game)
		{
			this.game = game;
			
			this.playersTechLevels = game.States.TechnologyAdvances.Of(game.CurrentPlayer)
				.ToDictionary(x => x.Topic.IdCode, x => x.Level);
			
			this.armorInfo = bestArmor();
		}

		private ArmorInfo bestArmor()
		{
			var armor = ArmorType.MakeBest(
				game.Statics.Armors.Values,
				playersTechLevels
			);
			
			return armor != null ? new ArmorInfo(armor.TypeInfo, armor.Level) : null;
		}
		
		private IsDriveInfo bestIsDrive()
		{
			var drive = IsDriveType.MakeBest(
				game.Statics.IsDrives.Values, 
				playersTechLevels, 
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level), 
				this.reactorInfo.Power
			);

			return drive != null ? new IsDriveInfo(drive.TypeInfo, drive.Level, this.selectedHull, this.reactorInfo.Power) : null;
		}

		private ReactorInfo bestReactor()
		{
			var reactor = ReactorType.MakeBest(
				game.Statics.Reactors.Values,
				playersTechLevels,
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level)
			);

			return reactor != null ? new ReactorInfo(reactor.TypeInfo, reactor.Level, this.selectedHull) : null;
		}
		
		#region Component lists
		public IEnumerable<HullInfo> Hulls()
		{
			return game.Statics.Hulls.Values.Select(x => new HullInfo(x, x.HighestLevel(playersTechLevels)));
		}
		
		public ArmorInfo Armor
		{
			get { return this.armorInfo; }
		}
		
		public IsDriveInfo AvailableIsDrive
		{
			get { return this.availableIsDrive; }
		}

		public ReactorInfo Reactor
		{
			get { return this.reactorInfo; }
		}
		#endregion
		
		#region Selected components
		private ArmorInfo armorInfo = null;
		private HullInfo selectedHull = null;
		private IsDriveInfo availableIsDrive = null;
		private ReactorInfo reactorInfo = null;

		void onHullChange()
		{
			if (this.ImageIndex < 0 || this.ImageIndex >= this.selectedHull.ImagePaths.Length)
				this.ImageIndex = 0;

			this.reactorInfo = bestReactor();
			this.availableIsDrive = bestIsDrive();
			this.HasIsDrive &= availableIsDrive != null;
		}

		#endregion

		#region Extra info
		public double PowerUsed
		{
			get { return 0; } //TODO(v0.5)
		}
		
		public double HitPoints
		{
			get 
			{
				var vars = new Var("hullHp", selectedHull.HitPointsBase).
					And("armorFactor", armorInfo.ArmorFactor).Get;
				
				return game.Statics.ShipFormulas.HitPoints.Evaluate(vars);
			}
		}
		#endregion

		#region Designer actions
		public string Name { get; set; } 
		public int ImageIndex { get; set; }
		
		public HullInfo Hull 
		{ 
			get { return this.selectedHull; }
			set
			{
				this.selectedHull = value;
				this.onHullChange();
			}
		}
		public bool HasIsDrive { get; set; }
		
		public bool IsDesignValid
		{
			get
			{
				//TODO(v0.5): check name length and uniqueness
				//TODO(v0.5): check image index
				return this.selectedHull != null && this.ImageIndex >= 0 && this.ImageIndex < this.selectedHull.ImagePaths.Length &&
					(this.availableIsDrive != null || !this.HasIsDrive);
			}
		}
		
		public void Commit()
		{
			if (!IsDesignValid)
				return;
			
			var desing = new Design(
				this.game.States.MakeDesignId(),
				this.game.CurrentPlayer,
				this.Name,
				this.ImageIndex,
				new Component<ArmorType>(this.armorInfo.Type, this.armorInfo.Level),
				new Component<HullType>(this.selectedHull.Type, this.selectedHull.Level),
				this.HasIsDrive ? new Component<IsDriveType>(this.availableIsDrive.Type, this.availableIsDrive.Level) : null,
				new Component<ReactorType>(this.reactorInfo.Type, this.reactorInfo.Level)
			);
			game.States.Designs.Add(desing); //TODO(v0.5) add to changes DB and propagate to states during turn processing
			game.Derivates.Players.Of(this.game.CurrentPlayer).Analyze(desing);
		}
		#endregion
	}
}
