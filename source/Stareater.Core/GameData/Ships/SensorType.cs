﻿using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Ships
{
	class SensorType : AComponentType, IIncrementalComponent
	{
		public string ImagePath { get; private set; }
		
		public Formula Detection;
		
		public SensorType(string code, string nameCode, string descCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                 Formula detection)
			: base(code, nameCode, descCode, prerequisites, maxLevel)
		{
			this.Detection = detection;
			this.ImagePath = imagePath;
		}

		#region IIncrementalComponent implementation
		public double ComparisonValue(int level)
		{
			return this.Detection.Evaluate(new Var(AComponentType.LevelKey, level).Get);
		}
		#endregion
	}
}
