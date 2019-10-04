using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Localization.StarNames;
using Stareater.Utils;

namespace Stareater.Controllers.Views
{
	public class StarInfo
	{
		internal StarData Data { get; private set; }

		internal StarInfo(StarData data)
		{
			this.Data = data;
		}

		public Color Color
		{
			get { return this.Data.Color; }
		}

		public Vector2D Position
		{
			get { return this.Data.Position; }
		}

		public float Size
		{
			get { return this.Data.ImageSizeScale; }
		}
		
		public IStarName Name
		{
			get { return this.Data.Name; }
		}

		public IEnumerable<TraitInfo> Traits
		{
			get
			{
				return this.Data.Traits.Select(x => new TraitInfo(x.Type));
			}
		}

		public override bool Equals(object obj)
		{
			var other = obj as StarInfo;
			return other != null &&
				   EqualityComparer<StarData>.Default.Equals(this.Data, other.Data);
		}

		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		public static bool operator ==(StarInfo info1, StarInfo info2)
		{
			return EqualityComparer<StarInfo>.Default.Equals(info1, info2);
		}

		public static bool operator !=(StarInfo info1, StarInfo info2)
		{
			return !(info1 == info2);
		}
	}
}
