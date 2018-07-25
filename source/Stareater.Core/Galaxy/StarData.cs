using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Stareater.Localization.StarNames;
using Stareater.Galaxy.BodyTraits;
using Ikadn;
using Stareater.Utils;

namespace Stareater.Galaxy
{
	class StarData 
	{
		[StateProperty]
		public Color Color { get; private set; }

		[StateProperty]
		public float ImageSizeScale { get; private set; }

		[StateProperty]
		public IStarName Name { get; private set; }

		[StateProperty]
		public Vector2D Position { get; private set; }

		[StateProperty]
		public PendableSet<ITrait> Traits { get; private set; }

		public StarData(Color color, float imageSizeScale, IStarName name, Vector2D position, List<TraitType> traits)
		{
			this.Color = color;
			this.ImageSizeScale = imageSizeScale;
			this.Name = name;
			this.Position = position;
			this.Traits = new PendableSet<ITrait>(traits.Select(x => x.Make()));
		}

		private StarData() 
		{ }

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as StarData;
			return other != null && object.Equals(this.Position, other.Position);
		}

		public override int GetHashCode()
		{
			return this.Position.GetHashCode();
		}

		public static bool operator ==(StarData lhs, StarData rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(StarData lhs, StarData rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

		private static IStarName loadName(IkadnBaseObject rawData, LoadSession session)
		{
			return rawData.Tag.Equals(ConstellationStarName.SaveTag) ?
				(IStarName)session.Load<ConstellationStarName>(rawData) :
				(IStarName)session.Load<ProperStarName>(rawData);
		}

		public override string ToString()
		{
			return this.Position.X.ToString("0.0") + "; " + this.Position.Y.ToString("0.0");
		}
	}
}
