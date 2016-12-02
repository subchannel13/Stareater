using System;
using Ikadn.Ikon.Types;

namespace Stareater.Players.Natives
{
	public class OrganellePlayerFactory : IOffscreenPlayerFactory
	{
		#region IOffscreenPlayerFactory implementation
		public IOffscreenPlayer Create()
		{
			return new OrganellePlayer();
		}
		
		public IOffscreenPlayer Load(IkonComposite rawData)
		{
			return this.Create();
		}
		
		public string Id 
		{
			get { return "OrganelleAI"; }
		}
		
		public string Name 
		{
			get { return "no name"; } //TODO(later) make name
		}
		#endregion
	}
}
