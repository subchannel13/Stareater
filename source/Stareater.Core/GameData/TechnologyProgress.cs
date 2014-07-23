 
using Ikadn.Ikon.Types;
using System;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.GameData 
{
	partial class TechnologyProgress 
	{
		public Player Owner { get; private set; }
		public Technology Topic { get; private set; }
		public int Level { get; private set; }
		public double InvestedPoints { get; private set; }

		public TechnologyProgress(Player owner, Technology topic, int level, double investedPoints) 
		{
			this.Owner = owner;
			this.Topic = topic;
			this.Level = level;
			this.InvestedPoints = investedPoints;
 
		} 


		internal TechnologyProgress Copy(PlayersRemap playersRemap) 
		{
			return new TechnologyProgress(playersRemap.Players[this.Owner], this.Topic, this.Level, this.InvestedPoints);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			IkonComposite data = new IkonComposite(TableTag);
			
			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			data.Add(TopicKey, new IkonText(this.Topic.IdCode));

			data.Add(LevelKey, new IkonInteger(this.Level));

			data.Add(InvestedKey, new IkonFloat(this.InvestedPoints));
 

			return data;
		}

		private const string TableTag = "TechnologyProgress"; 
		private const string OwnerKey = "owner";
		private const string TopicKey = "topic";
		private const string LevelKey = "level";
		private const string InvestedKey = "invested";
 
		#endregion
	}
}
