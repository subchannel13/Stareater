 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData;
using Stareater.Players.Reports;

namespace Stareater.Players 
{
	partial class Player 
	{
		public string Name { get; private set; }
		public Color Color { get; private set; }
		public PlayerControlType ControlType { get; private set; }
		public IOffscreenPlayer OffscreenControl { get; private set; }
		public HashSet<PredefinedDesign> UnlockedDesigns { get; private set; }
		public Intelligence Intelligence { get; private set; }
		public PlayerOrders Orders { get; set; }
		public List<IReport> Reports { get; private set; }

		public Player(string name, Color color, PlayerType type) 
		{
			this.Name = name;
			this.Color = color;
			initPlayerControl(type);
			
			this.UnlockedDesigns = new HashSet<PredefinedDesign>();
			this.Intelligence = new Intelligence();
			this.Orders = new PlayerOrders();
			this.Reports = new List<IReport>();
 
		} 

		private Player(Player original, GalaxyRemap galaxyRemap) 
		{
			this.Name = original.Name;
			this.Color = original.Color;
			copyPlayerControl(original);
			
			this.UnlockedDesigns = new HashSet<PredefinedDesign>();
			foreach(var item in original.UnlockedDesigns)
				this.UnlockedDesigns.Add(item);
			this.Intelligence = original.Intelligence.Copy(galaxyRemap);
			
			this.Reports = new List<IReport>();
			foreach(var item in original.Reports)
				this.Reports.Add(item);
 
		}

		private Player(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var nameSave = rawData[NameKey];
			this.Name = nameSave.To<string>();

			var colorSave = rawData[ColorKey];
			var colorArray = colorSave.To<IkonArray>();
			int colorR = colorArray[0].To<int>();
			int colorG = colorArray[1].To<int>();
			int colorB = colorArray[2].To<int>();
			this.Color = Color.FromArgb(colorR, colorG, colorB);

			var controlTypeSave = rawData[ControlTypeKey];
			this.ControlType = (PlayerControlType)Enum.Parse(typeof(PlayerControlType), (string)controlTypeSave.Tag);

			var offscreenControlSave = rawData[OffscreenControlKey];
			this.OffscreenControl = loadControl(offscreenControlSave);

			var unlockedDesignsSave = rawData[UnlockedDesignsKey];
			this.UnlockedDesigns = new HashSet<PredefinedDesign>();
			foreach(var item in unlockedDesignsSave.To<IkonArray>())
				this.UnlockedDesigns.Add(deindexer.Get<PredefinedDesign>(item.To<int>()));

			var intelligenceSave = rawData[IntelligenceKey];
			this.Intelligence = Intelligence.Load(intelligenceSave.To<IkonComposite>(), deindexer);

			var reportsSave = rawData[ReportsKey];
			this.Reports = loadReports(reportsSave);
 
		}

		internal Player Copy(GalaxyRemap galaxyRemap) 
		{
			return new Player(this, galaxyRemap);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
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
			data.Add(UnlockedDesignsKey, unlockedDesignsData);

			data.Add(IntelligenceKey, this.Intelligence.Save(indexer));

			var reportsData = new IkonArray();
			foreach(var item in this.Reports)
				reportsData.Add(item.Save(indexer));
			data.Add(ReportsKey, reportsData);
			return data;
 
		}

		public static Player Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new Player(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "Player";
		private const string NameKey = "name";
		private const string ColorKey = "color";
		private const string ControlTypeKey = "controlType";
		private const string OffscreenControlKey = "offscreenControl";
		private const string UnlockedDesignsKey = "unlockedDesigns";
		private const string IntelligenceKey = "intelligence";
		private const string OrdersKey = "orders";
		private const string ReportsKey = "reports";
 
		#endregion
	}
}
