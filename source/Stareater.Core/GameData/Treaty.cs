 


using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System;
using Stareater.Players;

namespace Stareater.GameData 
{
	class Treaty 
	{
		[StateProperty]
		public Player Party1 { get; private set; }
		[StateProperty]
		public Player Party2 { get; private set; }

		public Treaty(Player party1, Player party2) 
		{
			this.Party1 = party1;
			this.Party2 = party2;
 
			 
		} 


		private Treaty(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var party1Save = rawData[Party1Key];
			this.Party1 = deindexer.Get<Player>(party1Save.To<int>());

			var party2Save = rawData[Party2Key];
			this.Party2 = deindexer.Get<Player>(party2Save.To<int>());
 
			 
		}

		private Treaty() 
		{ }
		internal Treaty Copy(PlayersRemap playersRemap) 
		{
			return new Treaty(playersRemap.Players[this.Party1], playersRemap.Players[this.Party2]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(Party1Key, new IkonInteger(indexer.IndexOf(this.Party1)));

			data.Add(Party2Key, new IkonInteger(indexer.IndexOf(this.Party2)));
			return data;
 
		}

		public static Treaty Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new Treaty(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "Treaty";
		private const string Party1Key = "party1";
		private const string Party2Key = "party2";
 
		#endregion

 
	}
}
