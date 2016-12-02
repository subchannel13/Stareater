using System;
using Ikadn.Ikon.Types;

namespace Stareater.Players.Natives
{
	public class StareaterPlayerFactory : IOffscreenPlayerFactory
	{
		internal const string FactoryId = "DefaultAI";
		
		#region IOffscreenPlayerFactory implementation
		public IOffscreenPlayer Create()
		{
			//TODO(v0.6)
			throw new NotImplementedException();
		}
		
		public IOffscreenPlayer Load(IkonComposite rawData)
		{
			return this.Create();
		}
		
		public string Id 
		{
			get { return FactoryId; }
		}
		
		public string Name 
		{
			get { return "no name"; } //TODO(later) make name
		}
		#endregion
	}
}
