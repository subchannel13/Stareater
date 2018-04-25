namespace Stareater.GuiUtils
{
	class OneShotEvent
	{
		private bool triggered = false;
		private bool shouldTrigger = false;

		public void AllowEnter()
		{
			this.shouldTrigger = !triggered;
		}

		public bool TryEnter()
		{
			if (!this.shouldTrigger)
				return false;

			this.shouldTrigger = false;
			this.triggered = true;
			return true;
		}
	}
}
