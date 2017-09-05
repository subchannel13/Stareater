using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using System.Drawing;
using Stareater.GameData;
using Stareater.GameData.Ships;
using Ikadn.Ikon.Types;
using Stareater.Players.Natives;

namespace Stareater.Players
{
	partial class Player 
	{
		[StateProperty]
		public string Name { get; private set; }

		[StateProperty]
		public Color Color { get; private set; }

		[StateProperty]
		public PlayerControlType ControlType { get; private set; }

		[StateProperty(false)]
		public IOffscreenPlayer OffscreenControl { get; private set; }

		[StateProperty]
		public HashSet<PredefinedDesign> UnlockedDesigns { get; private set; }

		[StateProperty]
		public Intelligence Intelligence { get; private set; }

		public Player(string name, Color color, PlayerType type) 
		{
			this.Name = name;
			this.Color = color;

			this.ControlType = type.ControlType;
			this.OffscreenControl = type.OffscreenPlayerFactory != null ? type.OffscreenPlayerFactory.Create() : null;

			this.UnlockedDesigns = new HashSet<PredefinedDesign>();
			this.Intelligence = new Intelligence();
 
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

		private Organization organization; //TODO(v0.7) add to type
	}
}
