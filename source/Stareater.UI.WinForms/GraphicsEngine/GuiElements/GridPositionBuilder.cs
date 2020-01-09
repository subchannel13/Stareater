using Stareater.GraphicsEngine.GuiPositioners;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GridPositionBuilder
	{
		public int Columns { get; private set; }
		public float ElementWidth { get; private set; }
		public float ElementHeight { get; private set; }
		public float ElementSpacing { get; private set; }

		private int row = 0;
		private int column = 0;

		public GridPositionBuilder(int columns, float elementWidth, float elementHeight, float elementSpacing)
		{
			this.Columns = columns;
			this.ElementWidth = elementWidth;
			this.ElementHeight = elementHeight;
			this.ElementSpacing = elementSpacing;
		}

		public void Add(ElementPosition position)
		{
			if (this.row != 0 || this.column != 0)
				position.Offset((this.ElementWidth + this.ElementSpacing) * this.column, -(this.ElementHeight + this.ElementSpacing) * this.row);

			this.column++;
			if (column >= this.Columns)
			{
				this.column = 0;
				this.row++;
			}
		}

		public void Restart()
		{
			this.column = 0;
			this.row = 0;
		}
	}
}
