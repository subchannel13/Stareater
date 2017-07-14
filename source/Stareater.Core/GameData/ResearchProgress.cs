 


using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System;
using Stareater.Players;

namespace Stareater.GameData 
{
	partial class ResearchProgress 
	{
		[StateProperty]
		public Player Owner { get; private set; }
		[StateProperty]
		public ResearchTopic Topic { get; private set; }
		[StateProperty]
		public int Level { get; private set; }
		[StateProperty]
		public double InvestedPoints { get; private set; }

		public ResearchProgress(Player owner, ResearchTopic topic, int level, double investedPoints) 
		{
			this.Owner = owner;
			this.Topic = topic;
			this.Level = level;
			this.InvestedPoints = investedPoints;
 
			 
		} 


		private ResearchProgress(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var topicSave = rawData[TopicKey];
			this.Topic = deindexer.Get<ResearchTopic>(topicSave.To<string>());

			var levelSave = rawData[LevelKey];
			this.Level = levelSave.To<int>();

			var investedPointsSave = rawData[InvestedKey];
			this.InvestedPoints = investedPointsSave.To<double>();
 
			 
		}

		private ResearchProgress() 
		{ }
		internal ResearchProgress Copy(PlayersRemap playersRemap) 
		{
			return new ResearchProgress(playersRemap.Players[this.Owner], this.Topic, this.Level, this.InvestedPoints);
 
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

		public static ResearchProgress Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new ResearchProgress(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "ResearchProgress";
		private const string OwnerKey = "owner";
		private const string TopicKey = "topic";
		private const string LevelKey = "level";
		private const string InvestedKey = "invested";
 
		#endregion

 
	}
}
