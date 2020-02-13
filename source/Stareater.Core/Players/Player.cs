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
		[StatePropertyAttribute]
		public string Name { get; private set; }

		[StatePropertyAttribute]
		public Color Color { get; private set; }

		[StatePropertyAttribute]
		public Organization Organization { get; private set; }

		[StatePropertyAttribute]
		public PlayerControlType ControlType { get; private set; }

		[StatePropertyAttribute(false)]
		public IOffscreenPlayer OffscreenControl { get; private set; }

		[StatePropertyAttribute]
		public HashSet<DesignTemplate> UnlockedDesigns { get; private set; }
		
		[StatePropertyAttribute]
		public Intelligence Intelligence { get; private set; }

		[StatePropertyAttribute]
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

			switch (tag)
			{
				case PlayerType.NoControllerTag:
					return null;
				case PlayerType.OrganelleControllerTag:
					return new OrganellePlayerFactory().Load(rawData.To<IkonComposite>(), session);
				default:
					if (PlayerAssets.AIDefinitions.TryGetValue(tag, out var factory))
						return factory.Load(rawData.To<IkonComposite>(), session);
					else
						throw new KeyNotFoundException("Can't load player controller for " + tag);
			}
		}

		#region object ID
#if DEBUG
		private long id { get; set; }

		public override string ToString()
		{
			return "Player " + id;
		}

		private static long LastId = 0;
		private static readonly object LockObj = new object();

		private static long NextId()
		{
			lock (LockObj) {
				LastId++;
				return LastId;
			}
		}
#endif
		#endregion
	}
}
