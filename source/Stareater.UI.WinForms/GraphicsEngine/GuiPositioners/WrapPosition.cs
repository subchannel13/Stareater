using OpenTK;

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

		public ElementPosition WithPadding(float paddingX, float paddingY)
		{
			return this.WithPadding(new ValueReference<Vector2>(new Vector2(paddingX, paddingY)));
		}

		public ElementPosition WithPadding(ValueReference<Vector2> padding)
		{
			this.positioner.Padding(padding);

			return this.Then;
		}
	}
}
