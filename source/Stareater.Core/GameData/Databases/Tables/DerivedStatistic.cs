using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	public class DerivedStatistic
	{
		public Formula Base { get; private set; }
		public Formula Total { get; private set; }
		
		public DerivedStatistic(Formula baseValue, Formula total)
		{
			this.Base = baseValue;
			this.Total = total;
		}
		
		public double Evaluate(IDictionary<string, double> variables)
		{
			if (variables == null)
				throw new ArgumentNullException(nameof(variables));

			variables[BaseKey] = Base.Evaluate(variables);
			
			return Total.Evaluate(variables);
		}
		
		private const string BaseKey = "base";
	}
}
