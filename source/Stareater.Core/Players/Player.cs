using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.GameData.Ships;
using Stareater.Players.Natives;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using System.Drawing;

namespace Stareater.Players
{
	class Player 
	{
		[StateProperty]
		public string Name { get; private set; }

		[StateProperty]
		public Color Color { get; private set; }

		[StateProperty]
		public Organization Organization { get; private set; }

		[StateProperty]
		public PlayerControlType ControlType { get; private set; }

		[StateProperty(false)]
		public IOffscreenPlayer OffscreenControl { get; private set; }

		[StateProperty]
		public HashSet<DesignTemplate> UnlockedDesigns { get; private set; }
		
		[StateProperty]
		public Intelligence Intelligence { get; private set; }

		[StateProperty]
		public double VictoryPoints { get; set; }

		public Player(string name, Color color, Organization Organization, PlayerType type) 
		{
			this.Name = name;
			this.Color = color;
			this.Organization = Organization;

			this.ControlType = type.ControlType;
			this.OffscreenControl = type.OffscreenPlayerFactory?.Create();

			this.UnlockedDesigns = new HashSet<DesignTemplate>();
			this.Intelligence = new Intelligence();
			this.VictoryPoints = 0;
 
			#if DEBUG
			this.id = NextId();
			#endif
 
		} 
		
		private Player() 
		{ }

		private static IOffscreenPlayer loadControl(Ikadn.IkadnBaseObject rawData, LoadSession session)
		{
			var tag = rawData.Tag as string;

			if (tag.Equals(PlayerType.NoControllerTag))
				return null;
			else if (tag.Equals(PlayerType.OrganelleControllerTag))
				return new OrganellePlayerFactory().Load(rawData.To<IkonComposite>(), session);
			else if (PlayerAssets.AIDefinitions.ContainsKey(tag))
				return PlayerAssets.AIDefinitions[tag].Load(rawData.To<IkonComposite>(), session);

			throw new KeyNotFoundException("Can't load player controller for " + tag);
		}

		#region object ID
#if DEBUG
		private long id { get; set; }

		public override string ToString()
		{
			return "Player " + id;
		}

		private static long LastId = 0;

		private static long NextId()
		{
			lock (typeof(Player)) {
				LastId++;
				return LastId;
			}
		}
#endif
		#endregion
	}
}
