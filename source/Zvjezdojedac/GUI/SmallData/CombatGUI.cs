using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra.Bitka;

namespace Zvjezdojedac.GUI.SmallData
{
	class CombatGUI
	{
		public Borac Combatant { get; private set; }
		public FuzzyCombatPosition LeftSidePosition { get; private set; }

		public CombatGUI(Borac combatant, FuzzyCombatPosition fuzzyPosition)
		{
			this.Combatant = combatant; ;
			this.LeftSidePosition = fuzzyPosition;
		}
	}
}

