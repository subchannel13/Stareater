using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Views
{
	public class SystemPreview
	{
		public Vector2D Position { get; private set; }
		public bool IsHomeSystem { get; private set; }

		internal SystemPreview(StarData star, bool isHomeSystem)
		{
			this.IsHomeSystem = isHomeSystem;
			this.Position = star.Position;
		}
	}
}
