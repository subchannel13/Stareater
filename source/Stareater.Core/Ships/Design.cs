using System;
using System.Collections.Generic;
using Stareater.Players;
using Stareater.Utils.Collections;

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

		
		public double Cost { get; private set; }
		
		//public int id { get; private set; } //TODO: might need id
		//private Dictionary<string, double> efekti = new Dictionary<string,double>(); //TODO might need
		//public object Hash { get; private set; } //TODO: make type, might need
		
		public Design(Player owner, string name, Hull hull)
		{
			this.Owner = owner;
			this.Hull = hull;
			this.Name = name;
			
			this.Cost = hull.TypeInfo.Cost.Evaluate(new Var("lvl", hull.Level).Get);
		}
		
		public string ImagePath 
		{ 
			get
			{
				return Hull.ImagePath;
			}
		}
	}
}
