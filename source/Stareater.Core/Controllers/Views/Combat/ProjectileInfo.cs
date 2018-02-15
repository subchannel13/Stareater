using NGenerics.DataStructures.Mathematical;
using Stareater.SpaceCombat;

namespace Stareater.Controllers.Views.Combat
{
	public class ProjectileInfo
	{
		internal readonly Projectile Data;

		internal ProjectileInfo(Projectile data)
		{
			this.Data = data;
		}

		public Vector2D Position
		{
			get { return this.Data.Position; }
		}

		public PlayerInfo Owner
		{
			get { return new PlayerInfo(this.Data.Owner); }
		}

		public string ImagePath
		{
			get { return this.Data.Stats.ProjectileImage; }
		}

		public long Count
		{
			get { return this.Data.Count; }
		}
	}
}
