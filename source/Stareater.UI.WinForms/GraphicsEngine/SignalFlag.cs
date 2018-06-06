using System.Threading;

namespace Stareater.GraphicsEngine
{
	public class SignalFlag
	{
		private const int IntTrue = 1;
		private const int IntFalse = -1;
		
		private int state = IntFalse;
		
		public bool Check()
		{
			return Interlocked.CompareExchange(ref this.state, IntFalse, IntTrue) == IntTrue;
		}
		
		public void Clear()
		{
			this.state = IntFalse;
		}
		
		public void Set()
		{
			this.state = IntTrue;
		}
	}
}
