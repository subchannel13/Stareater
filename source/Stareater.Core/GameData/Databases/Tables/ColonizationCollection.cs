﻿using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ColonizationCollection : AIndexedCollection<ColonizationProject>
	{
		public CollectionIndex<ColonizationProject, Player> OwnedBy { get; private set; }
		public CollectionIndex<ColonizationProject, Planet> Of { get; private set; }
		Dictionary<Player, List<ColonizationProject>> OwnedByIndex = new Dictionary<Player, List<ColonizationProject>>();
		Dictionary<Planet, List<ColonizationProject>> OfIndex = new Dictionary<Planet, List<ColonizationProject>>();

		public ColonizationCollection()
		{
			this.OwnedBy = new CollectionIndex<ColonizationProject, Player>(x => x.Owner);
			this.Of = new CollectionIndex<ColonizationProject, Planet>(x => x.Destination);
			
			this.RegisterIndex(this.OwnedBy);
			this.RegisterIndex(this.Of);
		}
	}
}
