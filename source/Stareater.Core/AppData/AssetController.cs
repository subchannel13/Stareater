using System;
using System.Linq;

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
				Task.Factory.StartNew(task) : 
				this.lastTask.ContinueWith(t => checkCompletion(t, task));
		}
		
		private void checkCompletion(Task previousTask, Action nextTask)
		{
			if (previousTask.IsFaulted)
				System.Diagnostics.Trace.TraceError(previousTask.Exception.ToString()); //TODO(later) make user friendly variant for release
			
			nextTask();
		}
	}
}
