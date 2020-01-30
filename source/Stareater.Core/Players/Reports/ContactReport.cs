using Stareater.Utils.StateEngine;

namespace Stareater.Players.Reports
{
	[StateType(saveTag: SaveTag)]
	class ContactReport : IReport
	{
		[StateProperty]
		public Player Owner { get; private set; }
		[StateProperty]
		public Player Contact { get; private set; }

		public ContactReport(Player owner, Player contact)
		{
			this.Owner = owner;
			this.Contact = contact;
		}

		private ContactReport()
		{ }

		public void Accept(IReportVisitor visitor)
		{
			visitor.Visit(this);
		}

		public const string SaveTag = "ContactReport";
	}
}
