using Stareater.GameData.Databases;
using Stareater.Ships;
using Stareater.Utils;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.GameLogic
{
	class FleetProcessor
	{
		public static Dictionary<Design, long> FillCarriers(StaticsDB statics, PlayerProcessor playerProc, Dictionary<Design, long> shipGroups)
		{
			var designStats = playerProc.DesignStats;
			var uncarried = new PriorityQueue<KeyValuePair<Design, long>>();
			var carriedTows = new Stack<KeyValuePair<Design, long>>();
			var carryOrder = shipGroups.Keys.
				OrderBy(x => statics.ShipFormulas.GalaxySpeed.Evaluate(
					new Var(SpeedKey, designStats[x].GalaxySpeed).
					And(SizeKey, designStats[x].Size).
					And(TowKey, designStats[x].TowCapacity).
					And(LaneKey, false).Get
				)).
				ThenByDescending(x => designStats[x].Size);

			foreach (var design in carryOrder)
			{
				var carryCapacity = designStats[design].CarryCapacity * shipGroups[design];
				var tooBig = new List<KeyValuePair<Design, long>>();

				while (carryCapacity > 0 && uncarried.Count > 0)
				{
					var priority = uncarried.PeekPriority();
					var sameSpeed = new PriorityQueue<KeyValuePair<Design, long>>();
					while (uncarried.Count > 0 && uncarried.PeekPriority() == priority)
					{
						var group = uncarried.Dequeue();
						sameSpeed.Enqueue(group, designStats[group.Key].Size);
					}

					while (carryCapacity > 0 && sameSpeed.Count > 0)
					{
						var group = sameSpeed.Dequeue();
						var maxTransportable = (long)Math.Floor(carryCapacity / designStats[group.Key].Size);
						var transported = Math.Min(maxTransportable, group.Value);
						var untransported = group.Value - transported;

						carryCapacity -= transported * designStats[group.Key].Size;

						if (untransported > 0)
							tooBig.Add(new KeyValuePair<Design, long>(group.Key, untransported));
						if (designStats[group.Key].TowCapacity > 0 && transported > 0)
							carriedTows.Push(new KeyValuePair<Design, long>(group.Key, transported));
					}

					tooBig.AddRange(sameSpeed);
				}

				uncarried.Enqueue(new KeyValuePair<Design, long>(design, shipGroups[design]), designStats[design].GalaxySpeed);
				foreach (var untransported in tooBig)
					uncarried.Enqueue(untransported, designStats[untransported.Key].GalaxySpeed);
			}

			var result = uncarried.ToDictionary(x => x.Key, x => x.Value);
			var nonIsSize = uncarried.Where(x => x.Key.IsDrive == null).Sum(x => designStats[x.Key].Size * x.Value);
			nonIsSize -= uncarried.Where(x => x.Key.IsDrive != null).Sum(x => designStats[x.Key].TowCapacity * x.Value);

			while (nonIsSize > 0 && carriedTows.Count > 0)
			{
				var towGroup = carriedTows.Pop();
				var usedTows = (long)Math.Min(Math.Floor(nonIsSize / designStats[towGroup.Key].TowCapacity), towGroup.Value);

				if (usedTows <= 0)
					continue;
				if (!result.ContainsKey(towGroup.Key))
					result.Add(towGroup.Key, 0);

				result[towGroup.Key] += usedTows;
				nonIsSize -= usedTows * designStats[towGroup.Key].TowCapacity;
			}

			return result;
		}

		public static Var SpeedVars(StaticsDB statics, PlayerProcessor playerProc, Dictionary<Design, long> shipGroups, bool onLane)
		{
			var designStats = playerProc.DesignStats;
			var totalTows = shipGroups.
				Where(x => x.Key.IsDrive != null && designStats[x.Key].TowCapacity > 0).
				Sum(x => x.Value * designStats[x.Key].TowCapacity);

			if (totalTows <= 0)
				return Methods.FindWorst(
					shipGroups.Select(x =>
						new Var(SpeedKey, designStats[x.Key].GalaxySpeed).
						And(SizeKey, designStats[x.Key].Size).
						And(TowKey, 0).
						And(LaneKey, onLane)
					),
					x => statics.ShipFormulas.GalaxySpeed.Evaluate(x.Get)
				);

			var towedSize = shipGroups.
				Where(x => x.Key.IsDrive == null).
				Sum(x => x.Value * designStats[x.Key].Size);

			if (towedSize > totalTows)
				return new Var("baseSpeed", 0).And("size", 1).And("towSize", 0).And(LaneKey, onLane);

			return Methods.FindWorst(
				shipGroups.
				Where(x => x.Key.IsDrive != null).
				Select(x =>
					new Var("baseSpeed", designStats[x.Key].GalaxySpeed).
					And("size", designStats[x.Key].Size).
					And("towSize", towedSize * x.Value * designStats[x.Key].TowCapacity / totalTows).
					And(LaneKey, onLane)
				),
				x => statics.ShipFormulas.GalaxySpeed.Evaluate(x.Get)
			);
		}

		private const string SpeedKey = "baseSpeed";
		private const string SizeKey = "size";
		private const string TowKey = "towSize";
		private const string LaneKey = "lane";
	}
}
