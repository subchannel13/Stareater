using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stareater.AppData
{
	public class AssetController
	{
		#region Singleton
		static AssetController instance = null;

		public static AssetController Get
		{
			get
			{
				if (instance == null)
					instance = new AssetController();
				return instance;
			}
		}
		#endregion

		private Task lastTask = null;

		public void AddLoader(Action loaderMethod)
		{
			this.appendTask(loaderMethod);
		}
		
		public void AddLoader(Action loaderMethod, Action completionCallback)
		{
			this.appendTask(loaderMethod);
			this.appendTask(completionCallback);
		}

		private void appendTask(Action task)
		{
			this.lastTask = lastTask == null ? 
				Task.Factory.StartNew(task, Task.Factory.CancellationToken, TaskCreationOptions.None, TaskScheduler.Default) : 
				this.lastTask.ContinueWith(t => checkCompletion(t, task), TaskScheduler.Default);
		}
		
		private static void checkCompletion(Task previousTask, Action nextTask)
		{
			if (previousTask.IsFaulted)
				ErrorReporter.Get.Report(previousTask.Exception);
			
			nextTask();
		}
	}
}
