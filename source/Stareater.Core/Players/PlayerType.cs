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

		public PlayerType(PlayerControlType controlType, string name)
		{
			this.ControlType = controlType;
			this.Name = name;
		}

		public override bool Equals(object obj)
		{
			PlayerType other = obj as PlayerType;
			if (other == null)
				return false;

			return ControlType.Equals(other.ControlType);
		}

		public override int GetHashCode()
		{
			return ControlType.GetHashCode();
		}
	}
}
