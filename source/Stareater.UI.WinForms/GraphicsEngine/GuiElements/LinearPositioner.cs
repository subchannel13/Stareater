using OpenTK;

namespace Stareater.GraphicsEngine.GuiElements
{
	class LinearPositioner
	{
		private readonly Vector2 direction;
		private Vector2? margin;
		private IGuispaceElement lastElement;

		public LinearPositioner(bool horizontal)
		{
			this.direction = horizontal ? new Vector2(1, 0) : new Vector2(0, -1);
		}

		public LinearPositioner(bool horizontal, IGuispaceElement lastElement) : this(horizontal)
		{
			this.lastElement = lastElement;
		}

		public void Add(AGuiElement element)
		{
			var x = this.direction.X;
			var y = this.direction.Y;

			if (this.lastElement != null)
			{
				var positionBuilder = element.Position.RelativeTo(this.lastElement, x, y, -x, -y);
				if (this.margin.HasValue)
					element.Margins += this.margin.Value;
				if (element.Margins.Length > 0)
					positionBuilder.UseMargins();
			}
			else
				element.Position.ParentRelative(-x, -y);

			this.lastElement = element;
			this.margin = null;
		}

		internal void AddSpace(int margin)
		{
			this.margin = this.direction * margin;
		}
	}
}
