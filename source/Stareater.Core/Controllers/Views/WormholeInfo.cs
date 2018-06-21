using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Views
{
	public class WormholeInfo
	{
		internal Wormhole Data { get; private set; }

		internal WormholeInfo(Wormhole data)
		{
			this.Data = data;
		}

		public Pair<StarInfo> Endpoints
		{
			get
			{
				return new Pair<StarInfo>(
					new StarInfo(this.Data.Endpoints.First),
					new StarInfo(this.Data.Endpoints.Second)
				);
			}
		}

		public override bool Equals(object obj)
		{
			var other = obj as WormholeInfo;
			return other != null &&
				   EqualityComparer<Wormhole>.Default.Equals(this.Data, other.Data);
		}

		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		public static bool operator ==(WormholeInfo info1, WormholeInfo info2)
		{
			return EqualityComparer<WormholeInfo>.Default.Equals(info1, info2);
		}

		public static bool operator !=(WormholeInfo info1, WormholeInfo info2)
		{
			return !(info1 == info2);
		}
	}
}
