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
		[StatePropertyAttribute]
		public Color Color { get; private set; }

		[StatePropertyAttribute]
		public float ImageSizeScale { get; private set; }

		[StatePropertyAttribute]
		public IStarName Name { get; private set; }

		[StatePropertyAttribute]
		public Vector2D Position { get; private set; } //TODO(v0.8) restrict to stars only

		[StatePropertyAttribute]
		public PendableSet<IStarTrait> Traits { get; private set; }

		public StarData(Color color, float imageSizeScale, IStarName name, Vector2D position, List<StarTraitType> traits)
		{
			this.Color = color;
			this.ImageSizeScale = imageSizeScale;
			this.Name = name;
			this.Position = position;
			this.Traits = new PendableSet<IStarTrait>(traits.Select(x => x.Make()));
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "For debug only")]
		public override string ToString()
		{
			return this.Position.X.ToString("0.0") + "; " + this.Position.Y.ToString("0.0");
		}
	}
}
