 

using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using Stareater.GameData.Databases;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Players 
{
	partial class Player 
	{
		public string Name { get; private set; }
		public Color Color { get; private set; }
		public PlayerControlType ControlType { get; private set; }
		public IOffscreenPlayer OffscreenControl { get; private set; }
		public ICollection<PredefinedDesign> UnlockedDesigns { get; private set; }
		public Intelligence Intelligence { get; private set; }
		public PlayerOrders Orders { get; set; }

		public Player(string name, Color color, PlayerType type) 
		{
			this.Name = name;
			this.Color = color;
			initPlayerControl(type);
			
			this.UnlockedDesigns = new HashSet<PredefinedDesign>();
			this.Intelligence = new Intelligence();
			this.Orders = new PlayerOrders();
 
		} 

		private Player(Player original, GalaxyRemap galaxyRemap) 
		{
			this.Name = original.Name;
			this.Color = original.Color;
			copyPlayerControl(original);
			
			copyDesigns(original);
			this.Intelligence = original.Intelligence.Copy(galaxyRemap);
			//TODO(v0.5) copy orders
 
		}
		internal Player Copy(GalaxyRemap galaxyRemap) 
		{
			return new Player(this, galaxyRemap);
 
		} 
 

		#region Saving
		public  IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(NameKey, new IkonText(this.Name));

			var colorData = new IkonArray();
			colorData.Add(new IkonInteger(this.Color.R));
			colorData.Add(new IkonInteger(this.Color.G));
			colorData.Add(new IkonInteger(this.Color.B));
			data.Add(ColorKey, colorData);

			data.Add(ControlTypeKey, new IkonComposite(this.ControlType.ToString()));

			data.Add(OffscreenControlKey, this.OffscreenControl != null ? this.OffscreenControl.Save() : new IkonComposite("None"));

			var unlockedDesignsData = new IkonArray();
			foreach(var item in this.UnlockedDesigns)
				unlockedDesignsData.Add(new IkonInteger(indexer.IndexOf(item)));
			data.Add(Collection, unlockedDesignsData);

			data.Add(IntelligenceKey, this.Intelligence.Save(indexer));
			return data;
 
		}

		private const string TableTag = "Player"; 
		private const string NameKey = "name";
		private const string ColorKey = "color";
		private const string ControlTypeKey = "controlType";
		private const string OffscreenControlKey = "offscreenControl";
		private const string Collection = "unlockedDesigns";
		private const string IntelligenceKey = "intelligence";
		private const string OrdersKey = "orders";
 
		#endregion
	}
}
