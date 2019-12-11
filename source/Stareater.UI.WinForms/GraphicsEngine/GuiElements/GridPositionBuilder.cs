using Stareater.GraphicsEngine.GuiPositioners;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GridPositionBuilder
	{
		private readonly int columns;
		private readonly float elementWidth;
		private readonly float elementHeight;
		private readonly float elementSpacing;

		private int row = 0;
		private int column = 0;

		public GridPositionBuilder(int columns, float elementWidth, float elementHeight, float elementSpacing)
		{
			this.columns = columns;
			this.elementWidth = elementWidth;
			this.elementHeight = elementHeight;
			this.elementSpacing = elementSpacing;
		}

		public void Add(ElementPosition position)
		{
			if (this.row != 0 || this.column != 0)
				position.Offset((this.elementHeight + this.elementSpacing) * this.column, -(this.elementWidth + this.elementSpacing) * this.row);

			this.column++;
			if (column >= this.columns)
			{
				this.column = 0;
				this.row++;
			}
		}

		internal void Restart()
		{
			this.column = 0;
			this.row = 0;
		}
	}
}
