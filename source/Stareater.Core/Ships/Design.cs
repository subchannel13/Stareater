 
using Ikadn.Ikon.Types;
using System;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Ships 
{
	partial class Design 
	{
		public string IdCode { get; private set; }
		public Player Owner { get; private set; }
		public string Name { get; private set; }
		public Hull Hull { get; private set; }
		public double Cost { get; private set; }

		public Design(string idCode, Player owner, string name, Hull hull) 
		{
			this.IdCode = idCode;
			this.Owner = owner;
			this.Name = name;
			this.Hull = hull;
			initCost(hull);
 
		} 

		private Design(Design original, Player owner) 
		{
			this.IdCode = original.IdCode;
			this.Owner = owner;
			this.Name = original.Name;
			this.Hull = original.Hull;
			this.Cost = original.Cost;
 
		}

		internal Design Copy(PlayersRemap playersRemap) 
		{
			return new Design(this, playersRemap.Players[this.Owner]);
 
		} 
 

		#region Saving
		public  IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(IdCodeKey, new IkonText(this.IdCode));

			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			data.Add(NameKey, new IkonText(this.Name));

			data.Add(HullKey, this.Hull.Save());
			return data;
 
		}

		private const string TableTag = "Design";
		private const string IdCodeKey = "idCode";
		private const string OwnerKey = "owner";
		private const string NameKey = "name";
		private const string HullKey = "hull";
		private const string CostKey = "cost";
 
		#endregion
	}
}
