 


using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System;
using Stareater.Players;

namespace Stareater.GameData 
{
	partial class DevelopmentProgress 
	{
		[StateProperty]
		public Player Owner { get; private set; }
		[StateProperty]
		public DevelopmentTopic Topic { get; private set; }
		[StateProperty]
		public int Level { get; private set; }
		[StateProperty]
		public double InvestedPoints { get; private set; }
		[StateProperty]
		public int Priority { get; set; }

		public DevelopmentProgress(Player owner, DevelopmentTopic topic, int level, double investedPoints, int priority) 
		{
			this.Owner = owner;
			this.Topic = topic;
			this.Level = level;
			this.InvestedPoints = investedPoints;
			this.Priority = priority;
 
			 
		} 


		private DevelopmentProgress(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var topicSave = rawData[TopicKey];
			this.Topic = deindexer.Get<DevelopmentTopic>(topicSave.To<string>());

			var levelSave = rawData[LevelKey];
			this.Level = levelSave.To<int>();

			var investedPointsSave = rawData[InvestedKey];
			this.InvestedPoints = investedPointsSave.To<double>();

			var prioritySave = rawData[PriorityKey];
			this.Priority = prioritySave.To<int>();
 
			 
		}

		private DevelopmentProgress() 
		{ }
		internal DevelopmentProgress Copy(PlayersRemap playersRemap) 
		{
			return new DevelopmentProgress(playersRemap.Players[this.Owner], this.Topic, this.Level, this.InvestedPoints, this.Priority);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			data.Add(TopicKey, new IkonText(this.Topic.IdCode));

			data.Add(LevelKey, new IkonInteger(this.Level));

			data.Add(InvestedKey, new IkonFloat(this.InvestedPoints));

			data.Add(PriorityKey, new IkonInteger(this.Priority));
			return data;
 
		}

		public static DevelopmentProgress Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new DevelopmentProgress(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "DevelopmentProgress";
		private const string OwnerKey = "owner";
		private const string TopicKey = "topic";
		private const string LevelKey = "level";
		private const string InvestedKey = "invested";
		private const string PriorityKey = "priority";
 
		#endregion

 
	}
}
