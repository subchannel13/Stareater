using NUnit.Framework;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class FunctionTests
	{
		[Test]
		public void FunctionAbsNegative()
		{
			var test = new ParserTester("abs(-2.1)", null, null, 2.1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionAbsPositive()
		{
			var test = new ParserTester("abs(5.2)", null, null, 5.2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionAbsVar()
		{
			var test = new ParserTester("abs(x)", null, new Var("x", -1), 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionCeilInteger()
		{
			var test = new ParserTester("Ceil(4)", null, null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionCeilNegative()
		{
			var test = new ParserTester("Ceil(-2.1)", null, null, -2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionCeilPositive()
		{
			var test = new ParserTester("Ceil(5.2)", null, null, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionCeilVar()
		{
			var test = new ParserTester("Ceil(x)", null, new Var("x", 5.2), 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionFloorInteger()
		{
			var test = new ParserTester("floor(4)", null, null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionFloorNegative()
		{
			var test = new ParserTester("floor(-2.1)", null, null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionFloorPositive()
		{
			var test = new ParserTester("floor(5.2)", null, null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionFloorVar()
		{
			var test = new ParserTester("floor(x)", null, new Var("x", 5.2), 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteElse()
		{
			var test = new ParserTester("if(-1, 10, 20)", null, null, 20);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteElseVar()
		{
			var test = new ParserTester("if(c, x, y)", null, new Var("c", -1).And("x", 10).And("y", 20), 20);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteSimplificationElse()
		{
			var test = new ParserTester("if(-1, x, y)", null, new Var("y", 20), 20);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteSimplificationThen()
		{
			var test = new ParserTester("if(1, x, y)", null, new Var("x", 10), 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteThen()
		{
			var test = new ParserTester("if(1, 10, 20)", null, null, 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteThenVar()
		{
			var test = new ParserTester("if(c, x, y)", null, new Var("c", 1).And("x", 10).And("y", 20), 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteZero()
		{
			var test = new ParserTester("if(0, 10, 20)", null, null, 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void FunctionCaseOneTrue()
		{
			var test = new ParserTester("case(0)", null, null, 0);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void FunctionCaseOneFalse()
		{
			var test = new ParserTester("case(-1)", null, null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void FunctionCaseTwoFirst()
		{
			var test = new ParserTester("case(1, 1)", null, null, 0);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void FunctionCaseTwoSecond()
		{
			var test = new ParserTester("case(-1, 1)", null, null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void FunctionCaseTwoNone()
		{
			var test = new ParserTester("case(-1, -1)", null, null, 2);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void FunctionLimitGreater()
		{
			var test = new ParserTester("limit(10, 1, 3)", null, null, 3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionLimitInside()
		{
			var test = new ParserTester("limit(2, 1, 3)", null, null, 2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionLimitLower()
		{
			var test = new ParserTester("limit(-2, 1, 3)", null, null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionLimitVar()
		{
			var test = new ParserTester("limit(x, a, b)", null, new Var("x", 10).And("a", 1).And("b", 3), 3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMax2ParamsConst()
		{
			var test = new ParserTester("max(2, 5)", null, null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMax2ParamsVar()
		{
			var test = new ParserTester("max(x, y)", null, new Var("x", 3).And("y", 5), 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMax5Params()
		{
			var test = new ParserTester("max(-2, 10.1, -3, 5, 1)", null, null, 10.1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMin2Params()
		{
			var test = new ParserTester("min(2, 5)", null, null, 2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMin2ParamsVar()
		{
			var test = new ParserTester("min(x, y)", null, new Var("x", 3).And("y", 5), 3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMin5Params()
		{
			var test = new ParserTester("min(-2, 2, -3, 5, 1.5)", null, null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundInteger()
		{
			var test = new ParserTester("round(4)", null, null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundNegativeDown()
		{
			var test = new ParserTester("round(-2.7)", null, null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundNegativeMidpointEven()
		{
			var test = new ParserTester("round(-2.5)", null, null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundNegativeMidpointOdd()
		{
			var test = new ParserTester("round(-3.5)", null, null, -4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundNegativeUp()
		{
			var test = new ParserTester("round(-2.1)", null, null, -2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundPositiveDown()
		{
			var test = new ParserTester("round(5.2)", null, null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundPositiveMidpointEven()
		{
			var test = new ParserTester("round(4.5)", null, null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundPositiveMidpointOdd()
		{
			var test = new ParserTester("round(5.5)", null, null, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundPositiveUp()
		{
			var test = new ParserTester("round(5.7)", null, null, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundVar()
		{
			var test = new ParserTester("round(x)", null, new Var("x", 5.7), 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionSngNegative()
		{
			var test = new ParserTester("sgn(-4)", null, null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionSngPositive()
		{
			var test = new ParserTester("sgn(2)", null, null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionSngVar()
		{
			var test = new ParserTester("sgn(x)", null, new Var("x", 5.7), 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionSngZero()
		{
			var test = new ParserTester("sgn(0)", null, null, 0);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionTruncInteger()
		{
			var test = new ParserTester("Trunc(4)", null, null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionTruncNegative()
		{
			var test = new ParserTester("Trunc(-2.1)", null, null, -2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionTruncPositive()
		{
			var test = new ParserTester("Trunc(5.2)", null, null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionTruncVar()
		{
			var test = new ParserTester("Trunc(x)", null, new Var("x", 5.7), 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void FunctionRatioConst()
		{
			var test = new ParserTester("Ratio(1, 3)", null, null, 0.25);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void FunctionRatioVar()
		{
			var test = new ParserTester("Ratio(x, y)", null, new Var("x", 2).And("y", 3), 0.4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
