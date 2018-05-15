using System.Globalization;
using NUnit.Framework;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class TermTests
	{
		[Test]
		public void Brackets()
		{
			var test = new ParserTester("2 * (1 + 3)", null, 8);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void InfinityConstant()
		{
			var test = new ParserTester("Inf", null, double.PositiveInfinity);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void IntegerConstant()
		{
			double input = 2;

			var test = new ParserTester(input.ToString(), null, input);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void DecimalConstant()
		{
			double input = 0.6;

			var test = new ParserTester(input.ToString(CultureInfo.InvariantCulture), null, input, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void SciBigIntegerConstant()
		{
			double input = 8e9;

			var test = new ParserTester(input.ToString(), null, input, 1e-6);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void SciBigDecimalConstant()
		{
			double input = 8.6e9;

			var test = new ParserTester(input.ToString(), null, input, 1e-6);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void SciSmallIntegerConstant()
		{
			double input = 4e-6;

			var test = new ParserTester(input.ToString(), null, input, 1e-12);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void SciSmallDecimalConstant()
		{
			double input = 3.14e-6;

			var test = new ParserTester(input.ToString(CultureInfo.InvariantCulture), null, input, 1e-12);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void Variable()
		{
			string varName = "lvl";
			double varValue = 3.14;

			var test = new ParserTester(varName, new Var(varName, varValue).Get, varValue);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void VariableWithUnderscore()
		{
			string varName = "hydro_lvl";
			double varValue = 4;

			var test = new ParserTester(varName, new Var(varName, varValue).Get, varValue);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
