using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;
using System.Threading;

namespace Stareater.Controllers
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

		private HashSet<Func<IEnumerable<double>>> Loaders = new HashSet<Func<IEnumerable<double>>>(new Func<IEnumerable<double>>[] {
			Organization.Loader,
		});
		private Thread workerThread;

		public double Progress { get; private set; }

		private AssetController()
		{
			this.workerThread = new Thread(backgroundWork);
			
			this.Progress = 0;
		}

		public void RegisterLoader(Func<IEnumerable<double>> loaderMethod)
		{
			lock (this) {
				Loaders.Add(loaderMethod);
			}
		}

		public void Start()
		{
			workerThread.Start();
		}

		private void backgroundWork()
		{
			Func<IEnumerable<double>>[] loaders;
			lock (this)
				loaders = Loaders.ToArray();

			for (int i = 0; i < loaders.Length; i++)
				foreach (var unitProgress in loaders[i]())
					Progress = (i + unitProgress) / loaders.Length;

			Progress = 1;
		}
	}
}
