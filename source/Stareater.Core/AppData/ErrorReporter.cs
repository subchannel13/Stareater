using System;

namespace Stareater.AppData
{
	public class ErrorReporter
	{
		#region Singleton
		protected static ErrorReporter instance = null;

		public static ErrorReporter Get
		{
			get
			{
				if (instance == null)
					instance = new ErrorReporter();

				return instance;
			}
		}
		#endregion

		private object lockObj = new object();
		private Action<Exception> OnExceptionHandler;

		public event Action<Exception> OnException
		{
			add
			{
				lock (lockObj)
				{
					OnExceptionHandler += value;
				}
			}
			remove
			{
				lock (lockObj)
				{
					OnExceptionHandler -= value;
				}
			}
		}

		internal void Report(Exception e)
		{
			lock (lockObj)
			{
				if (this.OnExceptionHandler != null)
					this.OnExceptionHandler(e);
			}
		}
	}
}
