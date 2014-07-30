using System;
using System.Collections.Generic;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameData
{
	partial class StarIntelligence
	{
		public const int NeverVisited = -1;

		public bool IsVisited
		{
			get { return LastVisited != NeverVisited; }
		}
		
		public void Visit(int turn)
		{
			this.LastVisited = turn;
		}
	}
}
