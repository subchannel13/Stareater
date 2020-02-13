using Stareater.Utils.StateEngine;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData
{
	class ColonizationProject 
	{
		[StatePropertyAttribute]
		public Player Owner { get; private set; }

		[StatePropertyAttribute]
		public Planet Destination { get; private set; }

		public ColonizationProject(Player owner, Planet destination) 
		{
			this.Owner = owner;
			this.Destination = destination;
 		} 
		
		private ColonizationProject() 
		{ }
	}
}
