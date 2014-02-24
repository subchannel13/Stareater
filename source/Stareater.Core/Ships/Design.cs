using System;
using System.Collections.Generic;
using Stareater.Players;

namespace Stareater.Ships
{
	internal class Design
	{
		public Player Owner { get; private set; }
		public string Name { get; private set; }
		
		public Hull Hull { get; private set; } //TODO: make type
		public object PrimaryEquip { get; private set; } //TODO: make type
		public object SecondaryEquip { get; private set; } //TODO: make type
		public object Armor { get; private set; } //TODO: make type
		public object Shield { get; private set; } //TODO: make type
		public Dictionary<object, int> SpecialEquip { get; private set; } //TODO: make type
		public object Sensors { get; private set; } //TODO: make type
		public object Thrusters { get; private set; } //TODO: make type
		public object ISDrive { get; private set; } //TODO: make type
		public object Reactor { get; private set; } //TODO: make type

		//public int id { get; private set; } //TODO: might need id
		
		//public Image ikona { get; private set; } //TODO: get from hull
		//public double cijena { get; private set; } //TODO: cache or derive?
		//private Dictionary<string, double> efekti = new Dictionary<string,double>(); //TODO might need
		
		//public object Hash { get; private set; } //TODO: make type, might need
		
		public Design(Player owner, string name, Hull hull)
		{
			this.Owner = owner;
			this.Hull = hull;
			this.Name = name;
		}
	}
}
