using System;
using Stareater.GameData;
using Stareater.Localization;
using Stareater.Players.Reports;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views
{
	public class TechnologyReportInfo : IReportInfo
	{
		private const string TechnologyReportKey = "tech";
		private const string topicVar = "tech";
		private const string levelVar = "level";
		
		private TechnologyReport report;
		
		internal TechnologyReportInfo(TechnologyReport report)
		{
			this.report = report;
		}
		
		public string Message {
			get {
				var topicVars = new Var(Technology.LevelKey, report.TechProgress.Item.NextLevel).Get;
				
				var vars = new TextVar(topicVar, LocalizationManifest.Get.CurrentLanguage[TechnologyTopic.LangContext][report.TechProgress.Item.Topic.NameCode].Text(topicVars)).
					And(levelVar, report.TechProgress.Item.Level.ToString()).Get;
				
				return LocalizationManifest.Get.CurrentLanguage[GameController.ReportContext][TechnologyReportKey].Text(null, vars);
			}
		}
		
		public string ImagePath
		{
			get { return report.TechProgress.Item.Topic.ImagePath; }
		}
		
		public TechnologyCategory Category
		{
			get { return this.report.TechProgress.Item.Topic.Category; }
		}
		
		public void Accept(IReportInfoVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
