using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Players
{
	public class PlayerType
	{
		public PlayerControlType ControlType { get; private set; }
		public string Name { get; private set; }
		public IOffscreenPlayerFactory OffscreenPlayerFactory { get; private set; }

		public PlayerType(PlayerControlType controlType, IOffscreenPlayerFactory offscreenPlayerFactory)
		{
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

		public override bool Equals(object obj)
		{
			var other = obj as PlayerType;
			return other != null && ControlType.Equals(other.ControlType) && OffscreenPlayerFactory == other.OffscreenPlayerFactory;
		}

		public override int GetHashCode()
		{
			return ControlType.GetHashCode();
		}
		
		public const string AiControllerTag = "Ai";
		public const string NoControllerTag = "None";
		public const string FactoryIdKey = "factory";
	}
}
