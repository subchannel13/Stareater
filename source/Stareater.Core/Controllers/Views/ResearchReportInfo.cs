using System;
using Stareater.GameData;
using Stareater.Localization;
using Stareater.Players.Reports;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views
{
	public class ResearchReportInfo : IReportInfo
	{
		private const string TechnologyReportKey = "tech";
		
		private readonly ResearchReport report;
		
		internal ResearchReportInfo(ResearchReport report)
		{
			this.report = report;
		}
		
		public string Message {
			get {
				var topicVars = new Var(DevelopmentTopic.LevelKey, report.TechProgress.Item.NextLevel).Get;
				
				var vars = new TextVar("tech", LocalizationManifest.Get.CurrentLanguage[DevelopmentTopicInfo.LangContext].Name(report.TechProgress.Item.Topic.LanguageCode).Text(topicVars)).
					And("level", report.TechProgress.Item.Level.ToStringInvariant()).Get;
				
				return LocalizationManifest.Get.CurrentLanguage[GameController.ReportContext][TechnologyReportKey].Text(null, vars);
			}
		}
		
		public string ImagePath
		{
			get { return report.TechProgress.Item.Topic.ImagePath; }
		}
		
		public void Accept(IReportInfoVisitor visitor)
		{
			if (visitor == null)
				throw new ArgumentNullException(nameof(visitor));

			visitor.Visit(this);
		}
	}
}
