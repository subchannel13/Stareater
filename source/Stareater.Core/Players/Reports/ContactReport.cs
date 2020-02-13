using Stareater.Utils.StateEngine;

namespace Stareater.Players.Reports
{
	[StateTypeAttribute(saveTag: SaveTag)]
	class ContactReport : IReport
	{
		[StatePropertyAttribute]
		public Player Owner { get; private set; }
		[StatePropertyAttribute]
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
