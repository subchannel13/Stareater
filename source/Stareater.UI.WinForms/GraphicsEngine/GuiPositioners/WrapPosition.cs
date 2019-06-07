namespace Stareater.GraphicsEngine.GuiPositioners
{
	class WrapPosition
	{
		private readonly IWrapPositioner positioner;

		public ElementPosition Then { get; private set; }

		public WrapPosition(ElementPosition original, IWrapPositioner positioner)
		{
			this.Then = original;
			this.positioner = positioner;
		}

		public ElementPosition WithPadding(float marginX, float marginY)
		{
			this.positioner.Padding(marginX, marginY);

			return this.Then;
		}
	}
}
