using System;

namespace Stareater.Players
{
	public class PlayerType
	{
		public PlayerControlType ControlType { get; private set; }
		public string Name { get; private set; }
		public IOffscreenPlayerFactory OffscreenPlayerFactory { get; private set; }

		public PlayerType(PlayerControlType controlType, IOffscreenPlayerFactory offscreenPlayerFactory)
		{
			if (offscreenPlayerFactory == null)
				throw new ArgumentNullException(nameof(offscreenPlayerFactory));

			this.ControlType = controlType;
			this.Name = offscreenPlayerFactory.Name;
			this.OffscreenPlayerFactory = offscreenPlayerFactory;
		}

		public PlayerType(PlayerControlType controlType, string name)
		{
			this.ControlType = controlType;
			this.Name = name;
			this.OffscreenPlayerFactory = null;
		}

		public PlayerType(PlayerControlType controlType, IOffscreenPlayerFactory offscreenPlayerFactory, string name) : 
			this(controlType, offscreenPlayerFactory)
		{
			this.Name = name;
		}

		public override bool Equals(object obj)
		{
			return obj is PlayerType other && ControlType.Equals(other.ControlType) && OffscreenPlayerFactory == other.OffscreenPlayerFactory;
		}

		public override int GetHashCode()
		{
			return ControlType.GetHashCode();
		}
		
		public const string AiControllerTag = "Ai";
		public const string NoControllerTag = "None";
		public const string OrganelleControllerTag = "Organelle";
		public const string FactoryIdKey = "factory";
	}
}
