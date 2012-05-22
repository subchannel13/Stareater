using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra.Bitka;
using Zvjezdojedac.Igra;

namespace Zvjezdojedac.GUI.SmallData
{
	class FuzzyCombatPosition
	{
		const int HashDeltaFactor = (int)CombatSlotDelta.N;

		public int slot { get; private set; }
		public CombatSlotDelta delta { get; private set; }

		public FuzzyCombatPosition(int slot, CombatSlotDelta delta)
		{
			this.delta = delta;
			this.slot = slot;
		}

		public FuzzyCombatPosition(Borac combatant, int maxPozicija, HashSet<Igrac> leftSidePlayers)
		{
			this.slot = Math.Min((int)Math.Round(combatant.Pozicija), maxPozicija);
			if (!leftSidePlayers.Contains(combatant.Igrac))
				slot *= -1;

			double direction = Math.Sign(combatant.CiljnaPozicija - combatant.Pozicija);
			if (direction == 0)
				if (leftSidePlayers.Contains(combatant.Igrac))
					this.delta = CombatSlotDelta.StationaryLeft;
				else
					this.delta = CombatSlotDelta.StationaryRigth;

			else if (combatant.Pozicija >= this.slot && direction > 0) this.delta = CombatSlotDelta.LeftOutgoing;
			else if (combatant.Pozicija < this.slot && direction > 0) this.delta = CombatSlotDelta.RightIncoming;
			else if (combatant.Pozicija > this.slot && direction < 0) this.delta = CombatSlotDelta.LeftIncoming;
			else if (combatant.Pozicija <= this.slot && direction < 0) this.delta = CombatSlotDelta.RightOutgoing;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != this.GetType()) return false;

			return this.Equals((FuzzyCombatPosition)obj);
		}

		public bool Equals(FuzzyCombatPosition obj)
		{
			return this.slot == obj.slot && this.delta == obj.delta;
		}

		public override int GetHashCode()
		{
			return slot * HashDeltaFactor + (int)delta;
		}
	}
}
