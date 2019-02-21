using NUnit.Framework;
using Stareater.Utils.Collections;
using System.Collections.Generic;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class SubformulaTests
	{
		[Test]
		public void SubformulaSimpleConst()
		{
			var subformulas = new Dictionary<string, string>
			{
				{"subformula", "5" }
			};
			var test = new ParserTester("2 * subformula", subformulas, null, 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void SubformulaSimpleVar()
		{
			var subformulas = new Dictionary<string, string>
			{
				{"subformula", "x ^ 2" }
			};
			var test = new ParserTester("2 * subformula", subformulas, new Var("x", 3), 18);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
