using System.Collections.Generic;
using Stareater.GameLogic;
using Stareater.Utils.Collections;
using Stareater.GameData.Construction;
using Stareater.GameLogic.Planning;

namespace Stareater.Controllers.Views
{
	public class ConstructableInfo
	{
		private readonly IDictionary<string, double> vars;
		private readonly ConstructionInfoExtractor infoExtractor;

		internal ConstructableInfo(IConstructionProject project, PlayerProcessor playerProcessor, 
			ConstructionResult progress, double stockpile)
		{
			this.Project = project;
			this.Stockpile = stockpile;

			this.infoExtractor = new ConstructionInfoExtractor(project);
            this.vars = new Var().UnionWith(playerProcessor.TechLevels).Get;
			
			if (progress != null)
			{
				this.Investment = progress.InvestedPoints;
				this.CompletedCount = progress.CompletedCount;
				this.FromStockpile = progress.FromStockpile;
				this.Overflow = progress.LeftoverPoints;
			}
		}
		
		internal ConstructableInfo(IConstructionProject data, PlayerProcessor playerProcessor) :
			this(data, playerProcessor, null, 0)
		{ }

		internal IConstructionProject Project { get; private set; }
		
		public string Name
		{
			get { return this.infoExtractor.Name; }
		}
		
		public string Description 
		{ 
			get { return this.infoExtractor.Description; }
		}
		
		public string ImagePath 
		{
			get { return this.infoExtractor.ImagePath; }
		}
				
		public bool IsVirtual
		{
			get 
			{
				return this.Project.IsVirtual;
			}
		}
		
		public double Cost
		{
			get 
			{
				return this.Project.Cost.Evaluate(vars);
			}
		}
		
		public double Investment { get; private set; }
		public double FromStockpile { get; private set; }
		public long CompletedCount { get; private set; }
		public double Overflow { get; private set; }
		
		public double Stockpile { get; private set; }
	}
}
