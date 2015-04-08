 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Ships 
{
	partial class Design 
	{
		public string IdCode { get; private set; }
		public Player Owner { get; private set; }
		public string Name { get; private set; }
		public Hull Hull { get; private set; }
		public IsDrive IsDrive { get; private set; }
		public double Cost { get; private set; }

		public Design(string idCode, Player owner, string name, Hull hull, IsDrive isDrive) 
		{
			this.IdCode = idCode;
			this.Owner = owner;
			this.Name = name;
			this.Hull = hull;
			this.IsDrive = isDrive;
			this.Cost = initCost();
 
			 
		} 

		private Design(Design original, Player owner) 
		{
			this.IdCode = original.IdCode;
			this.Owner = owner;
			this.Name = original.Name;
			this.Hull = original.Hull;
			this.IsDrive = original.IsDrive;
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

			var hullSave = rawData[HullKey];
			this.Hull = Hull.Load(hullSave.To<IkonComposite>(), deindexer);

			var isDriveSave = rawData[IsDriveKey];
			this.IsDrive = IsDrive.Load(isDriveSave.To<IkonComposite>(), deindexer);

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

			data.Add(HullKey, this.Hull.Save());

			data.Add(IsDriveKey, this.IsDrive.Save());
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
		private const string HullKey = "hull";
		private const string IsDriveKey = "isDrive";
		private const string CostKey = "cost";
 
		#endregion

 
	}
}
