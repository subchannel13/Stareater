 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using Stareater.GameData;
using Stareater.GameData.Ships;
using Stareater.Players;

namespace Stareater.Ships 
{
	partial class Design 
	{
		public string IdCode { get; private set; }
		public Player Owner { get; private set; }
		public string Name { get; private set; }
		private int imageIndex;
		public Component<ArmorType> Armor { get; private set; }
		public Component<HullType> Hull { get; private set; }
		public Component<IsDriveType> IsDrive { get; private set; }
		public Component<ReactorType> Reactor { get; private set; }
		public Component<SensorType> Sensors { get; private set; }
		public Component<ThrusterType> Thrusters { get; private set; }
		public double Cost { get; private set; }

		public Design(string idCode, Player owner, string name, int imageIndex, Component<ArmorType> armor, Component<HullType> hull, Component<IsDriveType> isDrive, Component<ReactorType> reactor, Component<SensorType> sensors, Component<ThrusterType> thrusters) 
		{
			this.IdCode = idCode;
			this.Owner = owner;
			this.Name = name;
			this.imageIndex = imageIndex;
			this.Armor = armor;
			this.Hull = hull;
			this.IsDrive = isDrive;
			this.Reactor = reactor;
			this.Sensors = sensors;
			this.Thrusters = thrusters;
			this.Cost = initCost();
 
			 
		} 

		private Design(Design original, Player owner) 
		{
			this.IdCode = original.IdCode;
			this.Owner = owner;
			this.Name = original.Name;
			this.imageIndex = original.imageIndex;
			this.Armor = original.Armor;
			this.Hull = original.Hull;
			this.IsDrive = original.IsDrive;
			this.Reactor = original.Reactor;
			this.Sensors = original.Sensors;
			this.Thrusters = original.Thrusters;
			this.Cost = original.Cost;
 
			 
		}

		private Design(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var idCodeSave = rawData[IdCodeKey];
			this.IdCode = idCodeSave.To<string>();

			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var nameSave = rawData[NameKey];
			this.Name = nameSave.To<string>();

			var imageIndexSave = rawData[ImageIndexKey];
			this.imageIndex = imageIndexSave.To<int>();

			var armorSave = rawData[ArmorKey];
			this.Armor = Component<ArmorType>.Load(armorSave.To<IkonArray>(), deindexer);

			var hullSave = rawData[HullKey];
			this.Hull = Component<HullType>.Load(hullSave.To<IkonArray>(), deindexer);

			var isDriveSave = rawData[IsDriveKey];
			this.IsDrive = Component<IsDriveType>.Load(isDriveSave.To<IkonArray>(), deindexer);

			var reactorSave = rawData[ReactorKey];
			this.Reactor = Component<ReactorType>.Load(reactorSave.To<IkonArray>(), deindexer);

			var sensorsSave = rawData[SensorsKey];
			this.Sensors = Component<SensorType>.Load(sensorsSave.To<IkonArray>(), deindexer);

			var thrustersSave = rawData[ThrustersKey];
			this.Thrusters = Component<ThrusterType>.Load(thrustersSave.To<IkonArray>(), deindexer);

			this.Cost = initCost();
 
			 
		}

		internal Design Copy(PlayersRemap playersRemap) 
		{
			return new Design(this, playersRemap.Players[this.Owner]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(IdCodeKey, new IkonText(this.IdCode));

			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			data.Add(NameKey, new IkonText(this.Name));

			data.Add(ImageIndexKey, new IkonInteger(this.imageIndex));

			data.Add(ArmorKey, this.Armor.Save());

			data.Add(HullKey, this.Hull.Save());

			data.Add(IsDriveKey, this.IsDrive.Save());

			data.Add(ReactorKey, this.Reactor.Save());

			data.Add(SensorsKey, this.Sensors.Save());

			data.Add(ThrustersKey, this.Thrusters.Save());
			return data;
 
		}

		public static Design Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new Design(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "Design";
		private const string IdCodeKey = "idCode";
		private const string OwnerKey = "owner";
		private const string NameKey = "name";
		private const string ImageIndexKey = "imageIndex";
		private const string ArmorKey = "armor";
		private const string HullKey = "hull";
		private const string IsDriveKey = "isDrive";
		private const string ReactorKey = "reactor";
		private const string SensorsKey = "sensors";
		private const string ThrustersKey = "thrusters";
		private const string CostKey = "cost";
 
		#endregion

 
	}
}
