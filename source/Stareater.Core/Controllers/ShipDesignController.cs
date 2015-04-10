using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Controllers.Views.Ships;
using Stareater.GameData;
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
		}

		private IsDriveInfo bestIsDrive()
		{
			//TODO(0.5) calculate ship's power
			var drive = Stareater.Ships.IsDrive.Best(
				game.Statics.IsDrives.Values, 
				playersTechLevels, 
				new Hull(this.selectedHull.HullType, this.selectedHull.Level, this.ImageIndex), 
				0);
			
			return drive != null ? new IsDriveInfo(drive.TypeInfo, drive.Level, this.selectedHull, 0) : null;
		}
		
		#region Component lists
		public IEnumerable<HullInfo> Hulls()
		{
			return game.Statics.Hulls.Values.Select(x => new HullInfo(x, x.HighestLevel(playersTechLevels)));
		}
		
		public IsDriveInfo AvailableIsDrive
		{
			get { return this.availableIsDrive; }
		}
		#endregion
		
		#region Selected components
		private HullInfo selectedHull = null;
		private IsDriveInfo availableIsDrive = null;

		void onHullChange()
		{
			if (this.ImageIndex < 0 || this.ImageIndex >= this.selectedHull.ImagePaths.Length)
				this.ImageIndex = 0;
			
			this.availableIsDrive = bestIsDrive();
			this.HasIsDrive &= availableIsDrive != null;
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
			
			game.States.Designs.Add(new Design(
				game.States.MakeDesignId(),
				game.CurrentPlayer,
				Name,
				new Hull(this.selectedHull.HullType, this.selectedHull.Level, this.ImageIndex),
				this.HasIsDrive ? new IsDrive(this.availableIsDrive.IsDriveType, this.availableIsDrive.Level) : null
			)); //TODO(v0.5) add to changes DB and propagate to states during turn processing
		}
		#endregion
	}
}
