using System;

namespace Stareater.AppData
{
	public class ErrorReporter
	{
		#region Singleton
		private static ErrorReporter instance = null;

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

		private readonly object lockObj = new object();
		private Action<Exception> OnExceptionHandler;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Already is an event")]
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
				this.OnExceptionHandler?.Invoke(e);
			}
		}
	}
}
