using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameData
{
	class SystemPolicy
	{
		[StateProperty]
		public string Id { get; private set; }

		[StateProperty]
		public string LangCode { get; private set; }

		[StateProperty]
		public double SpendingRatio { get; private set; }

		[StateProperty]
		public string[] Queue { get; private set; }

		public SystemPolicy(string id, string langCode, double spendingRatio, string[] queue)
		{
			this.Id = id;
			this.LangCode = langCode;
			this.SpendingRatio = spendingRatio;
			this.Queue = queue;
		}

		private SystemPolicy()
		{ }

		public override bool Equals(object obj)
		{
			var other = obj as SystemPolicy;
			return other != null &&
				   this.Id == other.Id;
		}

		public override int GetHashCode()
		{
			return 2108858624 + EqualityComparer<string>.Default.GetHashCode(this.Id);
		}
	}
}
