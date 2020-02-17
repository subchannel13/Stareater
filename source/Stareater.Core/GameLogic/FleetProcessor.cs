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
		public const string LaneKey = "lane";

		public static Dictionary<Design, long> FillCarriers(PlayerProcessor playerProc, Dictionary<Design, long> shipGroups)
		{
			var designStats = playerProc.DesignStats;
			var uncarried = new PriorityQueue<KeyValuePair<Design, long>>();
			var carryOrder = shipGroups.Keys.
				OrderBy(x => designStats[x].GalaxySpeed).
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
					}

					tooBig.AddRange(sameSpeed);
				}

				uncarried.Enqueue(new KeyValuePair<Design, long>(design, shipGroups[design]), designStats[design].GalaxySpeed);
				foreach (var untransported in tooBig)
					uncarried.Enqueue(untransported, designStats[untransported.Key].GalaxySpeed);
			}

			return uncarried.ToDictionary(x => x.Key, x => x.Value);
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
						new Var("baseSpeed", designStats[x.Key].GalaxySpeed).
						And("size", designStats[x.Key].Size).
						And("towSize", 0).
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
	}
}
