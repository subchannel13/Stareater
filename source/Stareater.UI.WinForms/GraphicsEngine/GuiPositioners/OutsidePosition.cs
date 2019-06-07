namespace Stareater.GraphicsEngine.GuiPositioners
{
	class OutsidePosition
	{
		private readonly IOutsidePositioner positioner;

		public ElementPosition Then { get; private set; }

		public OutsidePosition(ElementPosition original, IOutsidePositioner positioner)
		{
			this.Then = original;
			this.positioner = positioner;
		}

		public ElementPosition WithMargins(float marginX, float marginY)
		{
			this.positioner.Margins(marginX, marginY);

			return this.Then;
		}
	}
}
