using System;
using Stareater.GameData;
using Stareater.Localization;
using Stareater.Players.Reports;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views
{
	public class ResearchReportInfo : IReportInfo
	{
		private const string TechnologyReportKey = "tech";
		private const string topicVar = "tech";
		private const string levelVar = "level";
		
		private ResearchReport report;
		
		internal ResearchReportInfo(ResearchReport report)
		{
			this.report = report;
		}
		
		public string Message {
			get {
				var topicVars = new Var(DevelopmentTopic.LevelKey, report.TechProgress.Item.NextLevel).Get;
				
				var vars = new TextVar(topicVar, LocalizationManifest.Get.CurrentLanguage[DevelopmentTopicInfo.LangContext].Name(report.TechProgress.Item.Topic.LanguageCode).Text(topicVars)).
					And(levelVar, report.TechProgress.Item.Level.ToString()).Get;
				
				return LocalizationManifest.Get.CurrentLanguage[GameController.ReportContext][TechnologyReportKey].Text(null, vars);
			}
		}
		
		public string ImagePath
		{
			get { return report.TechProgress.Item.Topic.ImagePath; }
		}
		
		public void Accept(IReportInfoVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
