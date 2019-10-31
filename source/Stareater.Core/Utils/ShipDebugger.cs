using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Ships;

namespace Stareater.Utils
{
#if DEBUG
	/// <summary>
	/// Tool for debugging ship count inconsistencies
	/// </summary>
	class ShipDebugger
	{
		private readonly Dictionary<Design, long> designs = new Dictionary<Design, long>();

		public ShipDebugger(IEnumerable<Fleet> fleets)
		{
			foreach(var fleet in fleets)
				foreach(var group in fleet.Ships)
				{
					if (!this.designs.ContainsKey(group.Design))
						this.designs[group.Design] = 0;

					this.designs[group.Design] += group.Quantity;
				}
		}

		internal void Check(string title, IEnumerable<Fleet> fleets)
		{
			var state = new Dictionary<Design, long>();

			foreach (var fleet in fleets)
				foreach (var group in fleet.Ships)
				{
					if (!state.ContainsKey(group.Design))
						state[group.Design] = 0;

					state[group.Design] += group.Quantity;
				}

			var errors = new List<string>();
			foreach(var design in this.designs)
			{
				var newCount = state.ContainsKey(design.Key) ? state[design.Key] : 0;

				if (newCount != design.Value)
					errors.Add($"{design.Key.Name} of {design.Key.Owner.Name} off by {newCount - design.Value}");
			}

			foreach (var design in state.Keys.Except(this.designs.Keys))
				errors.Add($"{design.Name} of {design.Owner.Name} is new");

			if (errors.Any())
			{
				Trace.WriteLine(title);
				foreach (var message in errors)
					Trace.WriteLine(message);
			}
		}

		internal void Add(Design design, long quantity)
		{
			if (!this.designs.ContainsKey(design))
				this.designs[design] = 0;

			this.designs[design] += quantity;
		}
	}
#endif
}
