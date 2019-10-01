namespace Stareater.GuiUtils
{
	class RepeatGate
	{
		private bool entered = false;
		private bool finished = false;
		private bool queued = false;

		public bool TryEnter()
		{
			if (this.entered)
			{
				this.queued = true;
				return false;
			}

			var canEnter = !this.finished || this.queued;
			if (canEnter)
			{
				this.entered = true;
				this.queued = false;

				return true;
			}

			this.finished = false;
			return false;
		}

		public void Finish()
		{
			this.entered = false;
			this.finished = true;
		}
	}
}
