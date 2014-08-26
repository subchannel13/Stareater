 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using Stareater.Players;

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


		private  TechnologyProgress(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var topicSave = rawData[TopicKey];
			this.Topic = deindexer.Get<Technology>(topicSave.To<string>());

			var levelSave = rawData[LevelKey];
			this.Level = levelSave.To<int>();

			var investedPointsSave = rawData[InvestedKey];
			this.InvestedPoints = investedPointsSave.To<double>();
 
		}

		internal TechnologyProgress Copy(PlayersRemap playersRemap) 
		{
			return new TechnologyProgress(playersRemap.Players[this.Owner], this.Topic, this.Level, this.InvestedPoints);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			data.Add(TopicKey, new IkonText(this.Topic.IdCode));

			data.Add(LevelKey, new IkonInteger(this.Level));

			data.Add(InvestedKey, new IkonFloat(this.InvestedPoints));
			return data;
 
		}
		
		public static TechnologyProgress Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new TechnologyProgress(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}

		private const string TableTag = "TechnologyProgress";
		private const string OwnerKey = "owner";
		private const string TopicKey = "topic";
		private const string LevelKey = "level";
		private const string InvestedKey = "invested";
 
		#endregion
	}
}
